using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BrokenMeteor : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> rigidbodies;

    [SerializeField]
    private float explosionStrength, explosionStrengthOffset, explosionRadius, rotationSpeed;

    private void Awake()
    {
        for (int i = 0; i < rigidbodies.Count; i++)
        {
            float offset = Random.Range(-explosionStrengthOffset, explosionStrengthOffset);
            rigidbodies[i].AddExplosionForce(explosionStrength + offset, transform.position, explosionRadius);
            rigidbodies[i].AddTorque(Random.onUnitSphere * rotationSpeed);
        }

        Destroy(gameObject, 2.0f);
    }
}
