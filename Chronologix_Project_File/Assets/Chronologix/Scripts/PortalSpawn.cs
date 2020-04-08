using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawn : MonoBehaviour
{
    public GameObject player;
    public int portalID;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.lastPortalID == portalID)
        {
            player.transform.position = transform.position;
        }
    }
}
