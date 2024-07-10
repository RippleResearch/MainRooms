using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    private Vector2 mousePosition;
    public float moveSpeed = 1f;
    public float rotSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(Vector2.Distance(mousePosition,transform.position)<5){
                transform.position = Vector2.Lerp(transform.position,mousePosition,moveSpeed*.001f);

                // transform.position = Vector2.MoveTowards(transform.position,mousePosition, moveSpeed*Time.deltaTime);
                //Quaternion lookatWp = Quaternion.LookRotation(mousePosition - this.GetComponent<Rigidbody2D>().position);


                Vector2 direction = new Vector2(mousePosition.x -transform.position.x, mousePosition.y - transform.position.y);
                transform.up = direction;
                //transform.up = Vector3.Slerp(transform.up,direction,rotSpeed*Time.deltaTime); //direction;


            // this.transform.rotation = Quaternion.Slerp(transform.rotation, lookatWp, rotSpeed * Time.deltaTime);
            }


        }
    }
}
