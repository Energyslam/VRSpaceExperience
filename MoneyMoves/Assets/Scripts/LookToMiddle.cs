using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToMiddle : MonoBehaviour
{
    [SerializeField]
    private Transform middle;

    public bool straightAngle = true;

    // Start is called before the first frame update
    void Start()
    {
        transform.forward = middle.position - transform.position;
    }
}
