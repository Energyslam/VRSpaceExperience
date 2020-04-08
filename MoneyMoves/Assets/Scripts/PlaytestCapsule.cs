using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

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

    public bool available;
    public bool rotateText = true;
    public bool flickerLights;
    bool rotating;
    bool moving;

    enum GiftType
    {
        Destroyable,
        Collectable
    }
    [SerializeField]GiftType giftType = GiftType.Destroyable;
    #region navmeshtesting

    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    GameObject destination;
    Vector3 originalPosition;
    public float textSpeed = 50f;

    #endregion

    void Start()
    {
        textStartingY = timeText.transform.position.y;
        available = true;
        originalPosition = this.transform.position;
        foreach (Transform t in locationParent.transform)
        {
            giftLocations.Add(t.gameObject);
        }

        agent = this.GetComponentInParent<NavMeshAgent>();
        obstacle = this.GetComponentInParent<NavMeshObstacle>();
        agent.avoidancePriority = GameManager.Instance.AssignLowPriority();
        GameManager.Instance.priority--;
        locationAmount = giftLocations.Count;
        amountToSpawn = locationAmount / 2;
        //SpawnGifts();
    }

    private void Update()
    {
        UpdateAgent();
        if (rotating)
        {
            RotateCapsule(newRotation);
        }
        if (rotateText)
        {
            timeText.transform.RotateAround(this.transform.position, Vector3.up, textSpeed * Time.deltaTime);
        }
    }

    void ResetVariables()
    {
        timesOpened = 0;
        available = true;
    }
    void UpdateAgent()
    {
        if (!moving) return;
        if (this.transform.position == agent.destination)
        {
            moving = false;
            if (FastApproximately(agent.destination.x, originalPosition.x, 1f) && FastApproximately(agent.destination.z, originalPosition.z, 1f))
            {
                ResetVariables();
            }
            else
            {
                this.agent.enabled = false;
                StartCoroutine(activateObstacle());
                SpawnGifts();
                //Rotatetoplayer, then open capsule
                //GameObject tempObject = new GameObject();
                //tempObject.transform.position = this.transform.position;
                //tempObject.transform.LookAt(GameManager.Instance.player.transform.position);
                //Debug.Log(tempObject.transform.localEulerAngles.y);
                //Debug.Log(tempObject.transform.localEulerAngles.y + 180f);
                //newRotation = tempObject.transform.localEulerAngles.y + 180f;
                //rotating = true;
            }
        }
    }
    public void MoveToDestination()
    {
        moving = true;
        available = false;
        destination = GameManager.Instance.destinations[Random.Range(0, GameManager.Instance.destinations.Count)];
        GameManager.Instance.destinations.Remove(destination);
        agent.destination = destination.transform.position;
    }

    void MoveToOriginalPosition()
    {
        this.obstacle.enabled = false;
        StartCoroutine(activateAgent());


    }

    IEnumerator activateObstacle()
    {
        Debug.Log("What");
        yield return new WaitForSeconds(0.01f);
        Debug.Log("The Frick");
        obstacle.enabled = true;
    }

    IEnumerator activateAgent()
    {
        Debug.Log("Mister");
        yield return new WaitForSeconds(0.01f);
        Debug.Log("Agent");
        agent.enabled = true;
        agent.avoidancePriority = GameManager.Instance.AssignHighPriority();
        moving = true;
        GameManager.Instance.destinations.Add(destination);
        agent.destination = originalPosition;
        GameManager.Instance.MoveACapsule();

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
            if (giftType == GiftType.Destroyable)
            {
                GameObject giftGO = Instantiate(destroyableGift, go.transform.position, Quaternion.identity);
                giftGO.transform.parent = this.transform;
                giftGO.GetComponentInChildren<GiftBehaviour>().attachedCapsule = this;
                spawnedGifts.Add(giftGO);
            }
            else if (giftType == GiftType.Collectable)
            {
                GameObject giftGO = Instantiate(grabbableGift, go.transform.position, Quaternion.identity);
                giftGO.transform.parent = this.transform;
                giftGO.GetComponentInChildren<GiftBehaviour>().attachedCapsule = this;
                spawnedGifts.Add(giftGO);
            }
        }
        ManageTime();
        StartCoroutine(OpenCapsule());
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
            timeText.color = Color.green;
            timeText.text = "Good job!";
            StopAllCoroutines();
            StartCoroutine(RespawnGifts());
        }
    }
    IEnumerator OpenCapsule()
    {
        timesOpened++;
        cheatPrevention.SetActive(false);
        capsuleAnim.SetFloat("Speed", 1f);
        capsuleAnim.SetTrigger("OpeningTrigger");
        //Calculate distance, set it to opposite of player, rotate towards player
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
        if(!flickerLights)
        {
            light.TurnOnLight();
        }
        yield return new WaitForSeconds(capsuleAnim.GetCurrentAnimatorStateInfo(0).length);
        if (flickerLights)
        {
            light.FlickerLights();
        }


    }

    IEnumerator CloseCapsule()
    {
        light.TurnOffLight();
        timeText.gameObject.SetActive(false);
        capsuleAnim.SetFloat("Speed", -0.5f);
        capsuleAnim.SetTrigger("OpeningTrigger");
        yield return new WaitForSeconds(capsuleAnim.GetCurrentAnimatorStateInfo(0).length);// + capsuleAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (timesOpened >= timesToOpen)
        {
            MoveToOriginalPosition();
            yield break;
        }
        Debug.Log("Opening");
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
        if (FastApproximately(this.transform.localEulerAngles.y, toRotateTo, 10f))
        {
            rotating = false;
            SpawnGifts(); //CC
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
