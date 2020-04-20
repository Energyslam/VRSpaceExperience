using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    public GridCell[,] grid;

    private float verticalCellSize, horizontalCellSize;
    [Range(1.0f, 250)]
    public int columns = 10, rows = 10;
    [Range(1.0f, 250)]
    public float gridSizeX = 10, gridSizeZ = 10;

    [Range(1.0f, 50)]
    public float perlinScale = 1;

    public bool DEBUGGING = false;

    private int totalAlive = 0;

    [SerializeField]
    private Vector3 gridCellsOffset;

    [SerializeField]
    private Vector2 perlinNoiseOffset;

    [SerializeField]
    private GameObject gridCellPrefab;

    [SerializeField]
    private Transform cellParent;

    private int hasIterated = 0;

    public enum SpawningPhase { BIG, MEDIUM, SMALL, WALL, ROOF, TABLE };

    [SerializeField]
    private SpawningPhase currentSpawningPhase = SpawningPhase.BIG;

    [SerializeField]
    private float aliveThreshold = 0.5f;

    private List<GridCell> cells = new List<GridCell>();

    [SerializeField]
    private GameObject objectsHolder, connectableBigObject, standaloneBigObject, mediumTables, mediumGuitar;
    #endregion

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
                cellInfo.vertical = i;
                cellInfo.horizontal = j;
                cellInfo.scale = new Vector3(horizontalCellSize, 0.1f, verticalCellSize);

                if (cellInfo.vertical < columns / 2)
                    cellInfo.rotation = GridCell.RotateTowards.TOP;

                else if (cellInfo.vertical >= columns / 2)
                    cellInfo.rotation = GridCell.RotateTowards.BOTTOM;

                if (cellInfo.horizontal == 0)
                {
                    if (cellInfo.vertical > 0 && cellInfo.vertical < rows - 1)
                        cellInfo.rotation = GridCell.RotateTowards.LEFT;
                }

                else if (cellInfo.horizontal == columns - 1)
                {
                    if (cellInfo.vertical > 0 && cellInfo.vertical < rows - 1)
                        cellInfo.rotation = GridCell.RotateTowards.RIGHT;
                }

                grid[i, j] = cellInfo;

                SetUpGrid(i, j);

                cells.Add(cellInfo);
            }
        }

        grid = FindNeighbours(grid, columns, rows);
        StartCoroutine(IterateGameOfLife(grid, columns, rows, currentSpawningPhase, 15));
    }

    private void Update()
    {
        if (DEBUGGING)
        {
            for (int x = 0; x < cells.Count; x++)
            {
                GridCell cell = cells[x];
                int i = cell.vertical;
                int j = cell.horizontal;
                cell.isAlive = grid[i, j].isAlive;
                cells[x].transform.position = new Vector3(i * horizontalCellSize + horizontalCellSize * 0.5f, 0f, j * verticalCellSize + verticalCellSize * 0.5f) + transform.position + gridCellsOffset;
                cell.color = grid[i, j].isAlive ? new Color(0, 0, 0) : new Color(1, 1, 1);
            }
        }
    }

    // Calculates and sets the color based on Perlin noise. Black means a cell is alive, white means dead
    private void SetUpGrid(int x, int y)
    {
        perlinScale = Random.Range(1, 50);

        float xCoord = (float)x / (float)columns * (float)perlinScale + perlinNoiseOffset.x;
        float yCoord = (float)y / (float)rows * (float)perlinScale + perlinNoiseOffset.y;

        float fSample = Mathf.PerlinNoise(xCoord, yCoord);

        bool sample = fSample >= aliveThreshold;

        if (!grid[x, y].hasSpawnedAnObject)
        {
            grid[x, y].isAlive = sample;
            totalAlive++;
        }

        else
            grid[x, y].isAlive = false;
    }

    private IEnumerator IterateGameOfLife(GridCell[,] grid, int M, int N, SpawningPhase phase, int iterateAmount)
    {
        yield return new WaitForSeconds(0.00f);

        if (hasIterated < iterateAmount)
        {
            hasIterated++;
            this.grid = ExecuteGameOfLife(grid, M, N, phase);
            StartCoroutine(IterateGameOfLife(grid, M, N, phase, iterateAmount));
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

            case SpawningPhase.MEDIUM:
                SpawnMediumObjects();
                break;
        }
    }

    private void SpawnBigObjects()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (grid[i, j].horizontal > 0)
                {
                    if (grid[i, j].vertical == 0 && grid[i, j].horizontal <= 2 || grid[i, j].vertical == rows - 1 && grid[i, j].horizontal <= 2)
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
                        SpawnObjectOnGridCell(i, j, standaloneBigObject, new Vector3(gridSizeX, 1, gridSizeZ));
                    }

                    else
                    {
                        SpawnObjectOnGridCell(i, j, connectableBigObject, new Vector3(gridSizeX, 1, gridSizeZ));
                    }
                }
            }
        }

        GoToNextPhase();
    }

    private void SpawnMediumObjects()
    {
        int spawnedMedium = 0;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (grid[x, y].horizontal < 3)
                {
                    grid[x, y].isAlive = false;
                    continue;
                }

                // Overpopulation
                if (grid[x, y].isAlive && grid[x, y].aliveNeighbours > 0)
                    grid[x, y].isAlive = false;

                // Only allow 2 alive edge cells
                if (grid[x, y].vertical == 0 || grid[x, y].vertical == rows - 1)
                {
                    // Kill all alive cells but the first two
                    if (spawnedMedium >= 2)
                        grid[x, y].isAlive = false;

                    else 
                        if (grid[x, y].isAlive)
                            spawnedMedium++;
                }

                else
                    grid[x, y].isAlive = false;

                if (grid[x, y].horizontal == columns - 1 && grid[x, y].vertical == rows - 1)
                {
                    if (spawnedMedium < 2)
                    {
                        grid[x, y].isAlive = true;
                        spawnedMedium++;
                    }
                }

                grid = FindNeighbours(grid, columns, rows);

                if (grid[x, y].isAlive)
                {
                    SpawnObjectOnGridCell(x, y, mediumGuitar, new Vector3(1.0f, 1.0f, 1.0f));
                }
            }            
        }

        int newX = Random.Range(2, columns - 3);
        int newY = Random.Range(4, rows - 3);
        SpawnObjectOnGridCell(newX, newY, mediumTables, new Vector3(1.0f, 1.0f, 1.0f));
    }

    private void GoToNextPhase()
    {
        currentSpawningPhase = SpawningPhase.MEDIUM;
        hasIterated = 0;
        totalAlive = 0;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                aliveThreshold = 0.55f;
                SetUpGrid(x, y);
            }
        }

        if (totalAlive < 3)
        {
            GoToNextPhase();
        }

        grid = FindNeighbours(grid, columns, rows);

        StartCoroutine(IterateGameOfLife(grid, columns, rows, currentSpawningPhase, 5));
    }

    private void SpawnObjectOnGridCell(int i, int j, GameObject toSpawn, Vector3 upscaleFactor)
    {
        GameObject obj = Instantiate(toSpawn, grid[i, j].transform.position, Quaternion.identity, objectsHolder.transform);
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * upscaleFactor.x, obj.transform.localScale.y * ((upscaleFactor.x + upscaleFactor.z) / 2), obj.transform.localScale.z * upscaleFactor.z) + new Vector3(0.1f * upscaleFactor.x + 0.1f, 0.0f, 0.1f * upscaleFactor.z + 0.1f);
        switch (grid[i, j].rotation)
        {
            case GridCell.RotateTowards.LEFT:
                break;

            case GridCell.RotateTowards.RIGHT:
                obj.transform.eulerAngles = new Vector3(0, 180, 0);
                break;

            case GridCell.RotateTowards.TOP:
                obj.transform.eulerAngles = new Vector3(0, 90, 0);
                break;

            case GridCell.RotateTowards.BOTTOM:
                obj.transform.eulerAngles = new Vector3(0, -90, 0);
                break;
        }

        grid[i, j].hasSpawnedAnObject = true;
    }

    // Function to print next generation 
    private GridCell[,] ExecuteGameOfLife(GridCell[,] grid, int M, int N, SpawningPhase phase)
    {
        GridCell[,] future = new GridCell[M, N];

        future = grid;

        future = FindNeighbours(grid, M, N);

        for (int x = 0; x < M; x++)
        {
            for (int y = 0; y < N; y++)
            {
                // Implementing the Rules of Life 
                switch (phase)
                {
                    case SpawningPhase.BIG:
                        future = GameOfLifeBig(x, y, future);
                        break;
                    case SpawningPhase.MEDIUM:
                        future = GameOfLifeMedium(x, y, future);
                        break;
                }
            }
        }

        return future;
    }

    private GridCell[,] GameOfLifeBig(int x, int y, GridCell[,] future)
    {
        // Top row stays filled
        if (grid[x, y].isAlive && grid[x, y].horizontal == 0)
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

        return future;
    }

    private GridCell[,] GameOfLifeMedium(int x, int y, GridCell[,] future)
    { 
        // Overpopulation
        if (grid[x, y].isAlive && grid[x, y].aliveNeighbours > 0)
            future[x, y].isAlive = false;

        // Birth
        else if (!grid[x, y].isAlive && grid[x, y].aliveNeighbours == 0)
            future[x, y].isAlive = true;

        // Cell remains dead/alive
        else
            future[x, y].isAlive = grid[x, y].isAlive;

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
