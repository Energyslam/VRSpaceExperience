using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WhacASphereSpawner : MonoBehaviour
{
    public GameObject WhacASphere;
    public static WhacASphereSpawner instance;
    public WhacASphereVariables variables;
    //public bool ResetVariables;
    //public bool SpeedUpVariables;

    public bool SpeedupStart;
    public bool DefaultStart;
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
        if (SpeedupStart)
        {
            variables.SpeedUp();
        }
        else if (DefaultStart)
        {
            variables.SetHumanDefaults();
        }
        variables.iteration = 0;
    }

    private void Start()
    {     
        GameObject go = Instantiate(WhacASphere);
    }
    void Update()
    {
        //if (ResetVariables)
        //{
        //    variables.SetHumanDefaults();
        //    ResetVariables = false;
        //}
        //if (SpeedUpVariables)
        //{
        //    variables.SpeedUp();
        //    SpeedUpVariables = false;
        //}
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
