using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPlayerRotation : MonoBehaviour
{
    void LateUpdate()
    {
        this.transform.eulerAngles = new Vector3(0, 0, 0);   
    }
}
