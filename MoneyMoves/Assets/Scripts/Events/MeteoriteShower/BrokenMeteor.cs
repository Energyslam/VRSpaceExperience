using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BrokenMeteor : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> rigidbodies;

    [SerializeField]
    private float explosionStrength, explosionRadius, rotationSpeed;

    private void Awake()
    {
        for (int i = 0; i < rigidbodies.Count; i++)
        {
            rigidbodies[i].AddExplosionForce(explosionStrength, transform.position, explosionRadius);
            rigidbodies[i].AddTorque(Random.onUnitSphere * rotationSpeed);
        }

        Destroy(gameObject, 2.0f);
    }
}
