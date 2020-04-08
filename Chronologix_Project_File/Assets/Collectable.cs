using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.score += 5;
        Destroy(this.gameObject);
    }
}
