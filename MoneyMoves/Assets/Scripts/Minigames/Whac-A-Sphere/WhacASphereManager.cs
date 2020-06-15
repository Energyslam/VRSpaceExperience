using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using System.Threading;

public class WhacASphereManager : MonoBehaviour
{
    public WhacASphereVariables variables;
    public WhacASphere leftGame;
    public WhacASphere rightGame;
    public WhacASphereTester tester;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] Image errorCross;

    [SerializeField] float totalTime;
    [SerializeField] Image timerIcon;
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
        //timeText.text = remainingTime < 10 ? "00:0" + remainingTime : "00:" + remainingTime;
        StartCoroutine(CountdownTime());
        if (!isTesting)
        {
            //this.transform.position = CalculateGamePosition();
            this.transform.position = GameManager.Instance.platform.transform.position + (GameManager.Instance.platform.transform.forward * 10f); //TODO: make better after demo
            this.transform.position += new Vector3(0, 4f, 0);
            transform.LookAt(Camera.main.transform);
            transform.eulerAngles -= new Vector3(transform.localEulerAngles.x, 90, 0);
        }
    }


    IEnumerator CountdownTime()
    {
        yield return new WaitForSeconds(1f);

        remainingTime--;
        //timeText.text = remainingTime < 10 ? "00:0" + remainingTime : "00:" + remainingTime;
        timerIcon.fillAmount = remainingTime / totalTime;
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
        int finalScore = finalLeftScore + finalRightScore;
        float maximumPossibleScore = (totalTime / variables.timeBetweenActivation) * 10f * 2f; // 10 points per activation, times 2 because we use two sides
        float desiredPoints = maximumPossibleScore / 2f; //We want the player to obtain half of the obtainable points
        float absoluteDifference = Mathf.Abs(finalScore - desiredPoints);
        float tenPercent = maximumPossibleScore / 10f;
        float multiplier = Mathf.Clamp(absoluteDifference / tenPercent, 1f, Mathf.Infinity);
        float scaler = absoluteDifference / desiredPoints / 10f;

        if (finalScore > desiredPoints)
        {
            if (!isTesting)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f - scaler * (multiplier + 1f)), 0.0000001f, variables.totalTime);
                return;
            }
            if (WhacASphereSpawner.instance.justScalar)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f - scaler), 0.0000001f, variables.totalTime);
            }
            else if (WhacASphereSpawner.instance.scalarNmultiplier)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f - scaler * (multiplier)), 0.0000001f, variables.totalTime);
            }
            else if (WhacASphereSpawner.instance.scalarMultiplierNOffset)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f - scaler * (multiplier + 1f)), 0.0000001f, variables.totalTime);
            }
            else if (WhacASphereSpawner.instance.noScaling)
            {
            }
        }
        else if (finalScore < desiredPoints)
        {
            if (!isTesting)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f + scaler * (multiplier + 1f)), 0.0000001f, variables.totalTime);
                return;
            }
            if (WhacASphereSpawner.instance.justScalar)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f + scaler), 0.0000001f, variables.totalTime);
            }
            else if (WhacASphereSpawner.instance.scalarNmultiplier)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f + scaler * (multiplier)), 0.0000001f, variables.totalTime);
            }
            else if (WhacASphereSpawner.instance.scalarMultiplierNOffset)
            {
                variables.activeTime = Mathf.Clamp(variables.activeTime * (1f + scaler * (multiplier + 1f)), 0.0000001f, variables.totalTime);
            }

        }

        if (isTesting)
        {
            Debug.Log(
    "\nIteration = " + variables.iteration +
    "\nMaximum Possible Score = " + maximumPossibleScore +
    "\nDesired points  = " + desiredPoints +
    "\nFinal Score = " + finalScore
    );
            variables.testerSpeed += variables.skillGrowth * Mathf.Clamp(1f - (variables.iteration * 2f) / variables.iterationsToTest, 0f, 1f); //simulates player skill growing over time
            variables.iteration++;
            variables.totalIterations++;
            LogPlayerData(finalScore);
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

    void LogPlayerData(int finalScore)
    {
        string average = "";
        if (variables.iteration == 1)
        {
            MyTools.DEV_AppendHeadersToReport();
            variables.excelOffset++;
            average += finalScore.ToString();
        }
        else
        {
            average += "=AVERAGE($B$" + (variables.totalIterations + variables.excelOffset) + ":$B$" + (variables.totalIterations + 1 + variables.excelOffset) + ")";
        }
        MyTools.DEV_AppendSpecificsToReport(new string[5] { variables.iteration.ToString(), finalScore.ToString(), average, (variables.activeTime * variables.speedUpDivider).ToString("n3"), variables.playerSkill.ToString() });
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

        totalScoreText.text = "Total score = " + totalScore + "\nTotal added point: " + totalScore / 10;

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
            GameManager.Instance.AddScore(totalScore/10);
            GameManager.Instance.UpdateText();
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
