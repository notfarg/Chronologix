using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public float damageVal;
    public float knockbackSpeedVal;
    public LayerMask thingsToDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (1<<collision.collider.gameObject.layer == thingsToDamage)
        {
            collision.collider.gameObject.GetComponent<CombatHealth>().currentHealth -= damageVal;
            collision.collider.gameObject.GetComponent<Rigidbody>().AddForce((collision.collider.gameObject.transform.position - transform.position + Vector3.up).normalized * knockbackSpeedVal, ForceMode.VelocityChange);
        }
    }
}
