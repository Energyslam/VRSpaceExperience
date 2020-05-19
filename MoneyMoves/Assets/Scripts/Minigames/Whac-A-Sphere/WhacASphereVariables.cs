using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WhacASphere Variables", menuName = "WhacASphere", order = 1)]
public class WhacASphereVariables : ScriptableObject
{
    public float activeTime;
    public float negativeActiveTime;
    public float timeBetweenActivation;
    public int totalTime;
    public int spawnNegativeAfterSpawns;
}
