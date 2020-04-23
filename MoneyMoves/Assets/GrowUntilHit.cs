using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowUntilHit : MonoBehaviour
{
    private bool stopGrowing = false;

    private void FixedUpdate()
    {
        if (!stopGrowing)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 1.01f, transform.localScale.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        stopGrowing = true;
    }
}
