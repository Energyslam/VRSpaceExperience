using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WhacASphereManager : MonoBehaviour
{
    public WhacASphereVariables variables;
    [SerializeField] public WhacASphere leftGame;
    [SerializeField] public WhacASphere rightGame;
    public WhacASphereTester tester;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] Image errorCross;

    [SerializeField] float totalTime;
    float remainingTime;
    int finalLeftScore;
    int finalRightScore;
    int totalScore = 0;

    [Header("After minigame")]
    Platform platform;
    public GameObject explosion;
    public Vector3 destrucTorque;
    public bool isTesting;

    private void Awake()
    {
        platform = GameManager.Instance.platform;
        this.totalTime = variables.totalTime;
        remainingTime = totalTime;
    }
    void Start()
    {

        timeText.text = remainingTime < 10 ? "00:0" + remainingTime : "00:" + remainingTime;
        StartCoroutine(CountdownTime());
        if (!isTesting)
        {
            this.transform.position = CalculateGamePosition();
            transform.LookAt(Camera.main.transform);
            transform.eulerAngles -= new Vector3(transform.localEulerAngles.x, 90, 0);
        }

    }


    IEnumerator CountdownTime()
    {
        yield return new WaitForSeconds(1f);

        remainingTime--;
        timeText.text = remainingTime < 10 ? "00:0" + remainingTime : "00:" + remainingTime;
        if (remainingTime > 0)
        {
            StartCoroutine(CountdownTime());
        }
        else if (remainingTime <= 0)
        {
            StartCalculatingFinalScore();
        }
    }

    void AdjustDifficulty()
    {
        variables.testerSpeed += variables.skillGrowth * Mathf.Clamp(1f - (variables.iteration * 2f) / variables.iterationsToTest, 0f, 1f); //simulates player skill growing over time
        Debug.Log("Adding " + Mathf.Clamp((variables.skillGrowth * 1f - (variables.iteration * 2f) / variables.iterationsToTest), 0f, 1f) + " to testerspeed");

        int finalScore = finalLeftScore + finalRightScore;
        float maximumPossibleScore = (totalTime / variables.timeBetweenActivation) * 10 * 2f;
        float desiredPoints = maximumPossibleScore / 2f; //We want the player to obtain half of the obtainable points
        float absoluteDifference = Mathf.Abs(finalScore - desiredPoints);
        float tenPercent = maximumPossibleScore / 10f;
        float multiplier = Mathf.Clamp((absoluteDifference / tenPercent), 1f, Mathf.Infinity); //prevents multiplier from scaling something that's already being scaled down
        float scalar = absoluteDifference / desiredPoints / 10f;
        Debug.Log(
            "\nIteration = " + variables.iteration +
            "\nMaximum Possible Score = " + maximumPossibleScore +
            "\nDesired points  = " + desiredPoints +
            "\nFinal Score = " + finalScore
            );
        if (finalScore > desiredPoints)
        {
            if (WhacASphereSpawner.instance.justScalar)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f - scalar), 0.0000001f, variables.totalTime);
            }
            else if (WhacASphereSpawner.instance.scalarNmultiplier)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f - scalar * (multiplier)), 0.0000001f, variables.totalTime);
            }
            else if (WhacASphereSpawner.instance.scalarMultiplierNOffset)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f - scalar * (multiplier + 1f)), 0.0000001f, variables.totalTime);
            }
        }
        else if (finalScore < desiredPoints)
        {
            if (WhacASphereSpawner.instance.justScalar)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f + scalar), 0.0000001f, variables.totalTime);
            }
            else if (WhacASphereSpawner.instance.scalarNmultiplier)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f + scalar * (multiplier)), 0.0000001f, variables.totalTime);
            }
            else if (WhacASphereSpawner.instance.scalarMultiplierNOffset)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f + scalar * (multiplier + 1f)), 0.0000001f, variables.totalTime);
            }
            else if (WhacASphereSpawner.instance.noScaling)
            {

            }
        }
        float combinedMaxLifeTime = leftGame.maxSphereLifetime + rightGame.maxSphereLifetime;
        float combinedTotalLifetime = leftGame.totalSphereLifetime + rightGame.totalSphereLifetime;
        variables.iteration++;
        variables.totalIterations++;
        string average = "";
        if (isTesting)
        {
            if (variables.iteration == 1)
            {
                MyTools.DEV_AppendHeadersToReport();
                average += finalScore.ToString();
            }
            else
            {
                average += "=AVERAGE($B$" + (variables.totalIterations + variables.skillLevelsDone) + ":$B$" + (variables.totalIterations + 1 + variables.skillLevelsDone) + ")";
            }

            MyTools.DEV_AppendSpecificsToReport(new string[5] { variables.iteration.ToString(), finalScore.ToString(), average, (variables.activeTime * variables.speedUpDivider).ToString("n3"), variables.playerSkill.ToString() });
            if (variables.iteration >= variables.iterationsToTest)
            {
                if (WhacASphereSpawner.instance.fullTest)
                {
                    if (variables.playerSkill != WhacASphereVariables.PlayerSkill.Expert)
                    {
                        variables.iteration = 0;
                        variables.playerSkill += 1;
                        variables.skillLevelsDone += 1;
                        WhacASphereSpawner.instance.SetVariables();
                    }
                    else
                    {
                        UnityEditor.EditorApplication.isPlaying = false;
                    }
                }
                else
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }

            }
        }
    }

    void StartCalculatingFinalScore()
    {
        leftGame.DeactivateAll();
        rightGame.DeactivateAll();
        finalLeftScore = leftGame.score;
        finalRightScore = rightGame.score;
        AdjustDifficulty();
        totalScoreText.gameObject.SetActive(true);
        StartCoroutine(CalculateFinalScore());
    }
    IEnumerator CalculateFinalScore()
    {
        if (isTesting)
        {
            yield return new WaitForSeconds(0.001f);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }

        DisplayErrorCross();

        totalScoreText.text = "Total score = " + totalScore;

        if (leftGame.score - 10 < 0 && rightGame.score - 10 < 0)
        {
            if (isTesting)
            {
                yield return new WaitForSeconds(0.001f);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
            MoveAfterEnding();
        }
        else
        {
            StartCoroutine(CalculateFinalScore());
        }
    }

    void DisplayErrorCross()
    {
        if (leftGame.score - 10 >= 0)
        {
            leftGame.UpdateScore(-10);
            totalScore += 10;
        }
        else if (leftGame.score - 10 < 0)
        {
            if (!errorCross.gameObject.activeInHierarchy)
            {
                errorCross.gameObject.SetActive(true);
                errorCross.transform.localPosition = new Vector3(-3f, errorCross.transform.localPosition.y, errorCross.transform.localPosition.z);
            }
        }
        if (rightGame.score - 10 >= 0)
        {
            rightGame.UpdateScore(-10);
            totalScore += 10;

        }
        else if (rightGame.score - 10 < 0)
        {
            if (!errorCross.gameObject.activeInHierarchy)
            {
                errorCross.gameObject.SetActive(true);
                errorCross.transform.localPosition = new Vector3(3f, errorCross.transform.localPosition.y, errorCross.transform.localPosition.z);
            }
        }
    }
    public void MoveAfterEnding()
    {
        if (isTesting)
        {
            WhacASphereSpawner.instance.CreateNewWhacASphere();
            Destroy(this.gameObject);
            return;
        }
        if (finalLeftScore > finalRightScore)
        {
            MoveToA();
        }
        else if (finalRightScore > finalLeftScore)
        {
            MoveToB();
        }
        else if (finalLeftScore == finalRightScore)
        {
            //TODO: do something else than defaulting to left
            MoveToA();
        }
        Destroy(this.gameObject);
    }

    void MoveToA()
    {
        platform.dockingSpotB.transform.parent.gameObject.AddComponent<Rigidbody>();
        platform.ClearBTrack();
        Destroy(platform.dockingSpotB.transform.parent.gameObject, 10f);
        platform.dockingSpotB.transform.parent.gameObject.GetComponent<Rigidbody>().AddTorque(destrucTorque);
        GameObject explo = Instantiate(explosion, platform.dockingSpotB.transform.parent.gameObject.transform.position, Quaternion.identity);
        Destroy(explo, 2f);
        platform.ChangeStateToA();
    }

    void MoveToB()
    {
        platform.dockingSpotA.transform.parent.gameObject.AddComponent<Rigidbody>();
        platform.ClearATrack();
        Destroy(platform.dockingSpotA.transform.parent.gameObject, 10f);
        platform.dockingSpotA.transform.parent.gameObject.GetComponent<Rigidbody>().AddTorque(destrucTorque);
        GameObject explo = Instantiate(explosion, platform.dockingSpotA.transform.parent.gameObject.transform.position, Quaternion.identity);
        Destroy(explo, 2f);
        platform.ChangeStateToB();
    }

    Vector3 CalculateGamePosition()
    {
        //TODO: playtest position, track can be rendered behind gamescreen at cost of performance. Should not have any noticeable impact during minigame-gameplay
        Vector3 trackLeftQuarter = Tracks.SplitToA[Tracks.SplitToA.Count / 4];
        Vector3 trackRightQuarter = Tracks.SplitToB[Tracks.SplitToB.Count / 4];
        Vector3 vectorFromLeftToRightQuarter = trackRightQuarter - trackLeftQuarter;
        Vector3 endPosition = trackLeftQuarter + vectorFromLeftToRightQuarter / 2f;
        endPosition.y = GameManager.Instance.player.transform.position.y + 2f;
        return endPosition;
    }
}
