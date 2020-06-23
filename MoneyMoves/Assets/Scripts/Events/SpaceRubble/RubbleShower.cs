using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleShower : SpawnEvent
{
    [SerializeField]
    private Transform objectHolder;

    [SerializeField]
    private float spawnRate = 1.0f, ySpawnOffset = 1.0f;

    [SerializeField]
    [Range(1, 1000)]
    private int currentAmountofRubble = 0, maxAmountofRubble = 175;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public List<GameObject> prefabs;
        public int size;
    }

    public Pool pool;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < pool.size * pool.prefabs.Count; i++)
        {
            GameObject obj = Instantiate(pool.prefabs[Random.Range(0, pool.prefabs.Count)]);
            obj.transform.parent = objectHolder;
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        poolDictionary.Add(pool.tag, objectPool);

        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        if (currentAmountofRubble < maxAmountofRubble)
        {
            // Determine object details
            Vector3 position = area.RandomInsideArea();
            position = new Vector3(position.x, Random.Range(area.maxY - ySpawnOffset, area.maxY), position.z);

            GameObject spawnedRubble = SpawnFromPool("Rubble", position, Quaternion.identity);
        }

        // Recursively call spawning method
        yield return new WaitForSeconds(1 / spawnRate);
        StartCoroutine(SpawnObjects());
    }

    public override void UpdateSpawnAmount(int addition)
    {
        currentAmountofRubble += addition;
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
