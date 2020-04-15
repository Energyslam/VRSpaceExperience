using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCell : MonoBehaviour
{
    public int horizontal, vertical;
    public Color color;
    public Vector3 scale;
    public bool isAlive;
    public int aliveNeighbours = 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(transform.lossyScale.x * scale.x, transform.lossyScale.y * scale.y, transform.lossyScale.z * scale.z) - new Vector3(0.01f, 0.01f, 0.01f));
    }
}
