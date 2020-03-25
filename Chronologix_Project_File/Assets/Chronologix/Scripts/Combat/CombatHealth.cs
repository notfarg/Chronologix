using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHealth : MonoBehaviour
{
    public float currentHealth;

    private void Update()
    {
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // do something different for the player
        } else
        {
            // Destroy anything else. add death animations here prior to that.
            Destroy(gameObject);
        }
    }
}
