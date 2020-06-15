using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Make_copy : MonoBehaviour
{
    public GameObject gameObject;

    // Start is called before the first frame update
    void Start()
    {
        GameObject duplicate = Instantiate(gameObject, this.transform);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
