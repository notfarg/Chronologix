using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float damageVal;
    public float knockbackSpeedVal;
    public LayerMask thingsToDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == thingsToDamage)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * knockbackSpeedVal, ForceMode.VelocityChange);
            other.gameObject.GetComponent<AgentMotor3D>().currentVertVelocity = Vector3.up * knockbackSpeedVal; 
            other.gameObject.GetComponent<CombatHealth>().currentHealth -= damageVal * Time.deltaTime;

            if (thingsToDamage == (1 << LayerMask.NameToLayer("Player")))
            {
                AnalyticTracker.instance.DamageTaken(this.gameObject.name);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (1 << other.gameObject.layer == thingsToDamage)
        {
            other.gameObject.GetComponent<CombatHealth>().currentHealth -= damageVal * Time.deltaTime;

            if (thingsToDamage == (1 << LayerMask.NameToLayer("Player")))
            {
                AnalyticTracker.instance.DamageTaken(this.gameObject.name);
            }
        }
    }
}
