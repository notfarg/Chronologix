using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public Image sprite;
    public float fadeInTime;
    public Color newColour;

    public GameObject screen;

    // Start is called before the first frame update
    void Start()
    {
        screen.SetActive(true);
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = Color.Lerp(sprite.color, newColour, fadeInTime * Time.deltaTime);
    }

    IEnumerator Destroy()
    {
        Debug.Log("start");
        yield return new WaitForSeconds(5);
        screen.SetActive(false);
    }
}