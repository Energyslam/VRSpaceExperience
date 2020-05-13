using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Minigame : MonoBehaviour
{
    [SerializeField] GameObject target;
    public TextMeshProUGUI leftScore, rightScore, time;
    int leftPoints = 0, rightPoints = 0;
    [SerializeField] int totalTime;
    int remainingTime;
    Vector3 centeroffset;
    void Start()
    {
        centeroffset = new Vector3(Camera.main.transform.position.x - 15, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Vector3 UNO = Tracks.SplitToA[Tracks.SplitToA.Count / 4];
        Vector3 DUO = Tracks.SplitToB[Tracks.SplitToB.Count / 4];
        Vector3 UNOtoDUO = DUO - UNO;
        Vector3 endPosition = UNO + UNOtoDUO / 2;
        endPosition.y += 5;
        this.transform.position = endPosition;
        transform.LookAt(Camera.main.transform);
        transform.eulerAngles -= new Vector3(transform.localEulerAngles.x, 90, 0);
        remainingTime = totalTime;
        StartCoroutine(CountdownTime());
    }

    public void UpdateScore(TinyGame.GameSide side)
    {
        if (side == TinyGame.GameSide.Left)
        {
            leftPoints += 1;
            leftScore.text = "" + leftPoints;
        } else if (side == TinyGame.GameSide.RIght)
        {
            rightPoints += 1;
            rightScore.text = "" + rightPoints;
        }
    }

    IEnumerator CountdownTime()
    {
        if (remainingTime < 10)
        {
            time.text = "00: 0" + remainingTime;
        }
        else if (remainingTime >= 10)
        {
            time.text = "00: " + remainingTime;
        }
        yield return new WaitForSeconds(1f);
        remainingTime--;
        if (remainingTime > 0)
        {
            StartCoroutine(CountdownTime());
        } 
        else if (remainingTime <= 0)
        {
            if (leftPoints > rightPoints)
            {
                GameManager.Instance.platform.ChangeStateToA();
            }
            else if (rightPoints > leftPoints)
            {
                GameManager.Instance.platform.ChangeStateToB();
            }
            else if (leftPoints == rightPoints)
            {
                GameManager.Instance.platform.ChangeStateToA();
            }
            Destroy(this.gameObject);
        }
    }
}
