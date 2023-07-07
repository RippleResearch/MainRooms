using JetBrains.Annotations;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public MazeController maze;
    public GameObject SideMenu, dropdown;
    public List<GameObject> Buttons;
    Toggle randomNum;
    Slider resetSlider, speedSlider, wallsSlider, colorSlider;
    TMPro.TextMeshProUGUI resetText, speedText, wallsText, colorText;
    TMP_Dropdown colorPalletes;
    private List<string> currentPals;

    private bool leaveRandOn;

    private void OnEnable() {
        //Buttons
        Button Reset = GameObject.Find("RequestReset").GetComponent<Button>();
        Debug.Assert(Reset != null);
        Reset.onClick.AddListener(ResetMaze);

        //Get Sliders
        resetSlider = GameObject.Find("ResetTime").GetComponent<Slider>();
        speedSlider = GameObject.Find("Speed").GetComponent<Slider>();
        wallsSlider = GameObject.Find("WallsRemove").GetComponent<Slider>();
        colorSlider = GameObject.Find("Colors").GetComponent<Slider>();

        Debug.Assert(resetSlider != null);
        Debug.Assert(wallsSlider != null);
        Debug.Assert(speedSlider != null);
        Debug.Assert(colorSlider != null);

        //Get Text
        resetText = GameObject.Find("Reset Text").GetComponent<TMPro.TextMeshProUGUI>();
        speedText = GameObject.Find("Speed Text").GetComponent<TMPro.TextMeshProUGUI>();
        wallsText = GameObject.Find("Walls Text").GetComponent<TMPro.TextMeshProUGUI>();
        colorText = GameObject.Find("Colors Text").GetComponent<TMPro.TextMeshProUGUI>();

        Debug.Assert(resetText != null);
        Debug.Assert(speedText != null);
        Debug.Assert(wallsText != null);
        Debug.Assert(colorText != null);

        randomNum = GameObject.Find("RandomNumber").GetComponent<Toggle>();

        Debug.Assert(randomNum != null);
        
        colorPalletes = dropdown.GetComponent<TMP_Dropdown>();

        //Add buts to array so we can change color
        GameObject buttonParent = GameObject.Find("ColorButtons");
        for(int i = 0; i < buttonParent.transform.childCount; i++) {
            Buttons.Add(buttonParent.transform.GetChild(i).gameObject);
        }
        Debug.Assert(colorPalletes != null);

        //Defaults
        colorSlider.value = Random.Range(3, 13);
        randomNum.isOn = true;
        updatePalletes(updateButtons: true);
    }

    public void FixedUpdate() {
        resetText.text = "Reset After: " + resetSlider.value + " sec";
        speedText.text = "Speed: " + System.MathF.Round(speedSlider.value * 100f) + "%";
        wallsText.text = "Remove " + System.MathF.Round(wallsSlider.value * 100f) + "%";

        int colorSliderValue;
        if (randomNum.isOn || maze.palSet) {
            colorSliderValue = maze.usedColors.Value.Count;
        } else {
            colorSliderValue = (int) colorSlider.value;
        }
        colorText.text = "Colors: " + colorSliderValue;


        if (maze.updateColorDropDown || maze.resetRequested) {
            updatePalletes(updateButtons: true);
            maze.updateColorDropDown = false;
        }
    }

    void updatePalletes(bool updateButtons = false) {
        colorPalletes.ClearOptions();
        //colorPalletes.AddOptions(new List<string>{ maze.usedColors.Key });
        List<string> names = new();
        Dictionary<string, List<string>> currPal;
        if (!maze.colorBlindMode) {//If not color blind mode use the normal pallete to find colors of length
            currPal = ColorPalettes.GetColorMap()[maze.usedColors.Value.Count - 3];
        }
        else {//Color blind
            currPal = ColorPalettes.GetCBColorMap()[maze.usedColors.Value.Count - 3];
        }

        //Make sure correct name is selected on start
        foreach (string name in currPal.Keys) {
            if (name.Equals(maze.usedColors.Key)) {
                names.Insert(0, name);
            }
            else {
                names.Add(name);
            }
        }

        if (updateButtons || randomNum.isOn) {
            UpdateColorButtons(names[0]);
        }
        colorPalletes.AddOptions(currentPals = names); //Set field to use when it is changed
    }

    public void UpdateColorButtons(string palName) {
        List<Color> colors;
        if(!maze.colorBlindMode) {//Assume its selected so it is at 0 index
            colors = maze.colorController.HexListToColor(ColorPalettes.GetColorMap()[maze.usedColors.Value.Count - 3][palName]); 
        }
        else {//CB mode
            colors = maze.colorController.HexListToColor(ColorPalettes.GetCBColorMap()[maze.usedColors.Value.Count - 3][palName]);
        }
        
        for(int i = 0; i < colors.Count; i++) {
            //Enable button
            Buttons[i].SetActive(true);
            //Set Color
           ColorBlock cb = Buttons[i].GetComponent<Button>().colors;
            cb.normalColor = colors[i];
            cb.pressedColor = colors[i];
            cb.disabledColor = colors[i];
            cb.selectedColor = colors[i];

            Buttons[i].GetComponent<Button>().colors = cb;
        }
        for(int i = colors.Count; i < Buttons.Count; i++) {
            //disable button
            Buttons[i].SetActive(false);
        }

    }

    public void GetNextPal(int val) {
        KeyValuePair<string, List<string>> pal;
        if (!maze.colorBlindMode) {
            pal = new KeyValuePair<string, List<string>>(currentPals[val], ColorPalettes.GetColorMap()[maze.usedColors.Value.Count - 3][currentPals[val]]);
        }
        else {
            pal = new KeyValuePair<string, List<string>>(currentPals[val], ColorPalettes.GetCBColorMap()[maze.usedColors.Value.Count - 3][currentPals[val]]);
        }
        //Set Button Colors and whether or not they are enabled
        UpdateColorButtons(pal.Key);


        //Random color needs to be false since pallete selected
        if (randomNum.isOn) randomNum.isOn = false;

        SetNextPal(pal);
    }


    //Slider Methods
    public void ResetTimeChange(float value) {
        maze.ResetAfter = (int)value;
    }

    public void ChangeNumberOfColors(float value) {
        maze.numOfColors = (int)value;
        maze.palSet = false;
        randomNum.isOn = false;
        updatePalletes(updateButtons: true);
    }

    public void SpeedChange(float value) {
        maze.baseIncrement = value;
    }

    public void WallsRemoveChange(float value) {
        maze.wallsRemoved = value;
    }

    //Button Methods
    void ResetMaze() {
        maze.resetRequested = true;
    }


    public void SetColorBindMode(bool val) {
        maze.colorBlindMode = val;
        updatePalletes(updateButtons: true);
    }

    public void SetRandomNumOfColors(bool val) {
        maze.randomNumOfColors = val;
        maze.palSet = false;
    }

    public void SetNextPal(KeyValuePair<string, List<string>> pal) {
        maze.nextPal = maze.colorController.HexColorAndPair(pal);
        maze.palSet = true;
        

        UpdateColorButtons(pal.Key);
    }
}
