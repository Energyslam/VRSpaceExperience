using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleAnimations : MonoBehaviour
{
    [SerializeField]
    private Vector3 openPosition;

    [SerializeField]
    private float animationSpeed = 5;

    private Vector3 closedPosition;

    private void Start()
    {
        closedPosition = transform.localPosition;
    }

    public void Animate(bool hasToOpen)
    {
        Vector3 newPosition;

        newPosition = hasToOpen ? openPosition : closedPosition;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, animationSpeed / 100);

        if (transform.localPosition != newPosition)
            Animate(hasToOpen);
    }
}
