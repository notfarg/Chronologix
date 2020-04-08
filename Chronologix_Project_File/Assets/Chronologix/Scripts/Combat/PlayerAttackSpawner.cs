using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSpawner : MonoBehaviour
{
    public GameObject attackPrefab;
    public bool facingLeft;
    GameObject currentAttack;
    public float damageVal;
    public float knockbackSpeedVal;
    public void SpawnAttack()
    {
        if (currentAttack == null)
        {
            if (facingLeft)
            {
                currentAttack = Instantiate(attackPrefab, transform.position + Vector3.left + Vector3.up, Quaternion.identity, this.transform);
            }
            else
            {
                currentAttack = Instantiate(attackPrefab, transform.position + Vector3.right + Vector3.up, Quaternion.identity, this.transform);
            }

            currentAttack.GetComponent<PlayerAttack>().damageVal = damageVal;
            currentAttack.GetComponent<PlayerAttack>().knockbackSpeedVal = knockbackSpeedVal;

            if (!AnalyticTracker.instance.playerHasAttacked)
            {
                AnalyticTracker.instance.FirstAttack();
            }

            if (GameManager.instance.nearNPC)
            {
                AnalyticTracker.instance.NPCInteract("attack");
            }
        }
    }
}
