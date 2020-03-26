using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timestopper : MonoBehaviour
{
    public Image timestopper;
    float startValue = 0.01f;
    public static float timestopperValue;
    public float timestopperCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        timestopperValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        timestopperValue += 0.5f;

        timestopper.fillAmount = timestopperValue * startValue;

        Debug.Log(timestopperValue);

        if (timestopperValue >= 100)
        {
            Debug.Log("timstopper Ready");
            StartCoroutine(RestartTimestopper());
        }
    }

    IEnumerator RestartTimestopper()
    {
        timestopperValue = 0;
        yield return new WaitForSeconds(timestopperCoolDown);
    }
}
