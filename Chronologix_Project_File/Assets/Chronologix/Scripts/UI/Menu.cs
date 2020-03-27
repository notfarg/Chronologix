using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{

    public GameObject soundMenu;
    public GameObject controlsMenu;

    public GameObject soundMenuContent;
    public GameObject controlsMenuContent;

    public GameObject splashScreen;

    public Toggle optionsToggle;

    private void Awake()
    {
        soundMenu.SetActive(false);
        controlsMenu.SetActive(false);

        soundMenuContent.SetActive(false);
        controlsMenuContent.SetActive(false);
    }

    public void ChangeSceene (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
    }

    public void OptionsMenu()
    {
        if (optionsToggle.isOn)
        {
            soundMenu.SetActive(true);
            controlsMenu.SetActive(true);
        }
        else
        {
            soundMenu.SetActive(false);
            controlsMenu.SetActive(false);
        }
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
