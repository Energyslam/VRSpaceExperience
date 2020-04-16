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
    private int iterateAmount = 5;

    private int hasIterated = 0;

    public enum SpawningPhase { BIG, MEDIUM, SMALL, WALL, ROOF, TABLE };

    private SpawningPhase currentSpawningPhase = SpawningPhase.BIG;

    [SerializeField]
    private float aliveThreshold = 0.5f;

    private List<GridCell> cells = new List<GridCell>();

    [SerializeField]
    private GameObject objectsHolder, connectableBigObject, standaloneBigObject;

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

                SetUpGrid(i, j); 

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
                hasIterated = 0;
                StartCoroutine(IterateGameOfLife(grid, columns, rows));
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

    // Calculates and sets the color based on Perlin noise. Black means a cell is alive, white means dead
    private void SetUpGrid(int x, int y)
    {
        float xCoord = (float)x / (float)columns * (float)scale + perlinNoiseOffset.x;
        float yCoord = (float)y / (float)rows * (float)scale + perlinNoiseOffset.y;

        float fSample = Mathf.PerlinNoise(xCoord, yCoord);

        bool sample = fSample >= aliveThreshold;

        grid[x, y].isAlive = sample;
    }

    private IEnumerator IterateGameOfLife(GridCell[,] grid, int M, int N)
    {
        yield return new WaitForSeconds(0.1f);

        if (hasIterated < iterateAmount)
        {
            hasIterated++;
            this.grid = ExecuteGameOfLife(grid, M, N);
            StartCoroutine(IterateGameOfLife(grid, M, N));
        }

        else
            SpawnObjects();
    }

    private void SpawnObjects()
    {
        switch (currentSpawningPhase)
        {
            case SpawningPhase.BIG:
                SpawnBigObjects();
                break;
        }

        iterateAmount = 0;
    }

    private void SpawnBigObjects()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (grid[i, j].vertical > 0)
                {
                    if (grid[i, j].horizontal == 0 && grid[i, j].vertical <= 2 || grid[i, j].horizontal == rows - 1 && grid[i, j].vertical <= 2)
                        continue;

                    else
                        grid[i, j].isAlive = false;
                }
            }
        }

        grid = FindNeighbours(grid, columns, rows);

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (grid[i, j].isAlive)
                {
                    if (grid[i, j].aliveNeighbours == 0)
                    {
                        GameObject obj = Instantiate(standaloneBigObject, grid[i, j].transform.position, Quaternion.identity, objectsHolder.transform);
                        obj.transform.localScale = grid[i, j].totalScale;
                    }

                    else
                    {
                        GameObject obj = Instantiate(connectableBigObject, grid[i, j].transform.position, Quaternion.identity, objectsHolder.transform);
                        obj.transform.localScale = grid[i, j].totalScale;
                    }
                }
            }
        }
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

                // Top row stays filled
                if (grid[x, y].isAlive && grid[x, y].vertical == 0)
                    future[x, y].isAlive = grid[x, y].isAlive;

                // Lonelyness
                else if (grid[x, y].isAlive && grid[x, y].aliveNeighbours < 2)
                    future[x, y].isAlive = false;

                // Overpopulation
                else if (grid[x, y].isAlive && grid[x, y].aliveNeighbours > 3)
                    future[x, y].isAlive = false;

                // Birth
                else if (!grid[x, y].isAlive && grid[x, y].aliveNeighbours == 3)
                    future[x, y].isAlive = true;

                // Cell remains dead/alive
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
