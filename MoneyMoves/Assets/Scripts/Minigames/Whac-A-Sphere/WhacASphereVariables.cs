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
    public int totalIterations;
    public int skillLevelsDone;
    public float speedUpDivider;
    public float reactionTime;
    public float skillGrowth;
    public int iterationsToTest = 20;

    public enum PlayerSkill
    {
        Awful,
        Low,
        Average,
        High,
        Expert
    }
    public PlayerSkill playerSkill;

    public void SetHumanDefaults()
    {
        activeTime = 4;
        timeBetweenActivation = 2;
        totalTime = 20;
        reactionTime = 0.3f;
        if (playerSkill == PlayerSkill.Awful)
        {
            testerSpeed = 0.5f;
        }
        else if (playerSkill == PlayerSkill.Low)
        {
            testerSpeed = 1.5f;
        }
        else if (playerSkill == PlayerSkill.Average)
        {
            testerSpeed = 2.5f;
        }
        else if (playerSkill == PlayerSkill.High)
        {
            testerSpeed = 4f;
        }
        else if (playerSkill == PlayerSkill.Expert)
        {
            testerSpeed = 6f;
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
