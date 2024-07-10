using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        Texture2D texture = new Texture2D(128,128);
        Sprite sprite = Sprite.Create(texture,new Rect(0,0,128,128),Vector2.zero);
        GetComponent<SpriteRenderer>().sprite = sprite;

        //do stuff

        texture.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
