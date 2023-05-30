using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class MainMenuScript : MonoBehaviour
{
    public AudioSource[] audios;
    public Button[] buttons;
    public Button button;
    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] RectTransform canvasRect;
    // Start is called before the first frame update
    private void Start()
    {
        audios = GetComponentsInChildren<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }
    private void Update()
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                m_PointerEventData = new PointerEventData(m_EventSystem);
                m_PointerEventData.position = Input.GetTouch(i).position;
                List<RaycastResult> results = new List<RaycastResult>();
                m_Raycaster.Raycast(m_PointerEventData, results);
                if (results.Count > 0)
                {
                    RaycastResult res = results[0];
                    for(var j =  0; j < results.Count; j++) 
                    {
                        if (results[j].gameObject.name.Contains("Button"))
                        {
                            res = results[j];
                        }
                    }
                    if(res.gameObject.GetComponent<BoxCollider>().isTrigger)
                    {
                        res.gameObject.GetComponent<AudioSource>().Play();
                    }
                }
            }

               
            /* RaycastHit hit;
             // Construct a ray from the current touch coordinates
             Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
             // Create a particle if hit
             Debug.Log("1");
             bool psy = Physics.Raycast(ray, out hit);
             Debug.Log(psy);
             if (Physics.Raycast(ray, out hit))
             {
                 Debug.Log("2");
                 if (hit.collider.isTrigger)
                 {
                     Debug.Log("3");
                     hit.collider.GetComponent<AudioSource>().Play();
                 }
             }*/
        }
    }
    public void playButton()
    {
        
    }
    
    public void click(int index)
    {
        
        /*collision.GetComponent<AudioSource>().Play();
        Debug.Log("Comp Name: " + collision.gameObject.name);*/
    }

    public void stopPlaying()
    {
        foreach(AudioSource audio in audios)
        {
            audio.Stop();
        }
    }

    private int findIndex(Button[] buttonArr, string name)
    {
        int index = 0;
        for (int i = 0; i < buttonArr.Length; i++)
        {
            if (buttonArr[i].name.Equals(name))
            {
                index = i; break;
            }
        }
        return index;
    }
}
