using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Scripting.Pipeline;

public class BeatSaberLite : MonoBehaviour
{
    public GameObject beginning;
    public GameObject end;
    public GameObject gizmo;
    public GameObject leftCube;
    public GameObject rightCube;
    public float speed;
    public float distanceBetweenPoints;
    public float distanceDivision;
    public List<GameObject> cubes = new List<GameObject>();
    public GameObject[] spawnPositions = new GameObject[4];
    public GameObject[] endPosititions = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {


        //Math.abs for future prrof
        spawnPositions[0] = Instantiate(gizmo, beginning.transform.position + new Vector3(distanceBetweenPoints * -2f, 0f, 0f), Quaternion.identity, beginning.transform);
        spawnPositions[1] = Instantiate(gizmo, beginning.transform.position + new Vector3(distanceBetweenPoints * -1f, 0f, 0f), Quaternion.identity, beginning.transform);
        spawnPositions[2] = Instantiate(gizmo, beginning.transform.position + new Vector3(distanceBetweenPoints * 1f, 0f, 0f), Quaternion.identity, beginning.transform);
        spawnPositions[3] = Instantiate(gizmo, beginning.transform.position + new Vector3(distanceBetweenPoints * 2f, 0f, 0f), Quaternion.identity, beginning.transform);

        endPosititions[0] = Instantiate(gizmo, end.transform.position + new Vector3(distanceBetweenPoints / distanceDivision * 2f, 0f, 0f), Quaternion.identity, end.transform);
        endPosititions[1] = Instantiate(gizmo, end.transform.position + new Vector3(distanceBetweenPoints / distanceDivision * 1f, 0f, 0f), Quaternion.identity, end.transform);
        endPosititions[2] = Instantiate(gizmo, end.transform.position + new Vector3(distanceBetweenPoints / distanceDivision * -1f, 0f, 0f), Quaternion.identity, end.transform);
        endPosititions[3] = Instantiate(gizmo, end.transform.position + new Vector3(distanceBetweenPoints / distanceDivision * -2f, 0f, 0f), Quaternion.identity, end.transform);

        beginning.transform.LookAt(end.transform.position);
        end.transform.LookAt(beginning.transform.position);
        for (int i = 0; i < 4; i++)
        {
            //Instantiate(gizmo, spawnPositions[i].transform.position, Quaternion.identity);
            //Instantiate(gizmo, endPosititions[i], Quaternion.identity);
        }
        InvokeRepeating("SpawnCube", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    GameObject PickRandomCube()
    {
        int i = Random.Range(1, 3);
        if (i % 2 == 0)
        {
            return leftCube;
        }
        else
        {
            return rightCube;
        }
    }

    void SpawnCube()
    {
        int randomPosition = Random.Range(0, 4);

        GameObject cube = Instantiate(PickRandomCube(), beginning.transform);
        cube.transform.position = spawnPositions[randomPosition].transform.position;
        cube.transform.LookAt(endPosititions[randomPosition].transform.position);
        cube.GetComponent<BeatSaberCube>().speed = speed;
        cube.GetComponent<BeatSaberCube>().target = endPosititions[randomPosition].transform.position;
        cubes.Add(cube);
    }

    public void RemoveCubeFromList(GameObject cube)
    {
        cubes.Remove(cube);
    }
}
