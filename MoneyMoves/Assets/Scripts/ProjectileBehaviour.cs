﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] GameObject explosionSound;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding with " + other.gameObject.name);
        if (other.CompareTag("Collideable"))
        {
            other.GetComponent<ICollisionBehaviour>().SolveCollision();
            GameObject explosion = Instantiate(explosionSound, this.transform.position, Quaternion.identity);
            Destroy(explosion, explosion.GetComponent<AudioSource>().clip.length);
            Destroy(this.gameObject);
        }
        if (other.name == "arrowA")
        {
            GameManager.Instance.platform.RemoveArrowColliders();
            GameManager.Instance.platform.ChangeStateToA();
            Destroy(this.gameObject);
        }
        if (other.name == "arrowB")
        {
            GameManager.Instance.platform.RemoveArrowColliders();
            GameManager.Instance.platform.ChangeStateToB();
            Destroy(this.gameObject);
        }

    }
}
