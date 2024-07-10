using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDrawer : MonoBehaviour
{
    private Texture2D Image;
    public int res = 1024;

    void Start(){
        this.Image = new Texture2D(res,res,TextureFormat.RGBA32,false);
        GetComponent<Renderer>().material.SetTexture("_BaseMap",this.Image);
    }

    void Update(){
        


        if(Input.GetMouseButton(0)){
            Collider2D collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition),(1<<0));
            Debug.Log(collider.name);
            if(collider.name.Equals("SpriteMaker")){

                
                Debug.Log("Clicked on sqaure" );
            }



            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
            Debug.Log(hit.collider.name + "the raycast worked");

            // If it hits something...
            if (hit.collider.name.Equals("SpriteMaker")){
                
            }

        }


    }

    public void UpdateTexture(){
        
        //raycast pixels
        //set pixels


        //apply the texture
        this.Image.Apply();
    }
    

}
