using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int horizontal, vertical;
    public Color color;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(transform.parent.localScale.x * transform.localScale.x, transform.parent.localScale.y * transform.localScale.y, transform.parent.localScale.z * transform.localScale.z));
    }
}
