using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectMover : MonoBehaviour {
    float swipeDistance;
    private Vector2 startPos, endPos;

    int count;

    private Vector3 newPosition;
    private Vector3 lastPos;

    [SerializeField] private Dictionary<int, GameObject> movObj;
    [SerializeField] private Dictionary<int, SlidingBuffer<Vector2>> pastPoints;
    [SerializeField] private Dictionary<int, SlidingBuffer<float>> swipeTime;

    void Start() {
        movObj = new Dictionary<int, GameObject>();
        pastPoints = new Dictionary<int, SlidingBuffer<Vector2>>(); //make new SB when a new unique finger ID is registered
        swipeTime = new Dictionary<int, SlidingBuffer<float>>();
    }

    void Update() {
        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.touches[i];
            //float theta = Mathf.Atan2(touch.deltaPosition.y, touch.deltaPosition.x);
            //theta = theta * 180f / (float)Math.PI;
            //Debug.Log(count + ": DP: " + touch.deltaPosition + " DT: " + touch.deltaTime + " Th: " + theta);

            if (touch.phase == TouchPhase.Began) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Moveable") && hit.rigidbody) {
                    // Begin to move ball if it has the right tag and a rigidbody
                    // putting the moveable game object into an indexed array based on unique touch ID
                    movObj[touch.fingerId] = hit.transform.gameObject;
                    startPos = touch.position;
                    hit.rigidbody.useGravity = false;       // No gravity for now so it doesn't stutter

                    Rigidbody rb = movObj[touch.fingerId].GetComponent<Rigidbody>();
                    if (movObj[touch.fingerId].name.Contains("Sphere")) {
                        StopCoroutine(ResetBall(8f, rb, movObj[touch.fingerId]));  // Comment this out later, just so the ball returns
                    }
                    if (movObj[touch.fingerId].name.Contains("Cube")) {
                        StopCoroutine(ResetCube(8f, rb, movObj[touch.fingerId]));  // Comment this out later, just so the ball returns
                    }
                }

                // Initializing Dictionaries
                if (!pastPoints.ContainsKey(touch.fingerId)) {
                    pastPoints[touch.fingerId] = new SlidingBuffer<Vector2>(10);
                }
                if (!swipeTime.ContainsKey(touch.fingerId)) {
                    swipeTime[touch.fingerId] = new SlidingBuffer<float>(10);
                }
            }
            else if (touch.phase == TouchPhase.Moved) {
                pastPoints[touch.fingerId].Add(touch.position);
                swipeTime[touch.fingerId].Add(touch.deltaTime);
                PickupBall(touch);
            }
            else if (touch.phase == TouchPhase.Stationary) {
                // Once someone swipes a certain distance, stopping for a little while won't reset their swipe distance
                pastPoints[touch.fingerId].Add(touch.position);
                swipeTime[touch.fingerId].Add(touch.deltaTime);
                PickupBall(touch);
            }
            else if (touch.phase == TouchPhase.Ended && movObj.ContainsKey(touch.fingerId)) {
                //Debug.Log("**********************************");
                // Throw the ball
                ThrowBall(touch);
                pastPoints[touch.fingerId].Clear();
                swipeTime[touch.fingerId].Clear();
            }
            ++count;
        }
    }

    void PickupBall(Touch touch) {
        if (movObj.ContainsKey(touch.fingerId)) {
            GameObject go = movObj[touch.fingerId];         // Object associated with that finger ID
            Vector3 touchPos = touch.position;
            touchPos.z = Camera.main.nearClipPlane * 20f;   // Move the object 20 units away from the camera in the z direction
            newPosition = Camera.main.ScreenToWorldPoint(touchPos);
            go.transform.localPosition = newPosition;       // Move the object to that position
        }

    }

    private void ThrowBall(Touch touch) {
        endPos = touch.position;
        swipeDistance = (endPos - startPos).magnitude;
        //Debug.Log("Swipe Distance" + swipeDistance);

        Rigidbody rb = movObj[touch.fingerId].GetComponent<Rigidbody>();
        if (swipeDistance > 10f) {
            //throw ball
            Vector3 vec = CalculateBallForce(touch);
            rb.AddForce(vec);
            rb.useGravity = true;

            lastPos = movObj[touch.fingerId].transform.position;
            if (movObj[touch.fingerId].name.Contains("Sphere")) {
                StartCoroutine(ResetBall(8f, rb, movObj[touch.fingerId]));  // Comment this out later, just so the ball returns
            }
            if (movObj[touch.fingerId].name.Contains("Cube")) {
                StartCoroutine(ResetCube(8f, rb, movObj[touch.fingerId]));  // Comment this out later, just so the ball returns
            }
        }
        else {
            // You just drop the ball bc it didn't move
            rb.useGravity = true;
        }
        movObj.Remove(touch.fingerId);
    }

    private Vector3 CalculateBallForce(Touch touch) {
        (Vector2 point1, Vector2 point2, float dTime) = LinearRegression(touch);
        Vector2 force = point2 - point1;

        //Debug.Log("Before: X: " + forces.x + " Y: " + forces.y + " Z: " + forces.magnitude / Mathf.Ceil(Mathf.Abs(forces.x / forces.y)));
        if (Mathf.Abs(force.x) > force.y)
            force.x *= 2f;

        //Debug.Log("Dtime: " + dTime);
        //Debug.Log("After: X: " + forces.x + " Y: " + forces.y + " Z: " + forces.magnitude / Mathf.Ceil(Mathf.Abs(forces.x / forces.y)));
        return new Vector3(force.x, force.y, force.magnitude) * 2f;
        //return new Vector3(forces.x, forces.y, forces.magnitude / Mathf.Ceil(Mathf.Abs(forces.x / forces.y)));
    }

    public (Vector2, Vector2, float) LinearRegression(Touch touch) {

        Vector2 means = CalculateMeanPastPoints(touch);
        //Debug.Log("PPMeans: " +  means + " PPCount: " + pastPoints[touch.fingerId].Count());

        // Calculate the sums for the linear regression formula
        float sumXY = 0;
        float sumX2 = 0;

        float xPointZero = 0f;
        float xPointN = 0f;

        int count = 0;

        // b = Σ(x-xMean)*(y-yMean)  /  Σ(x-xMean)^2
        foreach (var point in pastPoints[touch.fingerId]) {
            if (count == 0) {
                xPointZero = point.x;
                Debug.Log("Point0: " + point);
            }

            if (count == pastPoints[touch.fingerId].Count() - 1) {
                xPointN = point.x;
                Debug.Log("PointN: " + point);
            }

            float xDiff = point.x - means.x;
            float yDiff = point.y - means.y;
            sumXY += xDiff * yDiff;
            sumX2 += xDiff * xDiff;

            count++;
        }

        // Calculate total time taken to throw ball of number of points in swipeTime
        float dTime = 0;
        foreach (var time in swipeTime[touch.fingerId]) {
            dTime += time;
        }

        // Calculate the slope (m) of the line
        float slope = sumXY / sumX2;

        // Calculate the y-intercept (b) of the line
        float yIntercept = means.y - slope * means.x;

        //Debug.Log("xP0: " + xPointZero + " xPn: " + xPointN + " Slp: " + slope + " yInt: " + yIntercept);

        Vector2 point1 = new(xPointZero, (slope * xPointZero) + yIntercept);
        Vector2 point2 = new(xPointN, (slope * xPointN) + yIntercept);

        return (point1, point2, dTime);
    }

    public Vector2 CalculateMeanPastPoints(Touch touch) {
        Vector2 sum = Vector2.zero;

        //CalculateVectorAngle(touch);

        if (pastPoints[touch.fingerId].Count() == 0)
            throw new NullReferenceException(); // Nothing in past points, should not happen?

        foreach (var point in pastPoints[touch.fingerId]) {
            sum += point;
        }

        return sum / pastPoints[touch.fingerId].Count();
    }

    private void CalculateVectorAngle(Touch touch) {
        Vector2[] pp = pastPoints[touch.fingerId].CopyTo(); // This works
        /*string s = "";
        foreach (Vector2 p in pp) {
            s += p + ", ";
        }
        Debug.Log(s);*/
        int index = 0;
        for (int i = 0; i < pp.Length - 2; i++) {
            Vector2 v1 = pp[i] - pp[i + 1];
            Vector2 v2 = pp[i + 2] - pp[i + 1];
            float angle = Mathf.Acos(Vector2.Dot(v1, v2) / (v1.magnitude * v2.magnitude));
            angle = (float)(angle * 180 / Math.PI);
            Debug.Log("v1: " + v1 + " v2: " + v2 + " Angle " + (i + 1) + ": " + angle);

            // This is outside of the range of what we want
            if (angle < 165f && !(i + 1 > pp.Length - 5)) {
                index = i + 1;
            }
        }
        Debug.Log("Index: " + index);
        Queue<Vector2> q = new Queue<Vector2>();
        for (int i = index; i < pp.Length; i++) {
            q.Enqueue(pp[i]);
        }
        pastPoints[touch.fingerId].SetQueue(q);

    }

    public void PrintQueue(Touch touch) {
        string vals = "";
        foreach (var point in pastPoints[touch.fingerId]) {
            vals += point.x + "," + point.y + "\n";
        }
        Debug.Log(vals);
    }

    IEnumerator ResetBall(float delay, Rigidbody rb, GameObject Ball) { // Can remove this later
        yield return new WaitForSeconds(delay);
        endPos = Vector2.zero;
        startPos = Vector2.zero;
        swipeDistance = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;
        Ball.transform.position = lastPos; // Can remove this later
    }

    IEnumerator ResetCube(float delay, Rigidbody rb, GameObject Ball) { // Can remove this later
        yield return new WaitForSeconds(delay);
        endPos = Vector2.zero;
        startPos = Vector2.zero;
        swipeDistance = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;
        Ball.transform.position = lastPos; // Can remove this later
    }

    class SlidingBuffer<T> : IEnumerable<T> {
        private Queue<T> _queue;
        private readonly int _maxCount;

        public SlidingBuffer(int maxCount) {
            _maxCount = maxCount;
            _queue = new Queue<T>(maxCount);
        }

        public void Add(T item) {
            if (_queue.Count == _maxCount)
                _queue.Dequeue();
            _queue.Enqueue(item);
        }

        public IEnumerator<T> GetEnumerator() {
            return _queue.GetEnumerator();
        }

        public T Peek() {
            return _queue.Peek();
        }

        public int Count() {
            return _queue.Count;
        }

        public void Clear() {
            _queue.Clear();
        }

        public T[] CopyTo() {
            T[] newArr = new T[_queue.Count];
            _queue.CopyTo(newArr, 0);
            return newArr;
        }

        public void SetQueue(Queue<T> q) {
            _queue = q;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

}
