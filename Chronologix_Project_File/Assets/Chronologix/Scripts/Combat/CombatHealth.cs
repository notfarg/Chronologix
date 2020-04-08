using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

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
            GameManager.instance.LoadScene(0);
        } else
        {
            // Destroy anything else. add death animations here prior to that.
            AnalyticTracker.instance.EnemyKilled();
            GameManager.instance.score += 10;
            Destroy(gameObject);
        }
    }
}
