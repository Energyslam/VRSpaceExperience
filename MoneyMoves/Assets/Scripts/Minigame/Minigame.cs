using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Minigame : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject explosion;
    [SerializeField] TinyGame leftTinyGame;
    [SerializeField] TinyGame rightTinyGame;
    [SerializeField] GameObject whirlingText;

    public TextMeshProUGUI leftScore, rightScore, time;
    int leftPoints = 0, rightPoints = 0;
    [SerializeField] int totalTime;
    int remainingTime;
    public Vector3 destrucTorque;
    public float waitBeforeStart;
    Platform platform;
    Vector3 centeroffset;
    void Start()
    {
        platform = GameManager.Instance.platform;
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
        if (!GameManager.Instance.hasShownInstruction)
        {
            StartCoroutine(WaitToStart());
        }
        else
        {
            StartPlay();
        }
    }

    IEnumerator WaitToStart()
    {
        GameManager.Instance.hasShownInstruction = true;
        yield return new WaitForSeconds(waitBeforeStart);
        whirlingText.AddComponent<WhirlingText>();
        yield return new WaitForSeconds(0.2f);
        StartPlay();
    }

    public void StartPlay()
    {
        leftTinyGame.playing = true;
        rightTinyGame.playing = true;
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
                MoveToA();
            }
            else if (rightPoints > leftPoints)
            {
                MoveToB();
            }
            else if (leftPoints == rightPoints)
            {
                //TODO: do something else than defaulting to left
                MoveToA();
            }
            Destroy(this.gameObject);
        }
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
