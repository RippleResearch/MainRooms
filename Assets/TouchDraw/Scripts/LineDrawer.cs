using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineDrawer : MonoBehaviour
{
    // A reference to the line prefab game object.
   // public GameObject linePrefab;
    public GameObject currentBrush;

    // A reference to the current line game object.
    //private GameObject _currentLine;

    // A reference to the LineRenderer component of the current line game object.
    //private LineRenderer _lineRenderer;
    //private ParticleSystem _particleSystem;

    //private GameObject[] brushStrokes;
    //private ParticleSystem[] particleSystems;

    //private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    private ParticleSystem[] particleSystems = new ParticleSystem[5];

    // A reference to the EdgeCollider2D component of the current line game object.
    //private EdgeCollider2D _edgeCollider;

    // A list to store the touch positions of the line.
    //private List<Vector2> _touchPositions = new List<Vector2>();

    // A constant value to set the sensitivity threshold for creating a new line segment.
    //private const float SensitivityThreshold = 0.3f;


    void Start(){
    }

    // private void FixedUpdate(){

    //     CheckTouches();
    // }

    private void Update()
    {

        //if(!EventSystem.current.IsPointerOverGameObject()){
            if(currentBrush!= null){
                CheckTouches();
            }
            else{
                Debug.Log("No Brush Selected");
            }
        //}


        //check touches


        //CheckTouches();
        // if(Input.touchCount>0){
        //     foreach(Touch touch in Input.touches){
        //         Debug.Log(touch.fingerId);
        //         GameObject line = null;
        //         //Instantiate(currentBrush,Vector2.zero, Quaternion.identity);
        //         switch(touch.phase){
        //             case Touch.Began:
        //                 line = Instantiate(currentBrush,Vector2.0,Quaternion.identity);
        //             case Touch.Moved:
                        

        //         }
                
                //GameObject newLine = null;
                //ParticleSystem particle; 
                            
                // if (touch.phase==TouchPhase.Began){
                //     newLine = Instantiate(currentBrush, Vector3.zero, Quaternion.identity);
                //     particle = newLine.GetComponent<ParticleSystem>();
                // }
                // else if (touch.phase==TouchPhase.Moved){
                //    if(currentBrush!=null){
                //         Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                //         newLine.transform.position = touchPosition;
                //     }
                //     else{
                //         Debug.Log("No Brush Selected");
                //     }
                // }
            //}
        //}

        // Check if mouse button 0 (left mouse button) is pressed down.
        // if (Input.GetMouseButtonDown(0))
        // {
        //     // Call the HandleClickDown method.
        //     HandleClickDown();
        // }
        // // Check if mouse button 0 (left mouse button) is held down.
        // else if (Input.GetMouseButton(0))
        // {
        //     // Call the HandleMoving method.
        //     HandleMoving();
        // }
        // // Check if mouse button 0 (left mouse button) is released.
        // else if (Input.GetMouseButtonUp(0))
        // {
        //     // Call the HandleClickUp method.
        //     HandleClickUp();
        // }
    }


    // private void CheckTouches(){
    //     Debug.Log("Checking Touches");
    //     if(Input.touchCount>0){
    //         for(int i=0;i<Input.touchCount;i++){
    //             //Debug.Log(Input.GetTouch(i).fingerId);
    //             if(currentBrush!=null){
    //                 if(touch.phase==TouchPhase.Began){
    //                     DrawNewLine(Input.GetTouch(i));
    //                 }
    //                 else if(touch.phase==TouchPhase.Moved||touch.phase==TouchPhase.Stationary){
    //                     brushstrokes[touch.fingerId].GetComponent
    //                 }
    //             }
    //             else{
    //                 Debug.Log("No Brush Selected");
    //             }
    //         }
    //     }
    //     // if(Input.GetMouseButtonDown(0)){
    //     //     Debug.Log("New line created");
    //     //     _currentLine = Instantiate(currentBrush,Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity);
    //     //     _particleSystem = _currentLine.GetComponent<ParticleSystem>();
    //     // }
    // }

    private void DrawNewLine(Touch touch){
        int id = touch.fingerId;
        GameObject newLine = Instantiate(currentBrush,Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity);
        ParticleSystem newParticle = newLine.GetComponent<ParticleSystem>();
        //brushStrokes[id] = newLine;
        particleSystems[id] = newParticle;
        //particleSystems.Add(newParticle); //problem is how to access this specific particle system after this method? can't add to list by index
    }

    private void CheckTouches(){
        Debug.Log("Checking Touches");
        if(Input.touchCount>0){
            for(int i=0;i<Input.touchCount;i++){
                //Debug.Log(Input.GetTouch(i).fingerId);
                Touch touch = Input.GetTouch(i);
                Debug.Log(i + " " + touch.fingerId);

                if(touch.phase==TouchPhase.Began){
                    Debug.Log("Starting New Line");
                    if(!EventSystem.current.IsPointerOverGameObject(touch.fingerId)){ //so you don't draw through the UI
                        DrawNewLine(touch);
                    }
                }
                else if(touch.phase==TouchPhase.Moved){//||touch.phase==TouchPhase.Stationary){
                    Debug.Log("Touch is moving");
                    particleSystems[touch.fingerId].transform.position = Camera.main.ScreenToWorldPoint(touch.position);
                    
                }
                else if(touch.phase==TouchPhase.Ended){
                    //Destroy(particleSystems[touch.fingerId]);
                    Debug.Log("The touch has ended"); 

                }
            }

        
        }
    }

    // private void DrawNewLine(Touch touch){
    //     int id = touch.fingerId;
    //     Debug.Log("Draw New Line");
    //         if(touch.phase == TouchPhase.Began){
    //             Debug.Log("New line created");
    //             //_currentLine = Instantiate(currentBrush,Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity);
    //             GameObject newLine = Instantiate(currentBrush,Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity);
    //             brushStrokes[id] = newLine;
    //             //_particleSystem = _currentLine.GetComponent<ParticleSystem>();

    //         }
    //         else if(touch.phase==TouchPhase.Moved){ //and stationary?
    //             Debug.Log("Line " + id + "  is Moving");
    //            // Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //             brushStrokes[id].GetComponent<ParticleSystem>().transform.position = Camera.main.ScreenToWorldPoint(touch.position);

    //             //_particleSystem.transform.position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
    //         }
    //         else if(touch.phase==TouchPhase.Ended){
    //             Debug.Log("Touch " + id + " has ended");
    //         }
    // }







    // private void HandleMoving()
    // {
    //     // Get the current mouse position in world space.
    //     Vector2 tempTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //     // // Check if a new line segment can be created.
    //     // if (CanCreateNewLine(tempTouchPos))
    //     // {
    //     //     // Call the DrawNewLine method with the new touch position.
    //     DrawNewLine(tempTouchPos);
    //     // }
    // }

    // private void HandleClickDown()
    // {
    //     // Instantiate a new line game object using the line prefab.
    //     _currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

    //     // Get the LineRenderer component of the current line game object.
    //     _lineRenderer = _currentLine.GetComponent<LineRenderer>();

    //     // Get the EdgeCollider2D component of the current line game object.
    //     //_edgeCollider = _currentLine.GetComponent<EdgeCollider2D>();

    //     // Add the current mouse position in world space to the touch positions list.
    //     _touchPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    //     _touchPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

    //     // Set the start and end positions of the LineRenderer component.
    //     _lineRenderer.SetPosition(0, _touchPositions[0]);
    //     _lineRenderer.SetPosition(1, _touchPositions[1]);

    //     // Set the points of the EdgeCollider2D component to the touch positions list.
    //     //_edgeCollider.points = _touchPositions.ToArray();
    // }

    // private void DrawNewLine(Vector2 newFingerPos)
    // {
    //     // add the new position of the finger to the list of touch positions
    //     _touchPositions.Add(newFingerPos);

    //     // increase the number of positions in the LineRenderer component
    //     _lineRenderer.positionCount++;

    //     // set the new position to the last position in the LineRenderer component
    //     _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, newFingerPos);

    //     // set the points in the EdgeCollider2D component to the touch positions
    //     //_edgeCollider.points = _touchPositions.ToArray();
    // }

    // private void HandleClickUp()
    // {
    //     // Clear the touch positions list
    //     _touchPositions.Clear();

        // Destroy the current line after 2 seconds
        //Destroy(_currentLine, 2f);
    //}

    // private bool CanCreateNewLine(Vector2 tempTouchPos)
    // {
    //     // Check if the distance between the current touch position and the last touch position in the list is greater than the sensitivity threshold
    //     return Vector2.Distance(tempTouchPos, _touchPositions[_touchPositions.Count - 1]) > SensitivityThreshold;
    // }
}