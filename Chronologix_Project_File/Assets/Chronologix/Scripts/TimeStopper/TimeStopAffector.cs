using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopAffector : MonoBehaviour
{
    public float localTimeScale;
    float goalTimeScale;
    float maxSize;
    float activeTimer;
    float timeActive;
    float startRadius;
    public AnimationCurve rateOfGrowth;
    public AnimationCurve rateofShrink;
    public float growthTimer;
    public float maxGrowthTime;
    public float shrinkTimer;
    public float maxShrinkTime;
    SphereCollider timeStopCollider;
    public SidescrollerController player;

    // Start is called before the first frame update
    void Start()
    {
        timeStopCollider = GetComponent<SphereCollider>();
        timeStopCollider.radius = 0;
        startRadius = timeStopCollider.radius;
        maxSize = ( 2 * Camera.main.orthographicSize * Camera.main.aspect )/3/2;
        localTimeScale = 1;
        goalTimeScale = 0.5f;
        timeActive = 5;
        StartCoroutine("ScaleUp");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeStopCollider.radius == maxSize)
        {
            activeTimer += Time.fixedDeltaTime;
            if (activeTimer >= timeActive)
            {
                StartCoroutine("ScaleDown");
            }
        }

        localTimeScale = 1 - ((timeStopCollider.radius - startRadius) / (maxSize - startRadius)) * (1 - goalTimeScale);
    }

    IEnumerator ScaleUp()
    {
        while (growthTimer < maxGrowthTime)
        {
            growthTimer = Mathf.Clamp( growthTimer + Time.deltaTime, 0, maxGrowthTime);
            timeStopCollider.radius = startRadius + (maxSize - startRadius) * (rateOfGrowth.Evaluate(growthTimer / maxGrowthTime));
            yield return new WaitForEndOfFrame();
        }
        growthTimer = 0;
    }

    IEnumerator ScaleDown()
    {
        while (shrinkTimer < maxShrinkTime)
        {
            shrinkTimer = Mathf.Clamp(shrinkTimer + Time.deltaTime, 0, maxShrinkTime);
            timeStopCollider.radius = startRadius + (maxSize - startRadius) * (rateofShrink.Evaluate(shrinkTimer / maxShrinkTime));
            yield return new WaitForEndOfFrame();
        }
        shrinkTimer = 0;
    }
}
