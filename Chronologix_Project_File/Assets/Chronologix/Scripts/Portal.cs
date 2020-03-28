using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int portalID;
    public int sceneID;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.lastPortalID = portalID;
        AnalyticTracker.instance.LevelExited(sceneID);
        GameManager.instance.LoadScene(sceneID);
    }
}
