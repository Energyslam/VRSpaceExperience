using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GridManager : MonoBehaviour
{
    public float[,] grid;
    private float verticalCellSize, horizontalCellSize;
    [Range(1.0f, 250)]
    public uint columns, rows, gridSizeX, gridSizeZ;

    // Start is called before the first frame update
    void Start()
    {
        horizontalCellSize = (float)gridSizeX / (float)columns;
        verticalCellSize = (float)gridSizeZ / (float)rows;
        grid = new float[columns, rows];
    }

    private void OnValidate()
    {
        horizontalCellSize = (float)gridSizeX / (float)columns;
        verticalCellSize = (float)gridSizeZ / (float)rows;
        grid = new float[columns, rows];
        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Gizmos.DrawCube(new Vector3(i * horizontalCellSize + horizontalCellSize * 0.5f, 0f, j * verticalCellSize + verticalCellSize * 0.5f) + transform.position, new Vector3(horizontalCellSize - 0.2f, 0.1f, verticalCellSize - 0.2f));
            }
        }
    }
}
