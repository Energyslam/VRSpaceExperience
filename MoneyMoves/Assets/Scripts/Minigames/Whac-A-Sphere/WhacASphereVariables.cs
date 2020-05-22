using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[CreateAssetMenu(fileName = "WhacASphere Variables", menuName = "WhacASphere", order = 1)]
public class WhacASphereVariables : ScriptableObject
{
    public float activeTime;
    public float negativeActiveTime;
    public float timeBetweenActivation;
    public float totalTime;
    public int spawnNegativeAfterSpawns;
    public float testerSpeed;
    public int iteration;
    public float speedUpDivider;
    public float reactionTime;
    public float skillGrowth;

    public enum PlayerSkill
    {
        Low,
        Average,
        High,
        GG
    }
    public PlayerSkill playerSkill;

    public void SetHumanDefaults()
    {
        activeTime = 4;
        timeBetweenActivation = 1;
        totalTime = 20;
        reactionTime = 0.3f;
        if (playerSkill == PlayerSkill.Low)
        {
            testerSpeed = 5;
        }
        else if (playerSkill == PlayerSkill.Average)
        {
            testerSpeed = 8;
        }
        else if (playerSkill == PlayerSkill.High)
        {
            testerSpeed = 10;
        }
        else if (playerSkill == PlayerSkill.GG)
        {
            testerSpeed = 15;
        }
        skillGrowth = testerSpeed / 10f;
    }

    public void SpeedUp()
    {
        SetHumanDefaults();
        activeTime /= speedUpDivider;
        timeBetweenActivation /= speedUpDivider;
        totalTime /= speedUpDivider;
        testerSpeed *= speedUpDivider;
        reactionTime /= speedUpDivider;
        skillGrowth *= speedUpDivider;
    }
}
