using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public MazeController maze;
    public GameObject SideMenu;
    public GameObject ResetTime, Speed, WallsRemove, Reset, ColorBlind;

    public Slider resetSlider, speedSlider, wallsSlider;
    public TMPro.TextMeshProUGUI resetText, speedText, wallsText;

    private bool colorMode = false;
    private void OnEnable() {
        //Sliders
        GameObject ResetTime = GameObject.Find("ResetTime");
        GameObject Speed = GameObject.Find("Speed");
        GameObject WallsRemove = GameObject.Find("WallsRemove");


        Debug.Assert(ResetTime != null);
        Debug.Assert(Speed != null);
        Debug.Assert(WallsRemove != null);

        //Buttons
        Button Reset = GameObject.Find("RequestReset").GetComponent<Button>();
        Button ColorBlind = GameObject.Find("ColorBlind").GetComponent<Button>().GetComponent<Button>();
        Debug.Assert(Reset != null);
        Debug.Assert(ColorBlind != null);

        Reset.onClick.AddListener(ResetMaze);
        ColorBlind.onClick.AddListener(ColorMode);

        //Get Sliders
        resetSlider = ResetTime.GetComponent<Slider>();
        speedSlider = Speed.GetComponent<Slider>();
        wallsSlider = WallsRemove.GetComponent<Slider>();

        Debug.Assert(resetSlider != null);
        Debug.Assert(wallsSlider != null);
        Debug.Assert(speedSlider != null);

        //Get Text
        resetText = GameObject.Find("Reset Text").GetComponent<TMPro.TextMeshProUGUI>();
        speedText = GameObject.Find("Speed Text").GetComponent<TMPro.TextMeshProUGUI>();
        wallsText = WallsRemove.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        Debug.Assert(resetText != null);
        Debug.Assert(speedText != null);
        Debug.Assert(wallsText != null);
    }

    public void Update() {
        resetText.text = "Reset Time: " + resetSlider.value + " sec";
        speedText.text = "Speed: " + System.MathF.Round(speedSlider.value * 100f) + "%";
        wallsText.text = "Remove " + System.MathF.Round(wallsSlider.value * 100f) + "%";
    }

    void ResetMaze() {
       maze.SetRequestReset();
    }

    void ColorMode() {
        if (colorMode == false) {
            maze.SetColorBindMode(true);
            colorMode = true;
        }
        else {
            maze.SetColorBindMode(false);
            colorMode = false;
        }
    }

}
