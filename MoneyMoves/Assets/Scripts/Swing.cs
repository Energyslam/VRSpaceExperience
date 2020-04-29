using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField]
    float swingSpeed = 1.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = -1f + Mathf.Sin(swingSpeed * Time.realtimeSinceStartup) * 15f;

        Vector3 newRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);

        transform.localEulerAngles = newRotation;
    }
}
