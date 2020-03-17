using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    public Color col;
    private void OnDrawGizmos()
    {
        Gizmos.color = col;
        Gizmos.DrawSphere(transform.position, 1f);
    }
}
