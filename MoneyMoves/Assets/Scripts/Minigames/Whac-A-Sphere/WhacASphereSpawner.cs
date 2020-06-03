using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WhacASphereSpawner : MonoBehaviour
{
    public static WhacASphereSpawner instance;
    public GameObject WhacASphere;
    public WhacASphereVariables variables;

    [Header("Test settings")]
    public bool resetIterationsOnStart;
    public bool speedTest;
    public bool fullTest;

    [Header("Dynamic Difficulty Adjustment")]
    public bool justScalar;
    public bool scalarNmultiplier;
    public bool scalarMultiplierNOffset;
    public bool noScaling;

    public bool testHunnit;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        variables.iteration = 0;
        variables.skillLevelsDone = 0;
        if (resetIterationsOnStart)
        {
            variables.totalIterations = 0;
            variables.excelOffset = -1;
        }
        if (fullTest)
        {
            variables.playerSkill = WhacASphereVariables.PlayerSkill.Awful;
        }
    }

    private void Start()
    {
        SetVariables();
        GameObject go = Instantiate(WhacASphere);

    }
    public void SetVariables()
    {
        if (speedTest)
        {
            variables.SpeedUp();
        }
        else
        {
            variables.SetHumanDefaults();
        }
    }
    public void CreateNewWhacASphere()
    {
        GameObject go = Instantiate(WhacASphere);
    }
}
