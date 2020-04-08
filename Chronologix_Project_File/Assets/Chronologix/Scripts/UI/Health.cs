using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image health;
    CombatHealth playerHealth;
    private void Start()
    {
        playerHealth = GameManager.instance.player.GetComponent<CombatHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        health.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
