using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrumkitHitter : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[i].position);
                if (Physics.Raycast(ray, out hit))
                {
                    ray.origin = Camera.main.transform.position;
                    // Check if the thing the ray is hitting is a trigger, and equals 
                    if (hit.collider.isTrigger)
                    {
                        try
                        {
                            if(hit.collider.gameObject.name != "Ship Loader")
                                hit.collider.gameObject.GetComponent<AudioSource>().Play();
                        }catch (System.Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                }
            }
        }
    }
}
/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumkitHitter : MonoBehaviour
{
    GameObject[] totalHitComponents;
    GameObject[] allHitComponents;
    int audioCount;
    // Start is called before the first frame update
    void Start()
    {
        totalHitComponents = GetComponentsInChildren<GameObject>();
        countAudio();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void countAudio()
    {
        for (int i = 0; i < totalHitComponents.Length; i++)
        {
            if (
        }
    }
}

 */
