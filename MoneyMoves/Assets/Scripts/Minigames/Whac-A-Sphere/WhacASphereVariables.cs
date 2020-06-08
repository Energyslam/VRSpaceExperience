using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[CreateAssetMenu(fileName = "WhacASphere Variables", menuName = "WhacASphere/Variables", order = 1)]
public class WhacASphereVariables : ScriptableObject
{
    public float activeTime;
    public float negativeActiveTime;
    public float timeBetweenActivation;
    public float totalTime;
    public float testerSpeed;
    public float speedUpDivider;
    public float reactionTime;
    public float skillGrowth;
    public float shootingDelay;

    public int spawnNegativeAfterSpawns;
    public int iteration;
    public int totalIterations;
    public int excelOffset;
    public int skillLevelsDone;
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
        SetTesterSpeed();
        activeTime = 4;
        timeBetweenActivation = 2f;
        totalTime = 5f;
        reactionTime = 0.3f;
        shootingDelay = 0.8f;
        skillGrowth = testerSpeed / 20f;
    }

    void SetTesterSpeed()
    {
        if (playerSkill == PlayerSkill.Awful)
        {
            testerSpeed = 0.5f;
        }
        else if (playerSkill == PlayerSkill.Low)
        {
            testerSpeed = 1f;
        }
        else if (playerSkill == PlayerSkill.Average)
        {
            testerSpeed = 2f;
        }
        else if (playerSkill == PlayerSkill.High)
        {
            testerSpeed = 4f;
        }
        else if (playerSkill == PlayerSkill.Expert)
        {
            testerSpeed = 8f;
        }
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
        shootingDelay /= speedUpDivider;
    }
}
