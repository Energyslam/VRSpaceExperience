using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaticCapsule : MonoBehaviour
{
     List<GameObject> giftLocations = new List<GameObject>();
     List<GameObject> chosenLocations = new List<GameObject>();
     List<GameObject> spawnedGifts = new List<GameObject>();

    [SerializeField] GameObject destroyableGift;
    [SerializeField] GameObject grabbableGift;
    [SerializeField] GameObject locationParent;
    [SerializeField] GameObject cheatPrevention;
    public GameObject dockingSpot;
    public GameObject otherCapsuleInWave;

    [SerializeField] Animator capsuleAnim;

    [SerializeField] TextMeshProUGUI timeText;

    [SerializeField] LightFlicker light;

    [SerializeField] int totalTime = 15;
    int currentTime;
    int locationAmount;
    int amountToSpawn;
    [SerializeField] int timesToOpen = 3;
    public int timesOpened = 0;

    [SerializeField] float rotationSpeed;
    [SerializeField] float respawnWaitTime = 2f;
    float newRotation;
    float textStartingY;

    public bool rotateText = true;
    bool rotating;

    enum GiftType
    {
        Destroyable,
        Collectable
    }
    [SerializeField] GiftType giftType = GiftType.Destroyable;

    public float textSpeed = 50f;


    void Start()
    {
        Wave wave = GetComponentInParent<Wave>();
        otherCapsuleInWave = this.gameObject == wave.a.gameObject ? wave.b.gameObject : wave.a.gameObject; 
        dockingSpot.transform.position = this.transform.position + (otherCapsuleInWave.transform.position - dockingSpot.transform.position).normalized * (dockingSpot.transform.position - this.transform.position).magnitude;
        textStartingY = timeText.transform.position.y;
        foreach (Transform t in locationParent.transform)
        {
            giftLocations.Add(t.gameObject);
        }
        locationAmount = giftLocations.Count;
        amountToSpawn = locationAmount / 2;
    }

    public void OpenUp()
    {
        SpawnGifts();
    }
    private void Update()
    {
        if (rotating)
        {
            RotateCapsule(newRotation);
        }
        if (rotateText)
        {
            timeText.transform.RotateAround(this.transform.position, Vector3.up, textSpeed * Time.deltaTime);
        }
    }

    void ManageTime()
    {
        currentTime = totalTime;
        timeText.color = Color.white;
        StartCoroutine(CountdownTime());
    }

    IEnumerator CountdownTime()
    {
        timeText.text = "Time Left: " + currentTime;
        yield return new WaitForSeconds(1f);
        currentTime--;
        timeText.text = "Time Left: " + currentTime;
        if (currentTime <= 0)
        {
            timeText.color = Color.red;
            timeText.text = "Fail ! You didn't get all gifts";
            StartCoroutine(CloseCapsule());
        }
        else if (currentTime > 0)
        {
            StartCoroutine(CountdownTime());
        }
    }
    void SpawnGifts()
    {
        ClearListsForRespawn();
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject locationGO = PickGiftLocation();
            chosenLocations.Add(locationGO);
        }
        foreach (GameObject go in chosenLocations)
        {
            if (giftType == GiftType.Destroyable)
            {
                GameObject giftGO = Instantiate(destroyableGift, go.transform.position, Quaternion.identity);
                giftGO.transform.parent = this.transform;
                giftGO.GetComponentInChildren<GiftBehaviour>().attachedStatic = this;
                spawnedGifts.Add(giftGO);
            }
            else if (giftType == GiftType.Collectable)
            {
                GameObject giftGO = Instantiate(grabbableGift, go.transform.position, Quaternion.identity);
                giftGO.transform.parent = this.transform;
                giftGO.GetComponentInChildren<GiftBehaviour>().attachedStatic = this;
                spawnedGifts.Add(giftGO);
            }
        }
        ManageTime();
        OpenCapsule();
    }

    public void UpdateGifts(GameObject gift)
    {
        spawnedGifts.Remove(gift);
        int remainingGifts = 0;
        foreach (GameObject go in spawnedGifts)
        {
            remainingGifts++;
        }
        if (remainingGifts == 0)
        {
            timeText.color = Color.green;
            timeText.text = "Good job!";
            StopAllCoroutines();
            StartCoroutine(CloseCapsule());
        }
    }
    void OpenCapsule()
    {
        timesOpened++;
        cheatPrevention.SetActive(false);
        capsuleAnim.SetFloat("Speed", 1f);
        capsuleAnim.SetTrigger("OpeningTrigger");

        if (!rotateText)
        {
            Vector3 textToCapsule = this.transform.position - timeText.transform.position;
            float textDistance = textToCapsule.magnitude;
            Vector3 playerToCapsule = this.transform.position - GameManager.Instance.player.transform.position;
            timeText.transform.position = this.transform.position + (playerToCapsule.normalized * textDistance);
            timeText.transform.LookAt(GameManager.Instance.player.transform);
            timeText.transform.localEulerAngles += new Vector3(0f, 180f, 0f);
            timeText.transform.position = new Vector3(timeText.transform.position.x, textStartingY, timeText.transform.position.z);
        }
        timeText.gameObject.SetActive(true);
        light.TurnOnLight();
    }

    IEnumerator CloseCapsule()
    {
        cheatPrevention.SetActive(true);
        yield return new WaitForSeconds(respawnWaitTime);
        light.TurnOffLight();
        timeText.gameObject.SetActive(false);
        capsuleAnim.SetFloat("Speed", -0.5f);
        capsuleAnim.SetTrigger("OpeningTrigger");
        yield return new WaitForSeconds(capsuleAnim.GetCurrentAnimatorStateInfo(0).length);
        if (timesOpened >= timesToOpen)
        {
            //TODO: Player moves to new location
            WaveManager.Instance.GetNextWave();
            yield break;
        }
        SetNewRotation();
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
        rotating = true;
    }

    void RotateCapsule(float toRotateTo)
    {
        this.transform.localEulerAngles = new Vector3(0f, Mathf.Lerp(this.transform.localEulerAngles.y, toRotateTo, rotationSpeed * Time.deltaTime), 0f);
        if (FastApproximately(this.transform.localEulerAngles.y, toRotateTo, 10f))
        {
            rotating = false;
            SpawnGifts();
        }
    }

    void ClearListsForRespawn()
    {
        chosenLocations.Clear();
        foreach (GameObject go in spawnedGifts)
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

    public static bool FastVectorApproximately(Vector3 a, Vector3 b, float threshold)
    {
        if (FastApproximately(a.x, b.x, threshold) && FastApproximately(a.y, b.y, threshold) && FastApproximately(a.z, b.z, threshold)){
            return true;
        }
        else
        {
            return false;
        }
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
