using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhacASphereSpawner : MonoBehaviour
{
    public GameObject WhacASphere;
    public static WhacASphereSpawner instance;
    WhacASphereVariables variables;
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
