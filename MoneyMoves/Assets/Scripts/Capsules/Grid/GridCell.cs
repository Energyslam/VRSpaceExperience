using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCell : MonoBehaviour
{
    public int vertical, horizontal;
    public Color color;
    public Vector3 scale, position;
    public bool isAlive;
    public int aliveNeighbours = 0;
    public bool hasSpawnedAnObject = false;

    public enum RotateTowards { TOP, RIGHT, BOTTOM, LEFT};
    public RotateTowards rotation = RotateTowards.TOP;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        position = transform.position;
        Vector3 totalScale = new Vector3(transform.lossyScale.x * scale.x, transform.lossyScale.y * scale.y, transform.lossyScale.z * scale.z) - new Vector3(0.01f, 0.01f, 0.01f);
        Gizmos.DrawCube(position, totalScale);
    }
}
