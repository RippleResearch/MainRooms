using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public MazeController maze;
    public GameObject SideMenu, dropdown;
    Toggle randomNum;
    Slider resetSlider, speedSlider, wallsSlider, colorSlider;
    TMPro.TextMeshProUGUI resetText, speedText, wallsText, colorText;
    TMP_Dropdown colorPalletes;
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
        
        Debug.Assert(colorPalletes != null);
        updatePalletes();
    }

    public void FixedUpdate() {
        resetText.text = "Reset After: " + resetSlider.value + " sec";
        speedText.text = "Speed: " + System.MathF.Round(speedSlider.value * 100f) + "%";
        wallsText.text = "Remove " + System.MathF.Round(wallsSlider.value * 100f) + "%";
        colorText.text = "Colors: " + (randomNum.isOn ? maze.usedColors.Value.Count : System.MathF.Round(colorSlider.value));

        if (maze.updateColorDropDown || maze.resetRequested) {
            updatePalletes();
            maze.updateColorDropDown = false;
        }
    }

    void updatePalletes() {
        colorPalletes.ClearOptions();
        //colorPalletes.AddOptions(new List<string>{ maze.usedColors.Key });
        List<string> names = new();
        if (!maze.colorBlindMode) {//If not color blind mode use the normal pallete to find colors of length
            foreach(string name in ColorPalettes.GetColorMap()[maze.usedColors.Value.Count - 3].Keys) {
                names.Add(name);
            }
        }
        else {//Color blind
            foreach (string name in ColorPalettes.GetCBColorMap()[maze.usedColors.Value.Count - 3].Keys) {
                names.Add(name);
            }
        }
        colorPalletes.AddOptions(currentPals = names); //Set field to use when it is changed
    }

    void ResetMaze() {
       SetRequestReset();
    }

    public void GetNextPal(int val) {
        KeyValuePair<string, List<string>> pal;
        if (!maze.colorBlindMode) {
            pal = new KeyValuePair<string, List<string>> (currentPals[val], ColorPalettes.GetColorMap()[maze.usedColors.Value.Count - 3][currentPals[val]]);
        }
        else {
            pal = new KeyValuePair<string, List<string>>(currentPals[val], ColorPalettes.GetCBColorMap()[maze.usedColors.Value.Count - 3][currentPals[val]]);
        }

        //Random color needs to be false since pallete selected
        if(randomNum.isOn) randomNum.isOn = false;

        SetNextPal(pal);
    }


    //Slider Methods
    public void ResetTimeChange(float value) {
        maze.ResetAfter = (int)value;
    }

    public void ChangeNumberOfColors(float value) {
        maze.numOfColors = (int)value;
        maze.palSet = false;
    }

    public void SpeedChange(float value) {
        maze.baseIncrement = value;
    }

    public void WallsRemoveChange(float value) {
        maze.wallsRemoved = value;
    }

    //Button Methods
    public void SetRequestReset() {
        maze.resetRequested = true;
    }

    public void SetColorBindMode(bool val) {
        maze.colorBlindMode = val;
    }

    public void SetRandomNumOfColors(bool val) {
        maze.randomNumOfColors = val;
        maze.palSet = false;
    }

    public void SetNextPal(KeyValuePair<string, List<string>> pal) {
        maze.nextPal = maze.colorController.HexColorAndPair(pal);
        maze.palSet = true;
    }
}
