using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    private Vector2 destination;
    private Vector2 direction;
    public float randomTime;
    public float time = 0;
    public float speed = 3.0f;
    public float rotSpeed = 1.0f;
    private Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        //destination = SetNewDestination();
        direction = SetNewDirection();
        randomTime = Random.Range(3.0f,10.0f);

    }

    // Update is called once per frame
    void Update()
    {

        if(time>=randomTime){
            direction = SetNewDirection();
            Debug.Log("Time is up, Changing direction");
            randomTime = Random.Range(3.0f,10.0f);
            time = 0;
        }
        rb2D.position += direction * Time.deltaTime * speed;
        time +=Time.deltaTime;
        transform.up = Vector3.Slerp(transform.up,direction,rotSpeed*Time.deltaTime); //direction;

        // if(rb2D.position==destination){
        //     //Debug.Log(gameObject.name + " Arrived at Destination");
        //     destination = SetNewDestination();

        // }
        // else{
        //     transform.position = Vector2.MoveTowards(transform.position,destination, speed*Time.deltaTime);
        //     transform.up = destination;
        // }

    }

    Vector2 SetNewDestination(){
        float randomx = Random.Range(-9.0f,9.0f);
        float randomy = Random.Range(-4.0f,4.0f);
        //Debug.Log(randomx);
        //Debug.Log(randomy);
        return new Vector2(randomx,randomy);

    }

    Vector2 SetNewDirection(){
        Vector2 randomRadius = Random.insideUnitCircle;
        // float randomx = Random.Range(-1.0f,1.0f);
        // float randomy = Random.Range(-1.0f,1.0f);
        // direction = new Vector2(randomx,randomy).normalized;
        return randomRadius;//direction;
    }



    void OnCollisionEnter2D(Collision2D other){
        direction = Vector3.Reflect(direction,other.GetContact(0).normal);//-direction;
        randomTime = Random.Range(3.0f,10.0f);
        //destination = SetNewDestination(); //should be the reflection of the angle?
        //Debug.Log(gameObject.name + " Collision - Changing Destination");
    }

}
