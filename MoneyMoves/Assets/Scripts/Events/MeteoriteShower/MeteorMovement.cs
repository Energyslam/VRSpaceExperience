using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MeteorMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1.0f;

    private Vector3 direction;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = Random.onUnitSphere * movementSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction;
    }
}
