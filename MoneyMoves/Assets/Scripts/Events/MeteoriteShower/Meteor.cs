using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Meteor : MonoBehaviour
{
    [SerializeField]
    private GameObject brokenMeteor;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject spawned = Instantiate(brokenMeteor, transform.position, transform.rotation, transform.parent);
        spawned.transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
}
