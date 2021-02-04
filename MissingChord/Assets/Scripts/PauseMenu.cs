using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private static bool gamePaused = false;
    [SerializeField] private GameObject pausedMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        gamePaused = false;
        pausedMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void PauseGame()
    {
        gamePaused = true;
        pausedMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void EnterSettingsButton()
    {
        SettingsManager.pastScene = 1;
        SceneManager.LoadScene(2);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
