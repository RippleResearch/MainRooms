using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Dropdown drop;
    // Start is called before the first frame update
    public void selectScene()
    {
       /* drop = this.gameObject.GetComponent<Dropdown>();
        int val = drop.value;
       *//* GameObject dropDownLabel = GameObject.FindWithTag("LoadButton")*//*; // Change button TAG in the button inspector and the string value for other labels
        Debug.Log(val);*/
        /*string dropDownValue = dropDownText.text;*/
        /*Debug.Log("1: " + dropDownValue);*/
        SceneManager.LoadScene("DrumPad");
    }
}
