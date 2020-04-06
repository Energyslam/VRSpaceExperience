using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float[,] grid;
    private float verticalCellSize, horizontalCellSize;
    [Range(1.0f, 250)]
    public uint columns = 10, rows = 10;
    [Range(1.0f, 250)]
    public float gridSizeX = 10, gridSizeZ = 10;

    [Range(1.0f, 50)]
    public float scale = 1;

    public bool DEBUGGING = false;

    [SerializeField]
    private Vector3 gridCellsOffset;

    [SerializeField]
    private Vector2 perlinNoiseOffset;

    [SerializeField]
    private Transform cellParent;

    private List<GameObject> cells = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        horizontalCellSize = (float)gridSizeX / (float)columns;
        verticalCellSize = (float)gridSizeZ / (float)rows;
        grid = new float[columns, rows];
        gridCellsOffset = new Vector3(-gridSizeX / 2, gridCellsOffset.y, -gridSizeZ / 2);

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject gridCell = new GameObject(i + "+" + j);
                gridCell.transform.position = new Vector3(i * horizontalCellSize + horizontalCellSize * 0.5f, 0f, j * verticalCellSize + verticalCellSize * 0.5f) + transform.position + gridCellsOffset;
                gridCell.transform.parent = cellParent;
                gridCell.tag = "Cell";
                BoxCollider collider = gridCell.AddComponent<BoxCollider>();
                collider.center = Vector3.zero;
                collider.size = new Vector3(horizontalCellSize, 0.1f, verticalCellSize);

                GridCell cellInfo = gridCell.AddComponent<GridCell>();
                cellInfo.horizontal = i;
                cellInfo.vertical = j;
                cellInfo.scale = new Vector3(horizontalCellSize, 0.1f, verticalCellSize);

                float x = (float)i / columns * scale;
                float y = (float)j / rows * scale;

                float noise = Mathf.PerlinNoise(x, y);
                noise = Mathf.Round(noise * 2) / 2;

                Color color = new Color(noise, noise, noise);
                cells.Add(gridCell);
            }
        }
    }

    private void Update() 
    {
        if (DEBUGGING)
        {
            for (int x = 0; x < cells.Count; x++)
            {
                int i = cells[x].GetComponent<GridCell>().horizontal;
                int j = cells[x].GetComponent<GridCell>().vertical;
                cells[x].transform.position = new Vector3(i * horizontalCellSize + horizontalCellSize * 0.5f, 0f, j * verticalCellSize + verticalCellSize * 0.5f) + transform.position + gridCellsOffset;

                cells[x].GetComponent<GridCell>().color = CalculateColor(i, j);
            }
        }
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / (float)columns * (float)scale + perlinNoiseOffset.x;
        float yCoord = (float)y / (float)rows * (float)scale + perlinNoiseOffset.y;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        sample = Mathf.Round(sample * 2) / 2;
        return new Color(sample, sample, sample);
    }
}
