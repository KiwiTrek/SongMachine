using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void EnterSettingsButton()
    {
        SettingsManager.pastScene = 0;
        SceneManager.LoadScene(2);
    }

    public void QuitGameButton()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
