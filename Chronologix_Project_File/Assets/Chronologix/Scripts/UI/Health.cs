using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image health;
    float maxHealth = 100f;
    public static float healthValue;

    // Start is called before the first frame update
    void Start()
    {
        healthValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health.fillAmount = healthValue / maxHealth;

        if (Input.GetKeyDown("h"))
        {
            healthValue -= 10f;
        }
    }
}
