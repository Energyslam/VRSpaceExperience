using System.Collections;
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
            other.GetComponent<GiftBehaviour>().SolveCollision();
            GameObject explosion = Instantiate(explosionSound, this.transform.position, Quaternion.identity);
            Destroy(explosion, explosion.GetComponent<AudioSource>().clip.length);
            Destroy(this.gameObject);
        }

        if (other.CompareTag("MinigameTarget"))
        {
            other.GetComponent<MinigameTarget>().Hit();
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
