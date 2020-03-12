using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSpawner : MonoBehaviour
{
    public GameObject attackPrefab;
    public bool facingLeft;
    GameObject currentAttack;
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
        }
        
    }
}
