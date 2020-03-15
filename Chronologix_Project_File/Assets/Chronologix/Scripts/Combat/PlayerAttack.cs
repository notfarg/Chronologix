using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float damageVal;
    public float knockbackSpeedVal;
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
            other.gameObject.GetComponent<CombatHealth>().currentHealth -= damageVal;
            other.gameObject.GetComponent<Rigidbody>().AddForce((other.gameObject.transform.position - transform.position + Vector3.up).normalized * knockbackSpeedVal, ForceMode.VelocityChange);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Breakable"))
        {
            // deal damage to breakable object
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Switch"))
        {
            // trigger switch swap
        }
    }
}
