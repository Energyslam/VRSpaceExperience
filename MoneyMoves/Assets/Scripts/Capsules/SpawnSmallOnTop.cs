using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSmallOnTop : MonoBehaviour
{
    [SerializeField]
    private int amountToSpawn = 1;

    [SerializeField]
    private List<GameObject> objectsToSpawn;

    [SerializeField]
    private Vector3 edge1, edge2;

    // Start is called before the first frame update
    void Start()
    {
        int toSpawn = Random.Range(0, amountToSpawn + 1);

        for (int i = 0; i < toSpawn; i++)
        {
            int rand = Random.Range(0, objectsToSpawn.Count);
            float xPos = Random.Range(edge1.x, edge2.x);
            float zPos = Random.Range(edge1.z, edge2.z);

            Vector3 randomPositions = new Vector3(xPos, 0, zPos);
            Vector3 position = transform.position + randomPositions;

            GameObject smallObj = Instantiate(objectsToSpawn[rand], position, Quaternion.identity);
            smallObj.transform.parent = transform;
        }
    }
}
