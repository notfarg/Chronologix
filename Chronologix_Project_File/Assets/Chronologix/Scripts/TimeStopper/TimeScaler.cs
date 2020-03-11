using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: ATTACH THIS SCRIPT TO ANYTHING WE WANT TO BE AFFECTED BY THE TIME STOPPER

public class TimeScaler : MonoBehaviour
{
    Rigidbody rBody;
    public GameObject fakeBase;
    public float timeScale;
    public bool inTimeStopper;
    public TimeStopAffector timeStopper;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (inTimeStopper)
        {
            timeScale = timeStopper.localTimeScale;
            rBody.velocity = fakeBase.GetComponent<Rigidbody>().velocity * timeScale;
            fakeBase.transform.position = this.transform.position;
        } else
        {
            timeScale = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TimeStopper"))
        {
            inTimeStopper = true;
            timeStopper = other.gameObject.GetComponent<TimeStopAffector>();
            fakeBase = new GameObject();
            CopyComponent(rBody, fakeBase);
            CopyComponent(GetComponent<Collider>(), fakeBase);
            fakeBase.layer = LayerMask.NameToLayer("PhysicsSim");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TimeStopper"))
        {
            inTimeStopper = false;
            timeStopper = null;
            Destroy(fakeBase);
        }
    }
    Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

}
