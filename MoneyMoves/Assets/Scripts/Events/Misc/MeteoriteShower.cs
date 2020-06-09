using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteShower : Event
{
    [Header("Setup objects")]
    [SerializeField]
    private Area area;

    [SerializeField]
    private Transform objectHolder;

    [Header("Spawning information")]
    [SerializeField]
    private List<GameObject> meteorites;

    [SerializeField]
    private float spawnRate = 1.0f;

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        // Determine object details
        Vector3 position = area.RandomInsideArea();
        GameObject meteorModel = meteorites[Random.Range(0, meteorites.Count)];

        GameObject spawnedMeteor = Instantiate(meteorModel, position, Quaternion.identity, objectHolder);

        // Recursively call spawning method
        yield return new WaitForSeconds(1 / spawnRate);
        StartCoroutine(SpawnObjects());
    }
}
