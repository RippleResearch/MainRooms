using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public MazeController maze;
    public GameObject SideMenu;

    public Slider resetSlider, speedSlider, wallsSlider, colorSlider;
    public TMPro.TextMeshProUGUI resetText, speedText, wallsText, colorText;

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
    }

    public void Update() {
        resetText.text = "Reset After: " + resetSlider.value + " sec";
        speedText.text = "Speed: " + System.MathF.Round(speedSlider.value * 100f) + "%";
        wallsText.text = "Remove " + System.MathF.Round(wallsSlider.value * 100f) + "%";
        colorText.text = "Colors: " + System.MathF.Round(colorSlider.value);
    }

    void ResetMaze() {
       maze.SetRequestReset();
    }
}
