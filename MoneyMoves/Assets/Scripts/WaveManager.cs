using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager _instance;
    public static WaveManager Instance { get { return _instance; } }

    [SerializeField] List<Wave> waves = new List<Wave>();
    [SerializeField] Platform platform;
    int currentWave = 0;

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
        platform.dockingSpotA = waves[0].a.dockingSpot;
        platform.dockingSpotB = waves[0].b.dockingSpot;
    }

    public void GetNextWave()
    {
        currentWave++;
        platform.dockingSpotA = waves[currentWave].a.dockingSpot;
        platform.dockingSpotB = waves[currentWave].b.dockingSpot;
        MovePlatformToNextSplit();
    }

    void MovePlatformToNextSplit()
    {
        platform.CreateVisuals();
        platform.ChangeStateToSplit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetNextWave();
        }
    }
}
