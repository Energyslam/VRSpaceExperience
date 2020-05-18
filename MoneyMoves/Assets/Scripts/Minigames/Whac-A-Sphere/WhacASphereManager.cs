using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.PackageManager;
using UnityEngine.PlayerLoop;

public class WhacASphereManager : MonoBehaviour
{
    public WhacASphere leftSide;
    public WhacASphere rightSide;
    Platform platform;
    public GameObject explosion;
    public Vector3 destrucTorque;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI totalScoreText;
    public Image ErrorCross;
    public int totalTime;
    int remainingTime;
    int finalLeftScore;
    int finalRightScore;
    int totalScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        platform = GameManager.Instance.platform;
        remainingTime = totalTime;
        Vector3 UNO = Tracks.SplitToA[Tracks.SplitToA.Count / 4];
        Vector3 DUO = Tracks.SplitToB[Tracks.SplitToB.Count / 4];
        Vector3 UNOtoDUO = DUO - UNO;
        Vector3 endPosition = UNO + UNOtoDUO / 2;
        endPosition.y += 5;
        this.transform.position = endPosition;
        transform.LookAt(Camera.main.transform);
        transform.eulerAngles -= new Vector3(transform.localEulerAngles.x, 90, 0);
        StartCoroutine(CountdownTime());
    }
    IEnumerator CountdownTime()
    {
        if (remainingTime < 10)
        {
            timeText.text = "00: 0" + remainingTime;
        }
        else if (remainingTime >= 10)
        {
            timeText.text = "00: " + remainingTime;
        }
        yield return new WaitForSeconds(1f);
        remainingTime--;
        if (remainingTime > 0)
        {
            StartCoroutine(CountdownTime());
        }
        else if (remainingTime <= 0)
        {
            leftSide.DeactivateAll();
            rightSide.DeactivateAll();
            finalLeftScore = leftSide.score;
            finalRightScore = rightSide.score;
            StartCoroutine(CalculateFinalScore());
        }
    }

    IEnumerator CalculateFinalScore()
    {
        yield return new WaitForSeconds(0.1f);
        if (!totalScoreText.gameObject.activeInHierarchy)
        {
            totalScoreText.gameObject.SetActive(true);
        }

        if (leftSide.score - 10 >= 0)
        {
            leftSide.UpdateScore(-10);
            totalScore += 10;
        }
        else if (leftSide.score -10 < 0)
        {
            if (!ErrorCross.gameObject.activeInHierarchy)
            {
                ErrorCross.gameObject.SetActive(true);
                ErrorCross.transform.position = leftSide.transform.position;
                ErrorCross.transform.position += new Vector3(0, 0, -0.5f);
            }
        }
        if (rightSide.score -10 >= 0)
        {
            rightSide.UpdateScore(-10);
            totalScore += 10;

        }
        else if (rightSide.score - 10 < 0)
        {
            if (!ErrorCross.gameObject.activeInHierarchy)
            {
                ErrorCross.gameObject.SetActive(true);
                ErrorCross.transform.position = rightSide.transform.position;
                ErrorCross.transform.position += new Vector3(0, 0, -0.5f);
            }
        }

        totalScoreText.text = "Total score = " + totalScore;

        if (leftSide.score - 10 < 0 && rightSide.score -10 < 0)
        {
            yield return new WaitForSeconds(2f);
            EndGame();
        }
        else
        {
            StartCoroutine(CalculateFinalScore());
        }
    }

    public void EndGame()
    {
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
}
