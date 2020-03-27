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

    public Toggle optionsToggle;
    public Toggle soundToggle;
    public Toggle controlToggle;

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
        if (soundToggle.isOn)
        {
            soundMenuContent.SetActive(true);
            controlsMenuContent.SetActive(false);
        }
        else
        {
            soundMenuContent.SetActive(false);
            controlsMenuContent.SetActive(false);
        }
    }

    public void OpenControls()
    {
        if (controlToggle.isOn)
        {
            soundMenuContent.SetActive(false);
            controlsMenuContent.SetActive(true);
        } else
        {
            soundMenuContent.SetActive(false);
            controlsMenuContent.SetActive(false);
        }

    }

}
