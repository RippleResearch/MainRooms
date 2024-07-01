using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSpawn : MonoBehaviour
{

    public GameObject canvas;
    private DrawingCanvas canvasScript;
    //public GameObject starPrefab;
    public float spaceBetweenPoints =1;
    //public bool isMoving = false;
    //public float timeUntilDeactivaton=2;
    //public string drawMode;

    //private bool mouseDown = false;
    private Vector2 lastSpawnPoint; //too make sure there isn't too much overlap between particles

    //variable to pass when the object is spawned from the pool
    //timeUntilDeactivation
    //size //original size and deviation
    //amount or space
    //color
    //sprite

    // Start is called before the first frame update
    void Start(){
        canvasScript = canvas.GetComponent<DrawingCanvas>();

    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetMouseButton(0)){
        //     Debug.Log("Mouse is Down");
        //     mouseDown = true;
        // }
        // else{
        //     mouseDown = false;
        // }

    }

    void FixedUpdate(){

    }

    void OnMouseDrag(){
        if(!EventSystem.current.IsPointerOverGameObject()){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if(Vector2.Distance(lastSpawnPoint, mousePosition)>spaceBetweenPoints){


                // GameObject star = Instantiate(starPrefab);

                GameObject star = ObjectPool.SharedInstance.GetPooledObject();
                if(star!=null){
                    star.transform.position = mousePosition;
                    lastSpawnPoint = mousePosition;
                    star.transform.Rotate(0,0,Random.Range(0,360));
                    star.SetActive(true);
                    Rigidbody2D rb2D = star.GetComponent<Rigidbody2D>();
                    float randomScale = Random.Range(-.05f,.05f);
                    star.transform.localScale += new Vector3(randomScale,randomScale,0); //should set back when deactivating because these are going back into the pool

                    // if(isMoving){
                    //     rb2D.velocity = Random.insideUnitCircle;
                    // }

                    //GameObject.Destroy(star,2f);
                    if(canvasScript.drawMode.Equals("disappearing")){
                        StartCoroutine(WaitAndDeactivate(star));
                        rb2D.velocity = Random.insideUnitCircle;

                    }
                    else{ //draw mode is infinite
                    }


                }



            }
        }
    }

    void OnMouseUp(){
        lastSpawnPoint = Vector2.zero;
    }


    //Deactivates the gameObject after 2 seconds
    IEnumerator WaitAndDeactivate(GameObject star){
        float timeUntilDeactivaton = Random.Range(5,10);
        yield return new WaitForSeconds(timeUntilDeactivaton);
        star.SetActive(false);
    }

}
