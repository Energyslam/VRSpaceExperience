using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float[,] grid;
    private float verticalCellSize, horizontalCellSize;
    [Range(1.0f, 250)]
    public uint columns, rows, gridSizeX, gridSizeZ;

    private List<GameObject> cells = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        horizontalCellSize = (float)gridSizeX / (float)columns;
        verticalCellSize = (float)gridSizeZ / (float)rows;
        grid = new float[columns, rows];

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject gridCell = new GameObject("GridCell: " + i + "x , " + j + "z");
                gridCell.transform.position = new Vector3(i * horizontalCellSize + horizontalCellSize * 0.5f, 0f, j * verticalCellSize + verticalCellSize * 0.5f) + transform.position;
                gridCell.transform.parent = transform;
                gridCell.tag = "cell";
                BoxCollider collider = gridCell.AddComponent<BoxCollider>();
                collider.center = Vector3.zero;
                collider.size = new Vector3(horizontalCellSize, 0.1f, verticalCellSize);
                cells.Add(gridCell);
            }
        }
    }

    /*
    private void OnValidate()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        horizontalCellSize = (float)gridSizeX / (float)columns;
        verticalCellSize = (float)gridSizeZ / (float)rows;
        grid = new float[columns, rows];

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject gridCell = new GameObject("GridCell: " + i + "x , " + j + "z");
                gridCell.transform.position = new Vector3(i * horizontalCellSize + horizontalCellSize * 0.5f, 0f, j * verticalCellSize + verticalCellSize * 0.5f) + transform.position;
                gridCell.transform.parent = transform;
                BoxCollider collider = gridCell.AddComponent<BoxCollider>();
                collider.center = Vector3.zero;
                collider.size = new Vector3(horizontalCellSize, 0.1f, verticalCellSize);
                cells.Add(gridCell);
            }
        }
    }
    */
}
