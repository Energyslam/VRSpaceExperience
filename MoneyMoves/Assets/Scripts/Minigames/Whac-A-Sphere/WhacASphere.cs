using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhacASphere : MonoBehaviour
{ 
    WhacASphereVariables variables;
    public List<GameObject> spheres = new List<GameObject>();
    public List<GameObject> activatedSpheres = new List<GameObject>();
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public float timeBetweenActivation = 1f;
    public WhacASphereManager manager;
    public int spawnNegativeAfterSpawns = 0;
    int spawnerCount = 0;
    public float maxSphereLifetime;
    public float totalSphereLifetime = 0f;
    public enum Side
    {
        Left,
        Right
    }
    public Side side;

    void Start()
    {
        variables = manager.variables;
        maxSphereLifetime = variables.totalTime / variables.timeBetweenActivation * variables.activeTime * 2f;
        Debug.Log("Max sphere lifetime = " + maxSphereLifetime);
        this.timeBetweenActivation = variables.timeBetweenActivation;
        this.spawnNegativeAfterSpawns = variables.spawnNegativeAfterSpawns;
        foreach(GameObject go in spheres)
        {
            go.GetComponent<Sphere>().Initialize(this, variables);
        }
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        ActivateRandomSphere();
        yield return new WaitForSeconds(timeBetweenActivation);
        StartCoroutine(GameLoop());
    }

    public void ActivateRandomSphere()
    {
        GetRandomUnactivatedSphere().GetComponent<Sphere>().ActivateASphere(Sphere.Mood.Positive);
        if (spawnNegativeAfterSpawns == 0)
        {
            return;
        }
        spawnerCount++;
        if (spawnerCount % spawnNegativeAfterSpawns == 0)
        {
            GetRandomUnactivatedSphere().GetComponent<Sphere>().ActivateASphere(Sphere.Mood.Negative);
        }
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
