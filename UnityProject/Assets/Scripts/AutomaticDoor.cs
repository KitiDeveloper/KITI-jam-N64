using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    public float liftDistance;          // How far to lift the door
    public float liftSpeed;             // How quickly to lift the door
    public Transform doorTransform;     // Reference to the door's transform component
    public Transform playerTransform;   // Reference to the player's transform component
    private Vector3 originalPosition;   // Original position of the door
    private bool isLifting;             // Flag to indicate if the door is currently being lifted
    private bool isPlayerInside = false;

    void Start()
    {
        originalPosition = doorTransform.position;
        isLifting = false;
    }

    void Update()
    {
        if (Vector3.Distance(doorTransform.position, playerTransform.position) < liftDistance && !isLifting)
        {
            StartCoroutine(LiftDoor());
        }
        else if (Vector3.Distance(doorTransform.position, playerTransform.position) > liftDistance && isLifting && !isPlayerInside)
        {
            StartCoroutine(LowerDoor());
        }
    }

    IEnumerator LiftDoor()
    {
        isLifting = true;

        while (doorTransform.position.y < originalPosition.y + liftDistance)
        {
            doorTransform.Translate(Vector3.up * liftSpeed * Time.deltaTime);
            yield return null;
        }

        isPlayerInside = false;

        yield return null;
    }

    IEnumerator LowerDoor()
    {
        isPlayerInside = true;

        while (doorTransform.position.y > originalPosition.y)
        {
            doorTransform.Translate(Vector3.down * liftSpeed * Time.deltaTime);
            yield return null;
        }

        isLifting = false;
        yield return null;
    }
}
