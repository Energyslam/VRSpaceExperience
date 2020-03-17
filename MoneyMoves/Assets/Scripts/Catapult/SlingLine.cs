using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SlingLine : MonoBehaviour
{
    [SerializeField] GameObject pullPoint;
    LineRenderer rend;
    void Start()
    {
        rend = this.GetComponent<LineRenderer>();
    }

    void Update()
    {
        rend.SetPosition(0, this.transform.position);
        rend.SetPosition(1, pullPoint.transform.position);     
    }
}
