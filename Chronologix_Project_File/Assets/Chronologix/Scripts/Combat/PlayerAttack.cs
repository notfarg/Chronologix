using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 0.2f);
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // deal damage to enemy
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Breakable"))
        {
            // deal damage to breakable object
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Switch"))
        {
            // trigger switch swap
        }
    }
}
