using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WhacASphereSpawner : MonoBehaviour
{
    public GameObject WhacASphere;
    public static WhacASphereSpawner instance;
    public WhacASphereVariables variables;
    public bool resetIterationsOnStart;
    public bool SpeedupStart;
    public bool fullTest;

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
        //variables.totalIterations = 0;
        variables.skillLevelsDone = 0;
        if (resetIterationsOnStart)
        {
            variables.totalIterations = 0;
            variables.excelOffset = 0;
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
        if (SpeedupStart)
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
