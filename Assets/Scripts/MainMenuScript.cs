using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public AudioSource[] audios;
    public Button[] buttons;
    public Button button;
    // Start is called before the first frame update
    private void Start()
    {
        audios = GetComponentsInChildren<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
    }
    private void Update()
    {

    }
    public void playButton()
    {
        /*Debug.Log("Name: " + EventSystem.current.currentSelectedGameObject.name);*/
        int index = findIndex(buttons, EventSystem.current.currentSelectedGameObject.name);
        /*Debug.Log("Audio " + index + ": " + audios[index]);*/
        audios[index].Play();
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
