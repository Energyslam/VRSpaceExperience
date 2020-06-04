using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSmallOnTop : MonoBehaviour
{
    [SerializeField]
    private int maxAmountToSpawn = 1;

    [SerializeField]
    private List<GameObject> objectsToSpawn;

    [SerializeField]
    private GameObject edge1GO, edge2GO;

    [SerializeField]
    private Vector3 edge1, edge2;

    private GameObject spawnedObj;

    [SerializeField]
    private bool randomRotation = true;

    // Start is called before the first frame update
    void Start()
    {
        int toSpawn = Random.Range(0, maxAmountToSpawn + 1);

        for (int i = 0; i < toSpawn; i++)
        {
            Vector3 positionLeft = edge1GO == null ? edge1 : edge1GO.transform.localPosition;
            Vector3 positionRight = edge2GO == null ? edge2 : edge2GO.transform.localPosition;

            int rand = Random.Range(0, objectsToSpawn.Count);
            float xPos = Random.Range(positionLeft.x, positionRight.x);
            float zPos = Random.Range(positionLeft.z, positionRight.z);

            Vector3 randomPositions = new Vector3(xPos, 1, zPos);
            Vector3 position = transform.position + randomPositions;

            spawnedObj = Instantiate(objectsToSpawn[rand], position, Quaternion.identity);

            if (randomRotation)
                spawnedObj.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            else
                spawnedObj.transform.rotation = objectsToSpawn[rand].transform.rotation;

            spawnedObj.transform.localScale = objectsToSpawn[rand].transform.localScale;

            spawnedObj.transform.parent = transform;

            if (transform.GetComponentInParent<GridManager>().transform.rotation.x == 0 && transform.GetComponentInParent<GridManager>().transform.rotation.z == 0)
            {
                MakeStatic();
                Invoke("Fall", 3);
            }
        }
    }

    private void Fall()
    {
        spawnedObj.GetComponent<Rigidbody>().isKinematic = false;
        Invoke("MakeStatic", 3);
    }

    private void MakeStatic()
    {
        spawnedObj.GetComponent<Rigidbody>().isKinematic = true;
    }
}
