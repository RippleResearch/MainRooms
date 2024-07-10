using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawPath : MonoBehaviour
{

    Coroutine drawing;
    public GameObject lineObject;
    public GameObject particles; //added in inspector from the assets folder
    public bool isLineTrigger = true;
    // private EdgeCollider2D edgeCollider;
    // private LineRenderer myLine;

    void Start(){
        // edgeCollider = lineObject.GetComponent<EdgeCollider2D>();
        // myLine = lineObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            
            if(!EventSystem.current.IsPointerOverGameObject()){
                StartLine();
            }
        }
        if(Input.GetMouseButtonUp(0)){
            FinishLine();
        }
        
        // if(Input.touchCount ==1){
            
        //     StartLine();
        //     if(Input.GetTouch(0).phase == TouchPhase.Ended){
        //         FinishLine();
        //     }
        //     // for(int i = 0;i<Input.touchCount;i++){
        //     //     Touch currentTouch = Input.GetTouch(i);
        //     //     StartLine();


        //     //     if (currentTouch.fingerId ==trackedFingerId &&currentTouch.phase = TouchPhase.Ended){
        //     //         FinishLine();
        //     //     }
        //     // }


        // }

// if(Input.touchCount > 0)
// {
// 	for(int i = 0; i < Input.touchCount; i++)
// 	{
// 		Touch currentTouch = Input.GetTouch(i);
// 		if(currentTouch.fingerId == trackedFingerId && currentTouch.phase == TouchPhase.Ended)
// 		{
// 			// Do something
// 		}
// 	}
// }

    }

    void StartLine(){
        if(drawing!=null){
            StopCoroutine(drawing);
        }
        drawing = StartCoroutine(DrawLine());
    }

    //changed from private
    public void FinishLine(){
        StopCoroutine(drawing);
    }

    IEnumerator DrawLine(){

        // //using a particle system
        // GameObject newGameObject = Instantiate(particles, new Vector3(0,0,0), Quaternion.identity);
        // ParticleSystem line = newGameObject.GetComponent<ParticleSystem>();
        // //line.positionCount = 0;

        // while(true){
        //     Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     mousePosition.z = 0;
        //     //line.positionCount++;
        //     //line.SetPosition(line.positionCount-1,position);
        //     //line.Particle.position = mousePosition;
        //     line.transform.position = mousePosition;
        //     yield return null;
        // }
    
    //  using a lineRenderer
        GameObject newGameObject = Instantiate(lineObject, new Vector3(0,0,0), Quaternion.identity);
        LineRenderer line = newGameObject.GetComponent<LineRenderer>();
        line.positionCount = 0;
        EdgeCollider2D edgeCollider = newGameObject.GetComponent<EdgeCollider2D>();
        edgeCollider.isTrigger = isLineTrigger;

        List<Vector2> points = new List<Vector2>();

        while(true){
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            line.positionCount++;
            points.Add(position);
            line.SetPosition(line.positionCount-1,position);

            edgeCollider.points = points.ToArray();
            yield return null;
        }

    }

    public void IsLineTrigger(bool val){
        isLineTrigger = val;
    }
    // public void SelectLine(GameObject newParticleSystem){
    //     GameObject newGameObject = Instantiate(newParticleSystem, new Vector3(0,0,0), Quaternion.identity);
    //     ParticleSystem line = newGameObject.GetComponent<ParticleSystem>();
    //     Debug.Log("Line Selected");

    // }

    // void SetEdgeCollider(LineRenderer lineRenderer){
    //     List<Vector2> edges = new List<Vector2>();

    //     for(int point = 0; point<line)
    // }
}
