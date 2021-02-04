using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioMixer audioMixer;
    public Dropdown dropdown;
    [SerializeField] private bool fullscreen;
    public static int pastScene;
    public static bool isInSettings;

    void Start()
    {
        isInSettings = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFullscreen()
    {
        if (fullscreen)
        {
            fullscreen = false;
        }
        else
        {
            fullscreen = true;
        }
    }

    public void SetRes()
    {
        int width, height;
        string res;
        switch(dropdown.value)
        {
            case 0:
                width = 1280;
                height = 760;
                res = "1280 x 760";
                break;
            case 1:
                width = 1920;
                height = 1080;
                res = "1920 x 1080";
                break;
            case 2:
                width = 2560;
                height = 1440;
                res = "2560 x 1440";
                break;
            default:
                width = 1280;
                height = 760;
                res = "1280 x 760";
                break;
        }
        res = "Changing resolution to: " + res;
        Debug.Log(res);
        Screen.SetResolution(width, height, fullscreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void ExitSettingsButton()
    {
        isInSettings = false;
        if (pastScene==1)
        {
            PauseMenu.stillPaused = true;
        }
        SceneManager.LoadScene(pastScene);
    }
}
