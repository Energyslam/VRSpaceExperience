using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhacASphereSpawner : MonoBehaviour
{
    public GameObject WhacASphere;
    public static WhacASphereSpawner instance;
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
    public void CreateNewWhacASphere()
    {
        GameObject go = Instantiate(WhacASphere);
    }
}
