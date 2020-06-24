using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowUntilHit : MonoBehaviour
{
    private bool stopGrowing = false;

    [SerializeField]
    GameObject swingParent, lamp;

    [SerializeField]
    private bool addSwing = false;

    [SerializeField]
    private float swingAmount = 5.0f;

    private void FixedUpdate()
    {
        if (!stopGrowing)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 1.01f, transform.localScale.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!stopGrowing)
        {
            stopGrowing = true;
            swingParent.transform.position = new Vector3(swingParent.transform.position.x, other.transform.position.y, swingParent.transform.position.z);
            transform.parent = swingParent.transform;
            lamp.transform.parent = swingParent.transform;
            if (addSwing)
            {
                Swing swing = swingParent.AddComponent<Swing>();
                swing.randomRotation = true;
                swing.rotation = swingAmount;
            }
        }
    }
}
