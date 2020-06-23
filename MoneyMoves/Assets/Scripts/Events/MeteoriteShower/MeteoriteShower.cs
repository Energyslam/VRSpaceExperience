﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteShower : SpawnEvent
{
    [SerializeField]
    private Transform objectHolder;

    [SerializeField]
    private float spawnRate = 1.0f, ySpawnOffset = 1.0f;

    [SerializeField]
    [Range(1, 1000)]
    private int currentAmountofMeteorites = 0, maxAmountofMeteorites = 175;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public Pool pool;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < pool.size; i++)
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.transform.parent = objectHolder;
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary.Add(pool.tag, objectPool);

        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        if (currentAmountofMeteorites < maxAmountofMeteorites)
        {
            // Determine object details
            Vector3 position = area.RandomInsideArea();
            position = new Vector3(position.x, Random.Range(area.maxY - ySpawnOffset, area.maxY), position.z);

            GameObject spawnedMeteor = SpawnFromPool("Meteor", position, Quaternion.identity);
        }

        // Recursively call spawning method
        yield return new WaitForSeconds(1 / spawnRate);
        StartCoroutine(SpawnObjects());
    }

    public override void UpdateSpawnAmount(int addition)
    {
        currentAmountofMeteorites += addition;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with the tag " + tag + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        UpdateSpawnAmount(1);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public override Area GetArea()
    {
        return area;
    }
}
