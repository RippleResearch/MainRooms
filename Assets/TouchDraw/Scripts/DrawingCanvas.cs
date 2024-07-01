using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingCanvas : MonoBehaviour
{
    //public ToggleGroup brushToggleGroup;
    //public GameObject[] brushes;
    public GameObject currentBrush;

    public GameObject drawLayer;
    private SpriteRenderer canvasBackground;
    private TouchDraw drawScript;
    private LineDrawer lineScript; 
    private ObjectSpawn objectSpawnScript;

    public string drawMode = "disappearing";
    public bool isErasing = false; // Add the isErasing property

    // Start is called before the first frame update
    void Start()
    {
        canvasBackground = drawLayer.GetComponent<SpriteRenderer>();
        drawScript = drawLayer.GetComponent<TouchDraw>();
        lineScript = drawLayer.GetComponent<LineDrawer>();
        objectSpawnScript = drawLayer.GetComponent<ObjectSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.touchCount>0){
        //     lineScript.DrawNewLine(); //
        // }
        // if(currentBrush!=null){
        //     drawScript.particles = currentBrush;
        //     lineScript.currentBrush = currentBrush;
        // }
    }

    // Sets the current brush used for drawing
    public void SetCurrentBrush(GameObject newBrush)
    {
        //currentBrush = newBrush;
        ClearCanvas();
        drawScript.particles = newBrush;
        lineScript.currentBrush = newBrush;
    }

    // Cycles through color options of the background layer
    public void ChangeCanvasBackground()
    {
        if (canvasBackground.color == new Color32(60, 60, 60, 255))
        {
            canvasBackground.color = Color.white;
        }
        else if (canvasBackground.color == Color.white)
        {
            canvasBackground.color = Color.grey;
        }
        else if (canvasBackground.color == Color.grey)
        {
            canvasBackground.color = new Color32(44, 42, 92, 255);
        }
        else if (canvasBackground.color == new Color32(44, 42, 92, 255))
        {
            canvasBackground.color = Color.black;
        }
        else
        {
            canvasBackground.color = new Color32(60, 60, 60, 255);
        }
    }

    public void ChangeDrawMode()
    {
        ParticleSystem.MainModule psmain = drawScript.particles.GetComponent<ParticleSystem>().main;
        if (drawMode.Equals("disappearing"))
        {
            drawMode = "infinite";
            psmain.startLifetime = 10000f;
            Debug.Log("Draw mode: infinite");
        }
        else
        {
            drawMode = "disappearing";
            psmain.startLifetime = 5.0f;
            Debug.Log("Draw mode: disappearing");
        }
    }

    public void ChangeMode()
    {
        ClearStars();
        if (drawMode.Equals("disappearing"))
        {
            drawMode = "infinite";
            //stop coroutine of deactivation for particles
        }
        else
        {
            drawMode = "disappearing";
        }
    }

    public void ClearStars()
    {
        GameObject[] stars = GameObject.FindGameObjectsWithTag("Stamp");
        foreach (GameObject star in stars)
        {
            if (star.activeInHierarchy)
            {
                star.SetActive(false);
            }
        }
    }

    public void ClearCanvas()
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("Brush");
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
    }
}
