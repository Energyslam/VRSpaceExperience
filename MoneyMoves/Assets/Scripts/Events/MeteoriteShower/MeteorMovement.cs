using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MeteorMovement : MonoBehaviour
{
    [SerializeField]
    private float minMovementSpeed = 1.0f, maxMovementSpeed = 1.0f;

    public float moveSpeed = 1.0f;

    [SerializeField]
    private Vector3 directionOffset;

    private Vector3 direction;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = Random.onUnitSphere;
        direction = new Vector3(direction.x, -Mathf.Abs(direction.y), direction.z) + directionOffset;
        moveSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
        direction *= moveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction;
        rb.AddTorque(Random.onUnitSphere * Random.Range(minMovementSpeed, maxMovementSpeed));
    }
}
