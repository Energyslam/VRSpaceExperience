using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardVector : MonoBehaviour
{
    public Vector3 lookingVector;
    public GameObject lookAtClose, lookAtFar;

    // Update is called once per frame
    void Update()
    {
        lookAtClose.transform.LookAt(lookAtFar.transform);
        lookingVector = lookAtClose.transform.eulerAngles + new Vector3(0, 180, 0);
    }
}
;