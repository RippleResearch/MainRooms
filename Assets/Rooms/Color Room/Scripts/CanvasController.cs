using JetBrains.Annotations;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    public MazeController maze;
    public GameObject SideMenu, dropdown;
    public List<GameObject> Buttons;
    Toggle randomPal, randomSize, randomNumOfColors, randomRules;
    Slider resetSlider, speedSlider, wallsSlider, colorSlider, sizeSlider;
    TMPro.TextMeshProUGUI resetText, speedText, wallsText, colorText, sizeText;
    TMP_Dropdown colorPalletes, rulesDropdown;
    private List<string> currentPals;

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
        sizeSlider = GameObject.Find("SizeMultiplier").GetComponent<Slider>();

        Debug.Assert(resetSlider != null);
        Debug.Assert(wallsSlider != null);
        Debug.Assert(speedSlider != null);
        Debug.Assert(colorSlider != null);
        Debug.Assert(sizeSlider != null);

        //Get Text
        resetText = GameObject.Find("Reset Text").GetComponent<TMPro.TextMeshProUGUI>();
        speedText = GameObject.Find("Speed Text").GetComponent<TMPro.TextMeshProUGUI>();
        wallsText = GameObject.Find("Walls Text").GetComponent<TMPro.TextMeshProUGUI>();
        colorText = GameObject.Find("Colors Text").GetComponent<TMPro.TextMeshProUGUI>();
        sizeText = GameObject.Find("Size Text").GetComponent<TMPro.TextMeshProUGUI>();

        Debug.Assert(resetText != null);
        Debug.Assert(speedText != null);
        Debug.Assert(wallsText != null);
        Debug.Assert(colorText != null);
        Debug.Assert(sizeText != null);

        //Get Toggles
        randomPal = GameObject.Find("RandomPal").GetComponent<Toggle>();
        randomSize = GameObject.Find("RandomSize").GetComponent<Toggle>();
        randomNumOfColors = GameObject.Find("RandomNum").GetComponent<Toggle>();
        randomRules = GameObject.Find("RandomRules").GetComponent<Toggle>();

        Debug.Assert(randomPal != null);
        Debug.Assert(randomSize != null);
        Debug.Assert(randomNumOfColors != null);
        Debug.Assert(randomRules != null);

        //Get Dropdowns
        colorPalletes = dropdown.GetComponent<TMP_Dropdown>();
        rulesDropdown = GameObject.Find("RulesDropDown").GetComponent<TMP_Dropdown>();

        //Add buttons to array so we can change color
        GameObject buttonParent = GameObject.Find("ColorButtons");
        for (int i = 0; i < buttonParent.transform.childCount; i++) {
            Buttons.Add(buttonParent.transform.GetChild(i).gameObject);
        }

        Debug.Assert(colorPalletes != null);
        Debug.Assert(rulesDropdown != null);

        //Defaults
        colorSlider.value = Random.Range(3, 13);
        randomPal.isOn = true;
        UpdateDropDownPals();
        BuildRules();
    }

    public void FixedUpdate() {
        resetText.text = "Reset After: " + resetSlider.value + " sec";
        speedText.text = "Speed: " + System.MathF.Round(speedSlider.value * 100f) + "%";
        wallsText.text = "Remove: " + System.MathF.Round(wallsSlider.value * 100f) + "%";
        sizeText.text = "Size Multiplier: " + (randomSize.isOn ? maze.sizeMultiplier :  sizeSlider.value);

        int colorSliderValue;
        if (randomNumOfColors.isOn || maze.palSet) {
            colorSliderValue = maze.usedColors.Value.Count;
            colorSlider.value = maze.usedColors.Value.Count;
        }
        else {
            colorSliderValue = (int)colorSlider.value;
        }
        colorText.text = "Colors: " + colorSliderValue;

        //Change drop downs and buttons
        if (!maze.palSet && (maze.updateColorDropDown || maze.resetRequested)) {
            UpdateDropDownPals();
            maze.updateColorDropDown = false;
        }
        //if we have random rules and we need a reset update the drop down
        if (randomRules.isOn && (maze.updateColorDropDown || maze.resetRequested)) { 
            BuildRules();
        }


    }

    void BuildRules() {
        rulesDropdown.options.Clear();
        //Make sure correct name is selected on start
        List<string> names = new List<string>();
        foreach (string name in maze.ruleMethodName.Keys) {
            if (name.Equals(maze.methodName)) {
                names.Insert(0, name);
            }
            else {
                names.Add(name);
            }
        }
        rulesDropdown.AddOptions(names);

        Debug.Log("Updating dropdown new first index should be: " + names[0]);
    }
    
    public void SetRules(int val) {
        randomRules.isOn = false;

        maze.rulesSet = true; //Rules have been set for next reset
        maze.methodName = rulesDropdown.options[val].text; //Set method name for method to call
    }

    public void RandomRules(bool val) {
        if (!val) {
            maze.methodName = rulesDropdown.options[rulesDropdown.value].text;
            maze.rulesSet = true; //Rules have been set for next reset
        }
        else {
            maze.rulesSet = false;
        }
    }

    void UpdateDropDownPals() {
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
        colorPalletes.AddOptions(currentPals = names); //Set field to use when it is changed
        UpdateButtonColors(names[0]);
    }

    public void UpdateButtonColors(string palName) {
        List<Color> colors;
        if (!maze.colorBlindMode) {//Assume its selected so it is at 0 index
            colors = maze.colorController.HexListToColor(ColorPalettes.GetColorMap()[maze.usedColors.Value.Count - 3][palName]);
        }
        else {//CB mode
            colors = maze.colorController.HexListToColor(ColorPalettes.GetCBColorMap()[maze.usedColors.Value.Count - 3][palName]);
        }

        for (int i = 0; i < colors.Count; i++) {
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
        for (int i = colors.Count; i < Buttons.Count; i++) {
            //disable button
            Buttons[i].SetActive(false);
        }

    }

    public void SeletDropDownPal(int val) {
        KeyValuePair<string, List<string>> pal = GetPalFromIndex(val);
        //Set Button Colors and whether or not they are enabled
        UpdateButtonColors(pal.Key);


        //Random color needs to be false since pallete selected
        if (randomPal.isOn) randomPal.isOn = false;

        SetNextPal(pal);
    }

    private KeyValuePair<string, List<string>> GetPalFromIndex(int val) {
        KeyValuePair<string, List<string>> pal;
        if (!maze.colorBlindMode) {
            pal = new KeyValuePair<string, List<string>>(currentPals[val], ColorPalettes.GetColorMap()[maze.usedColors.Value.Count - 3][currentPals[val]]);
        }
        else {
            pal = new KeyValuePair<string, List<string>>(currentPals[val], ColorPalettes.GetCBColorMap()[maze.usedColors.Value.Count - 3][currentPals[val]]);
        }

        return pal;
    }


    //Slider Methods
    public void ResetTimeChange(float value) {
        maze.ResetAfter = (int)value;
    }

    public void ChangeNumberOfColors(float value) {

        maze.numOfColors = (int)value;
        maze.palSet = false;
        //randomPal.isOn = false;
        UpdateDropDownPals();
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
        UpdateDropDownPals();
    }

    public void SetRandomNumOfColors(bool val) {
        maze.randomNumOfColors = val;
    }

    public void SetRandomPal(bool val) {
        if (val == false) { //if we turn off the button keep the pal we are on
            maze.palSet = true;
            maze.nextPal = maze.usedColors;
            randomNumOfColors.isOn = false;
        }
        else {
            maze.palSet = false; //else give me random pals
        }
    }

    public void SetNextPal(KeyValuePair<string, List<string>> pal) {
        maze.nextPal = maze.colorController.HexColorAndPair(pal);
        maze.palSet = true;
        UpdateButtonColors(pal.Key);
    }

    public void SetSizeMultiplier(float val) {
        maze.sizeMultiplier = (int)val;
        randomSize.isOn = false;
    }

    public void SetRandomSize(bool val) {
        maze.randomSize = val;
    }
}
