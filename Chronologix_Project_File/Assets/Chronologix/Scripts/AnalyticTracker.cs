using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class AnalyticTracker : MonoBehaviour@pp
{
    public static AnalyticTracker instance;
    public bool playerHasMoved;
    public bool playerHasJumped;
    public bool playerHasAttacked;
    public bool playerHasTimeStopped;
    public bool firstButtonPressed;
    public int interactAttemptCounter;
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    public void Update()
    {
        if (!firstButtonPressed)
        {
            if (Input.anyKeyDown)
            {
                FirstButtonPressed(Input.inputString);
                firstButtonPressed = true;
            }
        }
    }

    public void LevelExited(int newLevel)
    {
        Analytics.CustomEvent("FirstMoveAttempt", new Dictionary<string, object> {
            {"playerHealth", GameManager.instance.player.GetComponent<CombatHealth>().currentHealth },
            {"timeSinceStartup", Time.timeSinceLevelLoad },
            {"oldLevel", SceneManager.GetActiveScene().name },
            {"newLevel", newLevel }
        }) ;
    }

    public void FirstMove()
    {
        playerHasMoved = true;
        Analytics.CustomEvent("FirstMoveAttempt", new Dictionary<string, object> {
            {"timeSinceStartup", Time.realtimeSinceStartup }
        });
    }

    public void FirstJump()
    {
        playerHasJumped = true;
        Analytics.CustomEvent("FirstJumpAttempt", new Dictionary<string, object> {
            {"timeSinceStartup", Time.realtimeSinceStartup },
            {"level", SceneManager.GetActiveScene().name },
            {"location", GameManager.instance.player.transform.position }
        });
    }

    public void FirstAttack()
    {
        playerHasAttacked = true;
        Analytics.CustomEvent("FirstAttackAttempt", new Dictionary<string, object> {
            {"timeSinceStartup", Time.realtimeSinceStartup },
            {"level", SceneManager.GetActiveScene().name },
            {"location", GameManager.instance.player.transform.position }
        });
    }

    public void FirstTimeStop()
    {
        playerHasTimeStopped = true;
        Analytics.CustomEvent("FirstTimeStopAttempt", new Dictionary<string, object> {
            {"timeSinceStartup", Time.realtimeSinceStartup },
            {"level", SceneManager.GetActiveScene().name },
            {"location", GameManager.instance.player.transform.position }
        });
    }

    public void NPCInteract(string buttonPressed)
    {
        interactAttemptCounter++;
        Analytics.CustomEvent("PlayerNPCInteractAttempt", new Dictionary<string, object> {
            {"buttonPressed", buttonPressed },
            {"attemptNumber", interactAttemptCounter },
            {"level", SceneManager.GetActiveScene().name },
            {"location", GameManager.instance.player.transform.position }
        });
    }

    public void DamageTaken(string source)
    {
        Analytics.CustomEvent("PlayerTookDamage", new Dictionary<string, object> {
            {"timeSinceStartup", Time.realtimeSinceStartup },
            {"level", SceneManager.GetActiveScene().name },
            {"location", GameManager.instance.player.transform.position },
            {"source", source }
        });
    }

    public void FirstButtonPressed(string button)
    {
        Analytics.CustomEvent("ControlsMenuVisited", new Dictionary<string, object> {
            {"timeToButtonPress", Time.realtimeSinceStartup },
            {"buttonPressed", button }
        });
    }

    public void ExitingControlsMenu()
    {
        Analytics.CustomEvent("ControlsMenuVisited", new Dictionary<string, object> {
            {"totalTimeOnMenu", Time.timeSinceLevelLoad }
        });
    }

    public void PauseMenuOpened()
    {
        Analytics.CustomEvent("PauseMenuVisited", new Dictionary<string, object> {
            {"timeBeforeOpen", Time.realtimeSinceStartup }
        });
    }

    public void MapOpened()
    {
        Analytics.CustomEvent("MapVisited", new Dictionary<string, object> {
            {"timeBeforeOpen", Time.realtimeSinceStartup }
        });
    }
}
