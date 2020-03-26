using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image health;
    public CombatHealth playerHealth;

    // Update is called once per frame
    void Update()
    {
        health.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
