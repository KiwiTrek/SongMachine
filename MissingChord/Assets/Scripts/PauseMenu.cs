using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    public static bool stillPaused = false;
    public static PauseMenu pauseMenuCanvas;
    public GameObject pauseMenu;
    // Start is called before the first frame update

    void Start()
    {
        if (pauseMenuCanvas == null)
        {
            pauseMenuCanvas = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(stillPaused)
        {
            PauseGame();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("escape");
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
        Debug.Log("resume");
        gamePaused = false;
        stillPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }
    
    public void PauseGame()
    {
        Debug.Log("pause");
        gamePaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void EnterSettingsButton()
    {
        pauseMenu.SetActive(false);
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
