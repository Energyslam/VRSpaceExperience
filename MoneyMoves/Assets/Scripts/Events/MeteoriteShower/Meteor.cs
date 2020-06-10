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
        Explode();
    }

    public void Explode()
    {
        GameObject spawned = Instantiate(brokenMeteor, transform.position, transform.rotation, transform.parent);
        spawned.transform.localScale = transform.localScale;
        GetComponentInParent<MeteoriteShower>().UpdateMeteorCount(1);
        Destroy(gameObject);
    }
}
