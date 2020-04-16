using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    public GridCell[,] grid;

    private float verticalCellSize, horizontalCellSize;
    [Range(1.0f, 250)]
    public int columns = 10, rows = 10;
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

    private List<GridCell> cells = new List<GridCell>();

    // Start is called before the first frame update
    void Awake()
    {
        horizontalCellSize = (float)gridSizeX / (float)columns;
        verticalCellSize = (float)gridSizeZ / (float)rows;
        grid = new GridCell[columns, rows];
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
                grid[i, j] = cellInfo;

                CalculateColor(i, j); 

                cells.Add(cellInfo);
            }
        }

        grid = FindNeighbours(grid, columns, rows);
    }

    private void Update() 
    {
        if (DEBUGGING)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                grid = ExecuteGameOfLife(grid, columns, rows);
            }   

            for (int x = 0; x < cells.Count; x++)
            {
                GridCell cell = cells[x];
                int i = cell.horizontal;
                int j = cell.vertical;
                cell.isAlive = grid[i, j].isAlive;
                cells[x].transform.position = new Vector3(i * horizontalCellSize + horizontalCellSize * 0.5f, 0f, j * verticalCellSize + verticalCellSize * 0.5f) + transform.position + gridCellsOffset;
                cell.color = grid[i, j].isAlive ? new Color(0, 0, 0) : new Color(1, 1 , 1);
            }
        }
    }

    // Calculates and sets the color based on Perlin noise. Black (float of 0) means a cell is alive, white means dead
    private void CalculateColor(int x, int y)
    {
        float xCoord = (float)x / (float)columns * (float)scale + perlinNoiseOffset.x;
        float yCoord = (float)y / (float)rows * (float)scale + perlinNoiseOffset.y;

        float fSample = Mathf.PerlinNoise(xCoord, yCoord);

        bool sample = fSample >= aliveThreshold;

        grid[x, y].isAlive = sample;
    }

    // Function to print next generation 
    private GridCell[,] ExecuteGameOfLife(GridCell[,] grid, int M, int N)
    {
        GridCell[,] future = new GridCell[M, N];

        future = grid;

        future = FindNeighbours(grid, M, N);

        for (int x = 0; x < M; x++)
        {
            for (int y = 0; y < N; y++)
            {
                // Implementing the Rules of Life 

                if (grid[x, y].isAlive && grid[x, y].aliveNeighbours < 2)
                    future[x, y].isAlive = false;

                else if (grid[x, y].isAlive && grid[x, y].aliveNeighbours > 3)
                    future[x, y].isAlive = false;

                else if (!grid[x, y].isAlive && grid[x, y].aliveNeighbours == 3)
                    future[x, y].isAlive = true;

                else
                    future[x, y].isAlive = grid[x, y].isAlive;    
            }
        }

        return future;
    }

    private GridCell[,] FindNeighbours(GridCell[,] gridWithCells, int M, int N)
    {
        GridCell[,] newGrid = new GridCell[M, N];

        newGrid = gridWithCells;

        // Loop through every cell 
        for (int l = 0; l < M; l++)
        {
            for (int m = 0; m < N; m++)
            {
                // finding no Of Neighbours 
                // that are alive 
                int aliveNeighbours = 0;

                for (int i = -1; i <= 1; i++)
                {
                    if (l + i < 0 || l + i >= M)
                    {
                        continue;
                    }

                    for (int j = -1; j <= 1; j++)
                    {
                        if (m + j < 0 || m + j >= M)
                        {
                            continue;
                        }

                        if (grid[l + i, m + j].isAlive)
                            aliveNeighbours++;
                    }
                }

                if (grid[l, m].isAlive)
                    aliveNeighbours--;

                newGrid[l, m].aliveNeighbours = aliveNeighbours;
            }
        }

        return newGrid;
    }
}
