using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaytestCapsule : MonoBehaviour
{
    [SerializeField] List<GameObject> giftLocations = new List<GameObject>();
    [SerializeField] List<GameObject> chosenLocations = new List<GameObject>();
    [SerializeField] List<GameObject> spawnedGifts = new List<GameObject>();

    [SerializeField] GameObject destroyableGift;
    [SerializeField] GameObject grabbableGift;
    [SerializeField] GameObject locationParent;
    [SerializeField] GameObject cheatPrevention;

    [SerializeField] Animator capsuleAnim;

    [SerializeField] TextMeshProUGUI timeText;

    [SerializeField] int totalTime = 15;
    int currentTime;
    int locationAmount;
    int amountToSpawn;

    [SerializeField] float rotationSpeed;
    [SerializeField] float respawnWaitTime = 1f;
    float newRotation;

    bool rotating;

    void Start()
    {
        foreach (Transform t in locationParent.transform)
        {
            giftLocations.Add(t.gameObject);
        }

        locationAmount = giftLocations.Count;
        amountToSpawn = locationAmount / 2;
        SpawnGifts();
    }

    private void Update()
    {
        if (rotating)
        {
            RotateCapsule(newRotation);
        }
    }

    void ManageTime()
    {
        currentTime = totalTime;
        StartCoroutine(CountdownTime());
    }

    IEnumerator CountdownTime()
    {
        currentTime--;
        timeText.text = "Time Left: " + currentTime;
        yield return new WaitForSeconds(1f);
        if (currentTime <= 0)
        {
            timeText.text = "Fail! Gifts left: " + spawnedGifts.Count;
            StartCoroutine(RespawnGifts());
        }
        else if (currentTime > 0)
        {
            StartCoroutine(CountdownTime());
        }
    }
    IEnumerator RespawnGifts()
    {
        cheatPrevention.SetActive(true);
        yield return new WaitForSeconds(respawnWaitTime);
        StartCoroutine(CloseCapsule());
    }
    void SpawnGifts()
    {
        ClearListsForRespawn();
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject locationGO = PickGiftLocation();
            chosenLocations.Add(locationGO);
        }
        foreach(GameObject go in chosenLocations)
        {
            GameObject giftGO = Instantiate(destroyableGift, go.transform.position, Quaternion.identity);
            giftGO.GetComponentInChildren<GiftBehaviour>().attachedCapsule = this;
            spawnedGifts.Add(giftGO);
        }
        ManageTime();
        OpenCapsule();
    }

    public void UpdateGifts(GameObject gift)
    {
        spawnedGifts.Remove(gift);
        int remainingGifts = 0;
        foreach(GameObject go in spawnedGifts)
        {
            remainingGifts++;
        }
        if (remainingGifts == 0)
        {
            timeText.text = "Good job!";
            StartCoroutine(RespawnGifts());
        }
    }
    void OpenCapsule()
    {
        cheatPrevention.SetActive(false);
        capsuleAnim.SetFloat("Speed", 1f);
        capsuleAnim.SetTrigger("OpeningTrigger");
    }

    IEnumerator CloseCapsule()
    {
        capsuleAnim.SetFloat("Speed", -0.5f);
        capsuleAnim.SetTrigger("OpeningTrigger");
        yield return new WaitForSeconds(capsuleAnim.GetCurrentAnimatorStateInfo(0).length);// + capsuleAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        SetNewRotation();
        rotating = true;
    }
    void SetNewRotation()
    {
        int i = Random.Range(1, 3);
        if (i % 2 == 0)
        {
            newRotation = this.transform.localEulerAngles.y + 90f + Random.Range(0f, 180f);
        }
        else if (i % 2 == 1)
        {
            newRotation = this.transform.localEulerAngles.y - 90f - Random.Range(0f, 180f);
        }
        if (newRotation > 360)
        {
            newRotation -= 360;
        }
        else if (newRotation < 0)
        {
            newRotation += 360;
        }
    }

    void RotateCapsule(float toRotateTo)
    {
        this.transform.localEulerAngles = new Vector3(0f, Mathf.Lerp(this.transform.localEulerAngles.y, toRotateTo, rotationSpeed * Time.deltaTime), 0f);
        if (Mathf.Approximately(this.transform.localEulerAngles.y, toRotateTo))
        {
            rotating = false;
            OpenCapsule();
        }
        if (FastApproximately(this.transform.localEulerAngles.y, toRotateTo, 10f))
        {
            rotating = false;
            SpawnGifts();
        }
    }

    void ClearListsForRespawn()
    {
        chosenLocations.Clear();
        foreach(GameObject go in spawnedGifts)
        {
            GameManager.Instance.AddScore(-10);
            Destroy(go);
        }
        spawnedGifts.Clear();
    }
    GameObject PickGiftLocation()
    {
        GameObject chosenGiftLocation = giftLocations[Random.Range(0, locationAmount)];
        if (chosenLocations.Contains(chosenGiftLocation))
        {
            chosenGiftLocation = PickGiftLocation();
        }
        return chosenGiftLocation;
    }


    public static bool FastApproximately(float a, float b, float threshold)
    {
        if (threshold > 0f)
        {
            return Mathf.Abs(a - b) <= threshold;
        }
        else
        {
            return Mathf.Approximately(a, b);
        }
    }
}
