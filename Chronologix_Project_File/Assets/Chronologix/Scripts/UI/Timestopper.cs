using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timestopper : MonoBehaviour
{
    public Image timestopper;
    float maxValue = 100f;
    public static float timestopperValue;

    // Start is called before the first frame update
    void Start()
    {
        timestopperValue = maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        timestopperValue -= 0.5f;

        timestopper.fillAmount = timestopperValue / maxValue;

        Debug.Log(timestopperValue);

        if (timestopperValue < 0)
        {
            Debug.Log("timstopper Ready");
        }
    }
}
