using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Rendering;

public class ItemRecall : MonoBehaviour
{
    // The mesh you want to snap back
    public GameObject Target;
    // The time it takes to move back
    public float LerpTime = 2f;
    // The time it takes before it starts moving back
    public float WaitTime = 1f;
    
    // The number of hands grabbing the object
    private int GrabNumber = 0;

    private Quaternion original_rotation;
    private Vector3 original_position;

    private void Start()
    {
        // Gets the XR Grabbable object and adds method for grabbing and letting go
        XRBaseInteractable component = Target.GetComponent<XRBaseInteractable>();
        component.selectExited.AddListener(SnapObject);
        component.selectEntered.AddListener(CheckGrabs);
        original_position = Target.transform.localPosition;
        original_rotation = Target.transform.localRotation;
    }

    private void CheckGrabs(SelectEnterEventArgs TargetObject)
    {
        // Increase grab count and stop and current timers
        GrabNumber++;
        StopCoroutine("ReturnObject");
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
            Target.transform.localPosition = Vector3.Lerp(Target.transform.localPosition, original_position, CurrentTime / LerpTime);
            Target.transform.localRotation = Quaternion.Lerp(Target.transform.localRotation, original_rotation, CurrentTime / LerpTime);
            yield return null;
        }

    }
}
