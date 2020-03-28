using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretCollider : MonoBehaviour
{
    public bool secretFound;
    public int secretID;
    private void OnTriggerEnter(Collider other)
    {
        if (!secretFound)
        {
            secretFound = true;
            AnalyticTracker.instance.FoundSecret(secretID);
        }
    }
}
