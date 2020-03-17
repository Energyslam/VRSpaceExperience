using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager Instance { get { return _instance; } }

    public List<Transform> spawners = new List<Transform>();
    public int monstersPerWave, totalSmallMonsters;
    public float delay;
    [SerializeField] GameObject smallMonsterPrefab, smallMonstersParent, spawnPoints;
    public List<GameObject> smallMonsterPool = new List<GameObject>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void SpawnSmallEnemy(Vector3 pos)
    {
        foreach(GameObject go in smallMonsterPool)
        {
            if (!go.activeInHierarchy)
            {
                go.SetActive(true);
                //go.GetComponent<MonsterMovement>().SetPosition(pos);
                return;
            }
        }
    }

    void Start()
    {
        for(int i = 0; i < totalSmallMonsters; i++)
        {
            smallMonsterPool.Add(Instantiate(smallMonsterPrefab, new Vector3(2500, 0, 0), Quaternion.identity, smallMonstersParent.transform));
            smallMonsterPool[i].SetActive(false);
        }
        foreach (Transform t in spawnPoints.transform)
        {
            spawners.Add(t);
        }

        if (monstersPerWave >= spawners.Count) monstersPerWave = spawners.Count;
        StartSpawning();
    }

    public void StartSpawning()
    {
        StartCoroutine(ContinuousSpawns());
    }

    public void StopSpawning()
    {
        StopAllCoroutines();
    }

    IEnumerator ContinuousSpawns()
    {
        RandomSpawn();
        yield return new WaitForSeconds(delay);
        StartCoroutine(ContinuousSpawns());
    }
    void RandomSpawn()
    {
        List<int> randomNumbers = new List<int>();
        for (int i = 0; i < monstersPerWave; i ++)
        {
            repickNumber:
            int j = Random.Range(0, spawners.Count);
            if (randomNumbers.Contains(j))
            {
                goto repickNumber;
            }
            randomNumbers.Add(j);
        }
        foreach(int random in randomNumbers)
        {
            SpawnSmallEnemy(spawners[random].position);
        }
    }
}
