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

    public void Update() {
        resetText.text = "Reset After: " + resetSlider.value + " sec";
        speedText.text = "Speed: " + System.MathF.Round(speedSlider.value * 100f) + "%";
        wallsText.text = "Remove " + System.MathF.Round(wallsSlider.value * 100f) + "%";
        colorText.text = "Colors: " + (randomNum.isOn ? maze.usedColors.Value.Count : System.MathF.Round(colorSlider.value));

        if (maze.resetRequested) {
            updatePalletes();
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
        colorPalletes.AddOptions(names);
    }

    void ResetMaze() {
       maze.SetRequestReset();
    }
}
