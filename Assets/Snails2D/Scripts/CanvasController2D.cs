using DanielLochner.Assets.SimpleSideMenu;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public class CanvasController2D : MonoBehaviour {

    public GameObject drawLayer;
    private DrawPath drawScript;

    public GameObject SideMenu, eventSystem;
    public TMP_Dropdown sizeDropdown, colorDropdown, lineDropdown;
    public GameObject linePrefab, obstaclePrefab;
    //public List<GameObject> Buttons;
    //TMPro.TextMeshProUGUI resetText, speedText, wallsText, colorText, sizeText;
    //public GameObject littleSnail, mediumSnail, bigSnail;
    //public GameObject menu;

    private void OnEnable() {
        drawScript = drawLayer.GetComponent<DrawPath>();
   
    }

    public void Update() {
        

        if (Input.touchCount>0){
            //check if touch was on game object or ui
            //if on a snail
        }


    }

    // public void AddSnail(GameObject snailPrefab){
    //     Vector2 randomSpawnPoint = RandomPosition();

    //     int i = 0;
    //     while(Physics2D.OverlapCircle(randomSpawnPoint,2.56f) != null &&i<20){
    //         Debug.Log(Physics2D.OverlapCircle(randomSpawnPoint,2.56f) != null);
    //         randomSpawnPoint = RandomPosition();
    //         i++;
    //     }
    //     if(Physics2D.OverlapCircle(randomSpawnPoint,2.56f) == null){
    //         GameObject newSnail = Instantiate(snailPrefab, randomSpawnPoint, Quaternion.identity);
    //     }
    //     else{
    //         Debug.Log("No space could be found");
    //     }
    // }

    public Vector2 RandomPosition(){
        //check the aspect ratio to see where object can spawn
        float randomx = Random.Range(-9.0f,9.0f);
        float randomy = Random.Range(-4.0f,4.0f);
        Vector2 randomSpawnPoint = new Vector2(randomx,randomy);
        return randomSpawnPoint;
    }

    public void AddSnail(GameObject snailPrefab){//, Color32 snailColor){ //size and color chosen from dropdown in ui

        // TMP_Dropdown sd = sizeDropdown.GetComponent<TMP_Dropdown>();
        // TMP_Dropdown cd = colorDropdown.GetComponent<TMP_Dropdown>();

        //size dependent on the size dropdown
        //GameObject newSnail = null;
        // GameObject snailPrefab = null;
        // if(sd.value==0){
        //     //newSnail = Instantiate(bigSnail);
        //     snailPrefab = littleSnail;
        // }
        // else if (sd.value==1){
        //     //newSnail = Instantiate(mediumSnail);
        //     snailPrefab = mediumSnail;
        // }
        // else{
        //     //newSnail = Instantiate(littleSnail);
        //     snailPrefab = bigSnail;
        // }

        // float randomx = Random.Range(-9.0f,9.0f);
        // float randomy = Random.Range(-4.0f,4.0f);
        // Vector2 randomSpawnPoint = new Vector2(randomx,randomy);

        Vector2 randomSpawnPoint = RandomPosition();

        int i = 0;
        while(Physics2D.OverlapCircle(randomSpawnPoint,2.56f) != null&&i<30 ){
            Debug.Log(Physics2D.OverlapCircle(randomSpawnPoint,2.56f) != null);
            randomSpawnPoint = RandomPosition();
            i++;
        }
        if(Physics2D.OverlapCircle(randomSpawnPoint,2.56f) == null){
            GameObject newSnail = Instantiate(snailPrefab, randomSpawnPoint, Quaternion.identity);
            // newSnail.GetComponent<SpriteRenderer>().color = snailColor;
            newSnail.GetComponent<RandomMovement>().enabled = true;
        }
        else{
            Debug.Log("No space could be found");
        }


        // GameObject newSnail = Instantiate(snailPrefab, randomSpawnPoint, Quaternion.identity);
        //check to make sure there are no snails there already

        //color of snail dependent on the color dropdown
        //maybe add sprite instead
        // Color newSnailColor = new Color32(224,205,247,255); //purple
        // if(cd.value == 0){ //dark brown
        //     newSnailColor = new Color32(75,45,20,255);
        // }
        // else if (cd.value==1){ //light blue
        //     newSnailColor = new Color32(135,193,250,255);
        // }
        // else if (cd.value==2){ //peach
        //     newSnailColor = new Color32(255,199,168,255);
        // }
        // else if(cd.value==3){ //light brown
        //     newSnailColor = new Color32(226,195,168,255);
        // }
        // else if (cd.value==4){ //green
        //     newSnailColor = new Color32(123,237,154,255);
        // }
        // else if(cd.value==5){ //dark blue
        //     newSnailColor = new Color32(1,44,86,255);
        // } //(165, 199, 234, 1)
        // newSnail.GetComponent<SpriteRenderer>().color = newSnailColor;
        // newSnail.GetComponent<RandomMovement>().enabled = true;

    }

    public void ClearSnails(){ //Destroy all snails
        GameObject[] snails = GameObject.FindGameObjectsWithTag("Snail");
        foreach(GameObject snail in snails){
            //snail.transform.GetChild(0).parent = null;
            //Destroy(snail); 
            snail.GetComponent<SnailScript>().RemoveSnail();
        }
    }

    public void FreezeSnails(bool val){
        GameObject[] snails = GameObject.FindGameObjectsWithTag("Snail");
        foreach(GameObject snail in snails){
            if(val==true){
                snail.GetComponent<RandomMovement>().enabled = false;
                snail.GetComponent<SnailScript>().isMouseDown = true;
                Rigidbody2D rb2D= snail.GetComponent<Rigidbody2D>();
                rb2D.velocity = Vector2.zero;
                rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
                Debug.Log("Snails Frozen");
            }
            else{
                snail.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                snail.GetComponent<SnailScript>().isMouseDown = false;
                snail.GetComponent<RandomMovement>().enabled = true;
            }
        }
    }


    public void PathMode(bool val){
        GameObject[] snails = GameObject.FindGameObjectsWithTag("Snail");
        foreach(GameObject snail in snails){
            snail.GetComponent<SnailScript>().pathmode = val;
        }
        //if the mode is set to obstacle disable isTrigger = false
        if(val==false){
            drawScript.FinishLine();
        }
        FreezeSnails(val); //also make them not interactive until path mode is turned off
        if(lineDropdown.value==0){
            drawScript.IsLineTrigger(true); //also change the material so the difference is easier to see
        }
        else{
            drawScript.IsLineTrigger(false);
        }
        drawScript.enabled = val;

    }

    // public void PathType(){
    //     if(lineDropdown.value==0){
    //         drawScript.lineObject = linePrefab;
    //     }
    //     else{
    //         drawScript.lineObject = obstaclePrefab;
    //     }
    // }


    public void ClearPath(){
        GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
        foreach(GameObject path in paths){
            Destroy(path);
        }
    
    }

    public void CrazyMode(bool val){
        GameObject[] snails = GameObject.FindGameObjectsWithTag("Snail");
        if(val==true){
            foreach(GameObject snail in snails){
                var randomMovementScript = snail.GetComponent<RandomMovement>();
                randomMovementScript.enabled = true;
                randomMovementScript.speed = 100.0f;
            }
        }
        else{
            foreach(GameObject snail in snails){
            snail.GetComponent<RandomMovement>().speed = 1.0f;
            }
        }


    }



}
