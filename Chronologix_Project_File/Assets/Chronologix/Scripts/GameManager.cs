using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public float playerHealth;
    public float timeStopEnergy;
    public int sceneFrom;
    public int lastPortalID;
    public bool nearNPC;
    public int numActiveSwitches = 0;
    public int score = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        SceneManager.sceneLoaded += OnLevelLoad;
    }

    private void OnLevelLoad(Scene level, LoadSceneMode loadMode)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.GetComponent<CombatHealth>().currentHealth = playerHealth;
                player.GetComponent<TimeStopSpawner>().spawnRechargeTimer = timeStopEnergy;
            }
        }
    }

    public void LoadScene(int sceneToLoad)
    {
        sceneFrom = SceneManager.GetActiveScene().buildIndex;
        if (player != null)
        {
            if (sceneToLoad != 0)
            {
                playerHealth = player.GetComponent<CombatHealth>().currentHealth;
                timeStopEnergy = player.GetComponent<TimeStopSpawner>().spawnRechargeTimer;
            } else
            {
                GameManager.instance.lastPortalID = 0;
                playerHealth = 100;
                timeStopEnergy = 20;
            }
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
