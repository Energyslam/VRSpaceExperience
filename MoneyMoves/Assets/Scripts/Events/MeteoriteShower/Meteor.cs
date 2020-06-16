using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Meteor : MonoBehaviour
{
    [SerializeField]
    private GameObject brokenMeteor;

    private int value = 1;

    [SerializeField]
    private float pointMultiplier = 1.0f;

    private void Start()
    {
        float scaleMultiplier = GetComponent<RandomScaleOnStartup>().maxScale.x / transform.localScale.x;

        value = Mathf.RoundToInt(pointMultiplier * scaleMultiplier * GetComponent<MeteorMovement>().moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ProjectileBehaviour>() != null)
        {
            GameManager.Instance.AddScore(value);
        }

        Explode();
    }

    public void Explode()
    {
        GameObject spawned = Instantiate(brokenMeteor, transform.position, transform.rotation, transform.parent);
        spawned.transform.localScale = transform.localScale;
        GetComponentInParent<MeteoriteShower>().UpdateMeteorCount(-1);
        gameObject.SetActive(false);
    }
}
