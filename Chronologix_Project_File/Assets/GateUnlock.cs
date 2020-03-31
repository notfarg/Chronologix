using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateUnlock : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.instance.numActiveSwitches >= 2)
        {
            Destroy(this.gameObject);
        }
    }
}
