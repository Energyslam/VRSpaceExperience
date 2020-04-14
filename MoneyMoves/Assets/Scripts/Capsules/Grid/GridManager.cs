using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
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
    private GameObject gridCellPrefab;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private float aliveThreshold = 0.5f;

    private List<GameObject> cells = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        horizontalCellSize = (float)gridSizeX / (float)columns;
        verticalCellSize = (float)gridSizeZ / (float)rows;
        grid = new float[columns, rows];
        gridCellsOffset = new Vector3(-gridSizeX / 2, gridCellsOffset.y, -gridSizeZ / 2);

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject gridCell = Instantiate(gridCellPrefab);
                gridCell.name = "Cell: " + i + ", " + j;
                gridCell.transform.position = new Vector3(i * horizontalCellSize + horizontalCellSize * 0.5f, 0f, j * verticalCellSize + verticalCellSize * 0.5f) + transform.position + gridCellsOffset;
                gridCell.transform.parent = cellParent;
                BoxCollider collider = gridCell.GetComponent<BoxCollider>();
                collider.center = Vector3.zero;
                collider.size = new Vector3(horizontalCellSize, 0.1f, verticalCellSize);

                GridCell cellInfo = gridCell.GetComponent<GridCell>();
                cellInfo.horizontal = i;
                cellInfo.vertical = j;
                cellInfo.scale = new Vector3(horizontalCellSize, 0.1f, verticalCellSize);

                CalculateColor(i, j); 

                cells.Add(gridCell);
            }
        }
    }

    private void Update() 
    {
        if (DEBUGGING)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int x = 0; x < cells.Count; x++)
                {
                    GridCell cell = cells[x].GetComponent<GridCell>();

                    int i = cell.horizontal;
                    int j = cell.vertical;

                    CountLiveNeighbors(i, j);
                }
            }   

            for (int x = 0; x < cells.Count; x++)
            {
                GridCell cell = cells[x].GetComponent<GridCell>();
                int i = cell.horizontal;
                int j = cell.vertical;
                cell.value = grid[i, j];
                cells[x].transform.position = new Vector3(i * horizontalCellSize + horizontalCellSize * 0.5f, 0f, j * verticalCellSize + verticalCellSize * 0.5f) + transform.position + gridCellsOffset;
                cell.color = new Color(grid[i, j], grid[i, j], grid[i, j]);
            }
        }
    }

    // Calculates and sets the color based on Perlin noise. Black (float of 0) means a cell is alive, white means dead
    private void CalculateColor(int x, int y)
    {
        float xCoord = (float)x / (float)columns * (float)scale + perlinNoiseOffset.x;
        float yCoord = (float)y / (float)rows * (float)scale + perlinNoiseOffset.y;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        sample = sample >= aliveThreshold ? 0 : 1;

        grid[x, y] = sample;
    }

    // Returns the number of live neighbors around the cell at position (x,y).
    private void CountLiveNeighbors(int x, int y)
    {
        float[,] future = new float[columns, rows];

        // Loop through every cell 
        for (int l = 1; l < x - 1; l++)
        {
            for (int m = 1; m < y - 1; m++)
            {

                // finding no Of Neighbours 
                // that are alive 
                float aliveNeighbours = 0;
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        aliveNeighbours += grid[l + i, m + j];

                // The cell needs to be subtracted 
                // from its neighbours as it was  
                // counted before 
                aliveNeighbours -= grid[l, m];

                // Implementing the Rules of Life 

                // Cell is lonely and dies 
                if ((grid[l, m] == 1) &&
                            (aliveNeighbours < 2))
                    future[l, m] = 0;

                // Cell dies due to over population 
                else if ((grid[l, m] == 1) &&
                             (aliveNeighbours > 3))
                    future[l, m] = 0;

                // A new cell is born 
                else if ((grid[l, m] == 0) &&
                            (aliveNeighbours == 3))
                    future[l, m] = 1;

                // Remains the same 
                else
                    future[l, m] = grid[l, m];
            }
        }
        grid = future;
    }
}
