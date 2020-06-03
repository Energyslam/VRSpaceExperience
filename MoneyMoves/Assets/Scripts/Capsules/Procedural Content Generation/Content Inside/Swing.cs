using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField]
    float swingSpeed = 1.0f;

    public bool randomRotation = false;

    public float rotation = 60;

    public Vector3 axis;

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = -1f + Mathf.Sin(swingSpeed * Time.realtimeSinceStartup) * rotation;

        Vector3 newRotation = new Vector3();

        if (randomRotation)
        {
            if (Random.value > 0.5f)
            {
                newRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
            }
            else
            {
                newRotation = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
            }
        }
        else
        {
            newRotation = transform.localEulerAngles;

            if (axis.x != 0)
                newRotation.x = angle * axis.x;

            if (axis.y != 0)
                newRotation.y = angle * axis.y;

            if (axis.z != 0)
                newRotation.z = angle * axis.z;
        }
        
        transform.localEulerAngles = newRotation;
    }
}
