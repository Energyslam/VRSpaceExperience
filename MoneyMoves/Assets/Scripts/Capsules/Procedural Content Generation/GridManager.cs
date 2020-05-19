﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public enum SpawningPhase { BIG, MEDIUM, FINISHED };

    [SerializeField]
    private SpawningPhase currentSpawningPhase = SpawningPhase.BIG;

    private float aliveThreshold = 0.5f;

    [SerializeField]
    private float aliveThresholdBig = 0.5f, aliveThresholdMedium = 0.55f;

    private List<GridCell> cells = new List<GridCell>();

    [SerializeField]
    private GameObject objectsHolder, connectableBigObject, cornerConnectableBigObject, standaloneBigObject, mediumMultiObject, mediumSingleObject, topOfMediumMulti;

    [SerializeField]
    private Transform topWall, rightWall, botWall, leftWall;

    private Quaternion originalRotation;
    #endregion

    private void Awake()
    {
        Initialize();
    }

    public void DoTheGameOfLifeThing()
    {
        //Doesn't work if not done in awake. If initialize/reset gets called anywhere after awake, grid gets wrong rotation. Might be worth fixing for dynamic PCG ?
        Initialize();
    }
    // Sets up grid and spawns objects using game of life
    public void Initialize()
    {
        originalRotation = transform.rotation;
        transform.rotation = Quaternion.identity;

        // Calculating grid size
        horizontalCellSize = (float)gridSizeX / (float)columns;
        verticalCellSize = (float)gridSizeZ / (float)rows;
        grid = new GridCell[columns, rows];
        gridCellsOffset = new Vector3(-gridSizeX / 2, gridCellsOffset.y, -gridSizeZ / 2);

        // Resetting Game of Life variables
        aliveThreshold = aliveThresholdBig;
        currentSpawningPhase = SpawningPhase.BIG;
        totalAlive = 0;

        // Spawning grid
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject gridCell = Instantiate(gridCellPrefab);
                gridCell.name = "Cell: " + i + ", " + j;
                gridCell.transform.position = new Vector3(i * horizontalCellSize + horizontalCellSize * 0.5f, 0f, j * verticalCellSize + verticalCellSize * 0.5f) + transform.position + gridCellsOffset;
                gridCell.transform.parent = cellParent;

                GridCell cellInfo = gridCell.GetComponent<GridCell>();
                cellInfo.vertical = i;
                cellInfo.horizontal = j;
                cellInfo.scale = new Vector3(horizontalCellSize, 0.1f, verticalCellSize);

                if (cellInfo.vertical < columns / 2)
                {
                    cellInfo.rotation = GridCell.RotateTowards.TOP;
                }

                else if (cellInfo.vertical >= columns / 2)
                {
                    cellInfo.rotation = GridCell.RotateTowards.BOTTOM;
                }

                if (cellInfo.horizontal == 0)
                {
                    if (cellInfo.vertical > 0)
                        if (cellInfo.vertical <= rows - 1)
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

    private void Reset()
    {
        Clean();
        Initialize();
    }

    public void Clean()
    {
        if (objectsHolder.transform.childCount > 0)
        {
            for (int i = 0; i < objectsHolder.transform.childCount; i++)
            {
                Destroy(objectsHolder.transform.GetChild(i).gameObject);
            }
        }

        if (cellParent.transform.childCount > 0)
        {
            for (int i = 0; i < cellParent.transform.childCount; i++)
            {
                Destroy(cellParent.transform.GetChild(i).gameObject);
            }
        }

        cells.Clear();
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

        if (Input.GetKeyDown(KeyCode.Backspace))
            Reset();
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
                GoToNextPhase(currentSpawningPhase);
                break;

            case SpawningPhase.MEDIUM:
                SpawnMediumObjects();
                GoToNextPhase(currentSpawningPhase);
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
                    if (grid[i, j].vertical == 0 && grid[i, j].horizontal <= 4 || grid[i, j].vertical == rows - 1 && grid[i, j].horizontal <= 4)
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
                        if (grid[i, j].horizontal == 0 && grid[i, j].vertical == 0 || grid[i, j].horizontal == 0 && grid[i, j].vertical == rows - 1)
                            continue;

                        SpawnObjectOnGridCell(i, j, standaloneBigObject, new Vector3(gridSizeX, 1, gridSizeZ), 1, Vector3.zero, 0);
                    }

                    else
                    {
                        if (grid[i, j].horizontal == 0 && grid[i, j].vertical == 0 || grid[i, j].horizontal == 0 && grid[i, j].vertical == rows - 1)
                            SpawnObjectOnGridCell(i, j, cornerConnectableBigObject, new Vector3(gridSizeX, 1, gridSizeZ), 0, Vector3.zero, 0);

                        else
                            SpawnObjectOnGridCell(i, j, connectableBigObject, new Vector3(gridSizeX, 1, gridSizeZ), 0, Vector3.zero, 0);
                    }
                }
            }
        }
    }

    private void SpawnMediumObjects()
    {
        int spawnedMedium = 0;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (grid[x, y].horizontal <= 4)
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

                if (grid[x, y].isAlive && !grid[x, y].hasSpawnedAnObject)
                {
                    SpawnObjectOnGridCell(x, y, mediumSingleObject, new Vector3(1.0f, 1.0f, 1.0f), 0, Vector3.zero, 0.05f);
                }
            }            
        }

        PlaceRandomly(mediumMultiObject, 0, topOfMediumMulti, Random.Range(2, 4));
    }

    private void PlaceRandomly(GameObject toSpawn, int tries, GameObject onTop, float yOffset)
    {
        // Spawn multi object, such as a table
        int newX = Random.Range(3, columns - 3);
        int newY = Random.Range(3, rows - 3);

        float random = Random.Range(0, 3);
        random = Mathf.RoundToInt(random);
        Vector3 rotation = Vector3.zero;

        if (random < 1)
            rotation = new Vector3(0, -90, 0);

        else if (random > 1 && random < 2)
            rotation = new Vector3(0, 90, 0);

        if (grid[newX, newY].neighboursWithObjects <= 0)
        {
            SpawnObjectOnGridCell(newX, newY, toSpawn, new Vector3(1.0f, 1.0f, 1.0f), 1, rotation, 0);

            if (onTop != null)
                SpawnObjectOnGridCell(newX, newY, onTop, new Vector3(1.0f, 1.0f, 1.0f), 1, rotation, yOffset);
        }

        else
        {
            if (tries <= 25)
            {
                PlaceRandomly(toSpawn, tries++, onTop, yOffset);
            }
        }
    }

    private void GoToNextPhase(SpawningPhase oldPhase)
    {
        hasIterated = 0;
        totalAlive = 0;

        switch (oldPhase)
        {
            case SpawningPhase.BIG:
                currentSpawningPhase = SpawningPhase.MEDIUM;
                aliveThreshold = aliveThresholdMedium;
                break;
            case SpawningPhase.MEDIUM:
                currentSpawningPhase = SpawningPhase.FINISHED;
                transform.rotation = originalRotation;
                return;
        }

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                SetUpGrid(x, y);
            }
        }

        if (totalAlive < 3)
        {
            GoToNextPhase(oldPhase);
        }

        grid = FindNeighbours(grid, columns, rows);

        StartCoroutine(IterateGameOfLife(grid, columns, rows, currentSpawningPhase, 5));
    }

    private void SpawnObjectOnGridCell(int i, int j, GameObject toSpawn, Vector3 upscaleFactor, int radius, Vector3 extraRotation, float yOffset)
    {
        if (toSpawn != null)
        {
            GameObject obj = Instantiate(toSpawn, grid[i, j].transform.position + new Vector3(0, yOffset, 0), Quaternion.identity, objectsHolder.transform);
            obj.transform.rotation = toSpawn.transform.rotation;
            obj.transform.localScale = new Vector3(obj.transform.localScale.x * upscaleFactor.x, obj.transform.localScale.y * ((upscaleFactor.x + upscaleFactor.z) / 2), obj.transform.localScale.z * upscaleFactor.z) + new Vector3(0.1f * upscaleFactor.x + 0.1f, 0.0f, 0.1f * upscaleFactor.z + 0.1f);
            Vector3 rotateToWall = new Vector3();
            switch (grid[i, j].rotation)
            {
                case GridCell.RotateTowards.LEFT:
                    rotateToWall = leftWall.GetComponent<ForwardVector>().lookingVector;
                    obj.transform.eulerAngles += rotateToWall + extraRotation;
                    break;

                case GridCell.RotateTowards.RIGHT:
                    rotateToWall = rightWall.GetComponent<ForwardVector>().lookingVector;
                    obj.transform.eulerAngles += rotateToWall + extraRotation;
                    break;

                case GridCell.RotateTowards.TOP:
                    rotateToWall = topWall.GetComponent<ForwardVector>().lookingVector;
                    obj.transform.eulerAngles += rotateToWall + extraRotation;
                    break;

                case GridCell.RotateTowards.BOTTOM:
                    rotateToWall = botWall.GetComponent<ForwardVector>().lookingVector;
                    obj.transform.eulerAngles += rotateToWall + extraRotation;
                    break;
            }

            for (int x = -radius; x <= radius; x++)
            {
                if (i + x < 0 || i + x >= columns)
                {
                    continue;
                }

                for (int y = -radius; y <= radius; y++)
                {
                    if (j + y < 0 || j + y >= rows)
                    {
                        continue;
                    }

                    grid[i + x, j + y].hasSpawnedAnObject = true;
                }
            }

            grid[i, j].hasSpawnedAnObject = true;
        }

        grid = FindNeighbours(grid, columns, rows);
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
                int neighboursWithObjects = 0;

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

                        if (grid[l + i, m + j].hasSpawnedAnObject)
                            neighboursWithObjects++;
                    }
                }

                if (grid[l, m].isAlive)
                    aliveNeighbours--;

                if (grid[l, m].hasSpawnedAnObject)
                    neighboursWithObjects--;

                newGrid[l, m].aliveNeighbours = aliveNeighbours;
                newGrid[l, m].neighboursWithObjects = neighboursWithObjects;
            }
        }

        return newGrid;
    }
}