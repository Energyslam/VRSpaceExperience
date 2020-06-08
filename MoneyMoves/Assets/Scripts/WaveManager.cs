using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager _instance;
    public static WaveManager Instance { get { return _instance; } }

    [SerializeField] List<Wave> waves = new List<Wave>();
    [SerializeField] Platform platform;
    [SerializeField] GameObject highscoreContainer;
    int currentWave = 0;
    int activeWaves = 0;
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
    void Start()
    {
        foreach(Transform t in transform)
        {
            if (t.GetComponent<Wave>() != null)
            waves.Add(t.GetComponent<Wave>());
        }
        platform.dockingSpotA = waves[0].leftCapsule.dockingSpot;
        platform.dockingSpotB = waves[0].rightCapsule.dockingSpot;
        foreach (Wave w in waves)
        {
            if (w.gameObject.activeInHierarchy) activeWaves++;
        }
    }

    public void GetNextWave()
    {
        if (currentWave + 1 >= activeWaves)
        {
            //TODO: display highscore met mooie animatie ?
            //Vector3 spawnSpot = GameManager.Instance.player.transform.position + (((platform.dockingSpotA != null ? platform.dockingSpotA.transform.position : platform.dockingSpotB.transform.position) - GameManager.Instance.player.transform.position) * 4f) + new Vector3(0f, 5f, 0f);
            //Vector3 highscoreTarget = GameManager.Instance.player.transform.position + (((platform.dockingSpotA != null ? platform.dockingSpotA.transform.position : platform.dockingSpotB.transform.position) - GameManager.Instance.player.transform.position) * 2f);
            Vector3 spawnspot = GameManager.Instance.player.transform.position + GameManager.Instance.platform.gameObject.transform.forward * 40f;
            Vector3 highscoreTarget = GameManager.Instance.player.transform.position + GameManager.Instance.platform.gameObject.transform.forward * 10f;
            
            GameObject highscoreObject = Instantiate(highscoreContainer, spawnspot, Quaternion.identity);
            highscoreObject.GetComponent<LookAtPlayer>().target = highscoreTarget;
            GameObject go = platform.dockingSpotA != null ? platform.dockingSpotA.transform.root.gameObject : platform.dockingSpotB.transform.root.gameObject;
            if (waves[currentWave].leftCapsule.gameObject.activeInHierarchy)
            {
                waves[currentWave].leftCapsule.ClearListsForRespawn();
            }
            else if (waves[currentWave].rightCapsule.gameObject.activeInHierarchy)
            {
                waves[currentWave].rightCapsule.ClearListsForRespawn();
            }
            go.AddComponent<Rigidbody>();
            return;
        }
        currentWave++;
        platform.dockingSpotA = waves[currentWave].leftCapsule.dockingSpot;
        platform.dockingSpotB = waves[currentWave].rightCapsule.dockingSpot;
        MovePlatformToNextSplit();
    }

    void MovePlatformToNextSplit()
    {
        platform.CreateVisuals();
        platform.ChangeStateToSplit();
    }
}
