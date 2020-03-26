using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCapsule : MonoBehaviour
{
    [SerializeField]
    GameObject capsulePrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject capsule = Instantiate(capsulePrefab, this.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
