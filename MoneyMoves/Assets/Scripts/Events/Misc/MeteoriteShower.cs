using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteShower : Event
{
    [Header("Setup objects")]
    public Area area;

    [SerializeField]
    private Transform objectHolder;

    [Header("Spawning information")]
    [SerializeField]
    private List<GameObject> meteorites;

    [SerializeField]
    private float spawnRate = 1.0f, ySpawnOffset = 1.0f;

    [SerializeField]
    [Range(1, 1000)]
    private int maxAmountOfMeteorites = 250, currentAmountofMeteorites = 0;

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        if (currentAmountofMeteorites < maxAmountOfMeteorites)
        {
            // Determine object details
            Vector3 position = area.RandomInsideArea();
            position = new Vector3(position.x, Random.Range(area.maxY - ySpawnOffset, area.maxY), position.z);

            GameObject meteorModel = meteorites[Random.Range(0, meteorites.Count)];

            GameObject spawnedMeteor = Instantiate(meteorModel, position, Quaternion.identity, objectHolder);
            currentAmountofMeteorites++;
        }
        // Recursively call spawning method
        yield return new WaitForSeconds(1 / spawnRate);
        StartCoroutine(SpawnObjects());
    }

    public void UpdateMeteorCount(int addition)
    {
        currentAmountofMeteorites += addition;
    }
}
