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
        platform.dockingSpotA = waves[0].leftCapsule.dockingSpot;
        platform.dockingSpotB = waves[0].rightCapsule.dockingSpot;
    }

    public void GetNextWave()
    {
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
