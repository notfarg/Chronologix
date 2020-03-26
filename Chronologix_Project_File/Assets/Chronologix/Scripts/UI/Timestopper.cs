using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timestopper : MonoBehaviour
{
    public Image timestopper;
    public TimeStopSpawner timeStopData;


    // Update is called once per frame
    void Update()
    {
        timestopper.fillAmount = timeStopData.spawnRechargeTimer / timeStopData.spawnRechargeTime;
    }
}
