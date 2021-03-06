﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] GameObject explosionSound;
    [SerializeField] GameObject floatingPoints;

    private Vector3 direction, oldPosition;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding with " + other.gameObject.name);
        if (other.CompareTag("Collideable"))
        {
            other.GetComponent<GiftBehaviour>().SolveCollision();
            //GameObject floatyPoints = Instantiate(floatingPoints, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (other.CompareTag("MinigameSphere"))
        {
            other.GetComponent<Sphere>().Hit();
            Destroy(this.gameObject);
        }

        if (other.gameObject.name == "StartButton")
        {
            GameManager.Instance.StartGame();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Letter"))
        {
            other.GetComponentInParent<NameInputHandler>().AddLetter(other.name);
        }

        if (other.name == "FinishButton")
        {
            other.GetComponentInParent<HighscoreContainer>().TurnOffObjects();
            other.GetComponentInParent<HighscoreContainer>().CreateHighscores();
        }
    }
}
