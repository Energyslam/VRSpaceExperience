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
        if (resetIterationsOnStart)
        {
            variables.totalIterations = 0;
            variables.excelOffset = -1;
            variables.FrankCurrent = 0;
        }
        if (fullTest)
        {
            variables.playerSkill = WhacASphereVariables.PlayerSkill.Awful;
            noScaling = true;
            justScalar = false;
            scalarNmultiplier = false;
            scalarMultiplierNOffset = false;
        }
    }

    public void SwitchMultiplier()
    {
        if (noScaling)
        {
            noScaling = false;
            justScalar = true;
            return;
        }
        else if (justScalar)
        {
            justScalar = false;
            scalarNmultiplier = true;
            return;
        }
        else if (scalarNmultiplier)
        {
            scalarNmultiplier = false;
            scalarMultiplierNOffset = true;
            return;

        }
        else if (scalarMultiplierNOffset)
        {
            scalarMultiplierNOffset = false;
            //TODO: is done here
            Application.Quit();
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
