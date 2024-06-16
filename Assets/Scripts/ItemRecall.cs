using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemRecall : MonoBehaviour
{
    // The object you want to snap back
    public GameObject Target;
    // The time it takes to move back
    public float LerpTime = 2f;
    // The time it takes before it starts moving back
    public float WaitTime = 1f;
    // The number of hands grabbing the object
    private int GrabNumber = 0;
    // Scaling the object if Activated
    private bool Scaled = false;
    // How much we want to scale the object by
    public float Scale_Amount = 1.0f;

    private void Start()
    {
        // Gets the XR Grabbable object and adds method for grabbing and letting go
        XRBaseInteractable component = Target.GetComponent<XRBaseInteractable>();
        component.selectExited.AddListener(SnapObject);
        component.activated.AddListener(ScaleObject);
        component.selectEntered.AddListener(CheckGrabs);
    }

    private void CheckGrabs(SelectEnterEventArgs TargetObject)
    {
        // Increase grab count and stop and current timers
        GrabNumber++;
        StopCoroutine("ReturnObject");
    }

    private void ScaleObject(ActivateEventArgs TargetObject)
    {
        // Turns out you can not scale an object while it is held.
        // So this code does nothing
        if (Scaled)
        {
            Target.transform.localScale.Set(
                Target.transform.localScale.x / Scale_Amount,
                Target.transform.localScale.y / Scale_Amount,
                Target.transform.localScale.z / Scale_Amount);
            Scaled = false;
        }
        else
        {
            Target.transform.localScale.Set(
                Target.transform.localScale.x * Scale_Amount,
                Target.transform.localScale.y * Scale_Amount,
                Target.transform.localScale.z * Scale_Amount);
            Scaled = true;
        }
    }

    private void SnapObject(SelectExitEventArgs TargetObject)
    {
        // Decrement grab number and start coroutine
        GrabNumber--;
        if(GrabNumber <= 0)
        {
            StartCoroutine("ReturnObject");
        }
    }
    private IEnumerator ReturnObject()
    {
        yield return new WaitForSeconds(WaitTime);

        float CurrentTime = 0f;
        Vector3 CurrentPosition = Target.transform.position;

        while(CurrentTime < 2)
        {
            if(GrabNumber != 0)
            {
                break;
            }
            CurrentTime += Time.deltaTime;
            Target.transform.position = Vector3.Lerp(Target.transform.position, transform.position, CurrentTime / LerpTime);
            Target.transform.rotation = Quaternion.Lerp(Target.transform.rotation, transform.rotation, CurrentTime / LerpTime);
            yield return null;
        }

    }
}
