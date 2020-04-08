using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopSpawner : MonoBehaviour
{
    public GameObject timeStopperPrefab;
    public float spawnRechargeTimer;
    public float spawnRechargeTime;
    public bool canSpawn;

    private void Update()
    {
        if (spawnRechargeTimer < spawnRechargeTime)
        {
            spawnRechargeTimer += Time.deltaTime;
        }

        if (spawnRechargeTimer >= spawnRechargeTime)
        {
            canSpawn = true;
        }
    }
    public void SpawnTimeStopper()
    {
        if (canSpawn)
        {
            if (!AnalyticTracker.instance.playerHasTimeStopped)
            {
                AnalyticTracker.instance.FirstTimeStop();
            }
            Instantiate(timeStopperPrefab, transform.position, Quaternion.identity);
            spawnRechargeTimer = 0;
            canSpawn = false;
        }
        if (GameManager.instance.nearNPC)
        {
            AnalyticTracker.instance.NPCInteract("timestop");
        }
    }
}
