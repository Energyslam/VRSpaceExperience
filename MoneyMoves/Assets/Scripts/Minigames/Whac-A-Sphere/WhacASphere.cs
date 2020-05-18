using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhacASphere : MonoBehaviour
{
    //public Material unlit, positiveLit, negativeLit;
    public List<GameObject> spheres = new List<GameObject>();
    public List<GameObject> activatedSpheres = new List<GameObject>();
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public float activeTime = 1f;
    public float timeBetweenActivation = 1f;
    public GameObject gizmo;
    public WhacASphereManager manager;
    public int negativeModulo = 0;
    int negativeSpawner = 0;

    Vector3[][] grid;
    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    void Start()
    {
        foreach(GameObject go in spheres)
        {
            go.GetComponent<Sphere>().Initialize(this, activeTime);
        }
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        ActivateRandomSphere();
        yield return new WaitForSeconds(timeBetweenActivation);
        StartCoroutine(GameLoop());
    }

    GameObject GetRandomUnactivatedSphere()
    {
        GameObject randomSphere = spheres[Random.Range(0, spheres.Count)];
        if (activatedSpheres.Contains(randomSphere))
        {
            randomSphere = GetRandomUnactivatedSphere();
        }
        return randomSphere;
    }
    public void ActivateRandomSphere()
    {
            GetRandomUnactivatedSphere().GetComponent<Sphere>().Sphereoooo(Sphere.Mood.Positive);
        if (negativeModulo == 0)
        {
            return;
        }
        negativeSpawner++;
        if (negativeSpawner % negativeModulo == 0)
        {
            GetRandomUnactivatedSphere().GetComponent<Sphere>().Sphereoooo(Sphere.Mood.Negative);
        }
    }

    public void DeactivateAll()
    {
        StopAllCoroutines();
        foreach(GameObject go in spheres)
        {
            go.GetComponent<Sphere>().DeactivateSphere();
        }
    }

    public void UpdateScore(int points)
    {
        if ((score + points) < 0) return;
        score += points;
        if (side == Side.Left)
        {
            scoreText.text = "Left: " + score;
        }
        else if (side == Side.Right)
        {
            scoreText.text = "Right: " + score;
        }
    }



}
