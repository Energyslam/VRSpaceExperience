using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorGizmo : MonoBehaviour
{ 
    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 10;
        Gizmos.DrawRay(transform.position, direction);
    }
}
