using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmoLine : MonoBehaviour
{
    [SerializeField]
    private List<Transform> targets;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            Gizmos.DrawLine(transform.position, targets[i].position);
        }
    }
}
