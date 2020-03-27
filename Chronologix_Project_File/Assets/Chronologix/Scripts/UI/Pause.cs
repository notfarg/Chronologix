using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseWindow;
    public GameObject optionsWindow;
    public GameObject confirmQuitWindow;
    public GameObject confirmMenuWindow;

    public GameObject soundMenuContent;
    public GameObject controlsMenuContent;

    public void Awake()
    {
        optionsWindow.SetActive(false);
        confirmMenuWindow.SetActive(false);
        confirmQuitWindow.SetActive(false);
    }

    public void PauseWindow()
    {
        pauseWindow.SetActive(true);
        optionsWindow.SetActive(false);
        confirmMenuWindow.SetActive(false);
        confirmQuitWindow.SetActive(false);
    }

    public void Resume()
    {
        pauseWindow.SetActive(false);
        optionsWindow.SetActive(false);
        confirmMenuWindow.SetActive(false);
        confirmQuitWindow.SetActive(false);
    }

    public void Options()
    {
        optionsWindow.SetActive(true);
        confirmMenuWindow.SetActive(false);
        confirmQuitWindow.SetActive(false);

        soundMenuContent.SetActive(true);
        controlsMenuContent.SetActive(false);
    }

    public void MainMenu()
    {
        confirmMenuWindow.SetActive(true);
        confirmQuitWindow.SetActive(false);
        optionsWindow.SetActive(false);
    }

    public void Quit()
    {
        confirmQuitWindow.SetActive(true);
        optionsWindow.SetActive(false);
        confirmMenuWindow.SetActive(false);
    }

    public void BackToMainMenu()
    {
        Debug.Log("Go back to main menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
    }

    public void OpenSound()
    {
        soundMenuContent.SetActive(true);
        controlsMenuContent.SetActive(false);

    }

    public void OpenControls()
    {
        controlsMenuContent.SetActive(true);
        soundMenuContent.SetActive(false);

    }
}
