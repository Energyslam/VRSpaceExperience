﻿using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class WhacASphereManager : MonoBehaviour
{
    public WhacASphereVariables variables;
    [SerializeField] WhacASphere leftGame;
    [SerializeField] WhacASphere rightGame;

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] Image errorCross;

    [SerializeField] int totalTime;
    int remainingTime;
    int finalLeftScore;
    int finalRightScore;
    int totalScore = 0;
    int totalPossibleScore;
    [Header("After minigame")]
    Platform platform;
    public GameObject explosion;
    public Vector3 destrucTorque;
    public bool isTesting;

    void Start()
    {
        platform = GameManager.Instance.platform;
        this.totalTime = variables.totalTime;
        remainingTime = totalTime;
        timeText.text = remainingTime < 10 ? "00:0" + remainingTime : "00:" + remainingTime;
        if (!isTesting){
            this.transform.position = CalculateGamePosition();
            transform.LookAt(Camera.main.transform);
            transform.eulerAngles -= new Vector3(transform.localEulerAngles.x, 90, 0);
        }
        StartCoroutine(CountdownTime());
        totalPossibleScore = (int)Mathf.Round(totalTime / variables.timeBetweenActivation);
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

    void StartCalculatingFinalScore()
    {
        leftGame.DeactivateAll();
        rightGame.DeactivateAll();
        finalLeftScore = leftGame.score;
        finalRightScore = rightGame.score;
        totalScoreText.gameObject.SetActive(true);
        StartCoroutine(CalculateFinalScore());
    }
    IEnumerator CalculateFinalScore()
    {
        yield return new WaitForSeconds(0.1f);

        DisplayErrorCross();

        totalScoreText.text = "Total score = " + totalScore;

        if (leftGame.score - 10 < 0 && rightGame.score -10 < 0)
        {
            yield return new WaitForSeconds(2f);
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
                errorCross.transform.localPosition = new Vector3(3f, errorCross.transform.localPosition.y, errorCross.transform.localPosition.z);
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
                errorCross.transform.localPosition = new Vector3(-3f, errorCross.transform.localPosition.y, errorCross.transform.localPosition.z);
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
        //TODO: 
        Vector3 trackLeftQuarter = Tracks.SplitToA[Tracks.SplitToA.Count / 4];
        Vector3 trackRightQuarter = Tracks.SplitToB[Tracks.SplitToB.Count / 4];
        Vector3 vectorFromLeftToRightQuarter = trackRightQuarter - trackLeftQuarter;
        Vector3 endPosition = trackLeftQuarter + vectorFromLeftToRightQuarter / 2f;
        endPosition.y = GameManager.Instance.player.transform.position.y + 2f;
        return endPosition;
    }
}
