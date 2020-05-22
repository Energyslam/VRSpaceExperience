using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class WhacASphereSpawner : MonoBehaviour
{
    public GameObject WhacASphere;
    public static WhacASphereSpawner instance;
    public WhacASphereVariables variables;
    public bool ResetVariables;
    public bool SpeedUpVariables;
    // Start is called before the first frame update
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
    }

    private void Start()
    {
        variables.iteration = 0;
    }
    void Update()
    {
        if (ResetVariables)
        {
            variables.SetHumanDefaults();
            ResetVariables = false;
        }
        if (SpeedUpVariables)
        {
            variables.SpeedUp();
            SpeedUpVariables = false;
        }
    }
    private void SetDefaultVariablesOnStart()
    {
    float activeTime = 4f;
    float negativeActiveTime = 60f;
    float timeBetweenActivation = 1f;
    float totalTime = 15f;
    int spawnNegativeAfterSpawn = 0; ;
}
    public void CreateNewWhacASphere()
    {
        GameObject go = Instantiate(WhacASphere);
    }
}
