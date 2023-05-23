using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    private Material material;
    private Color previousColor;
    private bool isPressed;
    [SerializeField] public Queue<Vector3> pressesQueue;

    // Start is called before the first frame update
    void Start()
    {
        pressesQueue = new Queue<Vector3>();
        isPressed = false;
/*        Debug.Log("isPressed: " + isPressed);*/
        material = GetComponent<MeshRenderer>().sharedMaterial;
        previousColor = material.GetColor("_BaseColor");
        material.SetColor("_RippleColor", previousColor);
    }

    // Update is called once per frame
    void Update()
    {
 /*       Debug.Log("isPressed: " + isPressed);*/
        if (Input.GetMouseButtonDown(0))
        {
            
            if (!isPressed)
            {
                /*Debug.Log("1");*/
                isPressed = true;
                CastClickRay();
                StartCoroutine(rippleHappening());
            }
            else
            {
                /*Debug.Log("2");*/
                var cam = Camera.main;
                var mousePos = Input.mousePosition;
                var ray = cam.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
                Physics.Raycast(ray, out var hitInfo);
                pressesQueue.Enqueue(hitInfo.point);
            }
        }
        else if (pressesQueue.Count > 0 && !isPressed)
        {
            isPressed = true;
            CastClickRay();
            StartCoroutine(rippleHappening());
        }

    }

    private void CastClickRay()
    {
        var cam = Camera.main;

        var mousePos = Input.mousePosition;

        var ray = cam.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

        if(Physics.Raycast(ray, out var hitInfo) && hitInfo.collider.gameObject == gameObject) {
            /*Debug.Log("3");*/
            if (pressesQueue.Count > 0)
            {
                //Debug.Log("4");
                startRipple(pressesQueue.Dequeue());
            }
            else{
                //Debug.Log("5");
                startRipple(hitInfo.point);
            }
        }

        /*if(Input.touchCount > 0 && hitInfo.collider.gameObject == gameObject)
        {
           for(int i = 0; i < Input.touchCount; i++)
            {
                startRipple(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, cam.nearClipPlane));
            }
        }*/
    }

    private void startRipple(Vector3 center)
    {

        //Random Color
        Color rippleColor = Color.HSVToRGB(Random.value, 1, 1);

        material.SetVector("_RippleCenter", center);

        material.SetFloat("_RippleStartTime", Time.time);
        material.SetColor("_BaseColor", previousColor);
        material.SetColor("_RippleColor", rippleColor);


        previousColor = rippleColor;
        
    }

    IEnumerator rippleHappening()
    {
/*        print(Time.time);*/
        yield return new WaitForSeconds(2);
        isPressed = false;
    /*    print(Time.time);*/
    }
}
