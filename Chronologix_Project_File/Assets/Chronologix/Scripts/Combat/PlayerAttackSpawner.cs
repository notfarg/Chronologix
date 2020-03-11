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
                currentAttack = Instantiate(attackPrefab, transform.position + Vector3.left, Quaternion.identity);
            }
            else
            {
                currentAttack = Instantiate(attackPrefab, transform.position + Vector3.right, Quaternion.identity);
            }
            Destroy(currentAttack, 0.2f);
        }
        
    }
}
