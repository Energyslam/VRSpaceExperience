﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Rendering;

public class StaticCapsule : MonoBehaviour
{
    [SerializeField] List<GameObject> connectingWalls = new List<GameObject>();
    List<GameObject> giftLocations = new List<GameObject>();
    List<GameObject> chosenLocations = new List<GameObject>();
    List<GameObject> spawnedGifts = new List<GameObject>();
    [SerializeField] GameObject giftPrefab;
    [SerializeField] GameObject locationParent;
    [SerializeField] GameObject cheatPrevention;
    [SerializeField] GameObject otherCapsuleInWave;
    public GameObject dockingSpot;

    [SerializeField] GridManager gridManager;

    [SerializeField] List<CapsuleAnimations> openDoors = new List<CapsuleAnimations>();
    [SerializeField] List<CapsuleAnimations> animationHandlers;

    [SerializeField] AudioSource jukeBox;

    [SerializeField] Animator capsuleAnim;

    [SerializeField] public TextMeshProUGUI[] timeText;
    public GameObject timetextHolder;

    [SerializeField] int totalTime = 15;
    [SerializeField] int timesToOpen = 3;
    [SerializeField] int doorsToOpenAtOnce = 1;
    int currentTime;
    int locationAmount;
    int amountToSpawn;
    int timesOpened = 0;
    public int remainingGifts = 0;

    [SerializeField] List<float> distancesDoors = new List<float>();
    List<float> distancesPillars = new List<float>();
    [SerializeField] float rotationSpeed;
    [SerializeField] float respawnWaitTime = 2f;
    [SerializeField] float audioLerpSpeed;
    [SerializeField] float textSpeed = 50f;
    float newRotation;
    float textStartingY;

    bool rotating;

    enum GiftType
    {
        Destroyable,
        Grabbable
    }
    [SerializeField] GiftType giftType = GiftType.Destroyable;

    [SerializeField]
    private VolumeProfile postProcessingProfile;

    private void Start()
    {
        timesToOpen = GameManager.Instance.timesToOpenCapsules;
        totalTime = GameManager.Instance.totalOpenTime;
        respawnWaitTime = GameManager.Instance.respawnWaitTime;
        if (timetextHolder != null)
        {
            timeText = timetextHolder.GetComponentsInChildren<TextMeshProUGUI>();
        }
        Wave wave = GetComponentInParent<Wave>();
        if (wave != null)
        {
            otherCapsuleInWave = this.gameObject == wave.leftCapsule.gameObject ? wave.rightCapsule.gameObject : wave.leftCapsule.gameObject;
        }

        if (dockingSpot != null)
        {
            float dockingSpotY = dockingSpot.transform.position.y;
            dockingSpot.transform.position = this.transform.position + (otherCapsuleInWave.transform.position - dockingSpot.transform.position).normalized * (dockingSpot.transform.position - this.transform.position).magnitude;
            dockingSpot.transform.position = new Vector3(dockingSpot.transform.position.x, dockingSpotY, dockingSpot.transform.position.z);
            this.transform.LookAt(dockingSpot.transform);
            this.transform.localEulerAngles = new Vector3(0, this.transform.localEulerAngles.y, 0);
        }
        //textStartingY = timeText.transform.position.y;

        foreach (Transform t in locationParent.transform)
        {
            giftLocations.Add(t.gameObject);
        }
        locationAmount = giftLocations.Count;
        amountToSpawn = locationAmount / 2;
        remainingGifts = amountToSpawn;
    }

    public void OpenUp()
    {
        //SetNewRotation(); //starts with a rotation, call openfreshcapsule if first rotation isn't wanted
        OpenFreshCapsule();
    }
    void OpenFreshCapsule()
    {
        ClearListsForRespawn();
        SpawnGifts();
        ManageTime();
        OpenCapsule();
    }
    private void Update()
    {
        if (rotating)
        {
            RotateCapsule();
        }
    }

    void ManageTime()
    {
        currentTime = totalTime;
        timetextHolder.gameObject.SetActive(true);
        timetextHolder.transform.rotation = this.transform.rotation;
        foreach (TextMeshProUGUI text in timeText)
        {
            text.color = Color.white;
            text.GetComponentInParent<MaskedText>().enabled = true;
        }
        StartCoroutine(CountdownTime());
    }

    IEnumerator CountdownTime()
    {
        string tmpTime = "" + (currentTime >= 10 ? currentTime.ToString() : "0" + currentTime);

        foreach (TextMeshProUGUI text in timeText)
        {
            text.text = "                Time: " + tmpTime + "                Time: " + tmpTime + "                Time: " + tmpTime;
        }



        yield return new WaitForSeconds(1f);
        currentTime--;
        foreach (TextMeshProUGUI text in timeText)
        {
            text.text = "                Time: " + tmpTime + "                Time: " + tmpTime + "                Time: " + tmpTime;
        };
        if (currentTime <= 0)
        {
            foreach (TextMeshProUGUI text in timeText)
            {
                text.color = Color.red;
                text.text = "Failed!";
                text.transform.localPosition = Vector3.zero;
                text.GetComponentInParent<MaskedText>().enabled = false;
            }
            StartCoroutine(CloseCapsule());
        }
        else if (currentTime > 0)
        {
            StartCoroutine(CountdownTime());
        }
    }

    void SpawnGifts()
    {
        int spawnAmount = amountToSpawn; // remainingGifts == 0 ? amountToSpawn : remainingGifts;

        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject locationGO = PickGiftLocation();
            chosenLocations.Add(locationGO);
        }
        foreach (GameObject go in chosenLocations)
        {
            GameObject giftGO = Instantiate(giftPrefab, go.transform.position, Quaternion.identity);
            giftGO.transform.parent = this.transform;
            giftGO.GetComponentInChildren<GiftBehaviour>().attachedStatic = this;
            spawnedGifts.Add(giftGO);
            if (giftType == GiftType.Grabbable)
            {
                giftGO.GetComponent<GiftBehaviour>().isGrabbable = true;
            }
        }

        remainingGifts = spawnAmount;
    }

    public void UpdateGifts(GameObject gift)
    {
        spawnedGifts.Remove(gift);
        remainingGifts--;
        if (remainingGifts <= 0)
        {
            foreach(TextMeshProUGUI text in timeText)
            {
                text.color = Color.green;
                text.text = "Good job!";
                text.transform.localPosition = Vector3.zero;
                text.GetComponentInParent<MaskedText>().enabled = false;
            }
            StopAllCoroutines();
            StartCoroutine(CloseCapsule());
        }
    }
    void OpenCapsule()
    {

        timesOpened++;
        CalculateNearestDoors();
        PlayMusic();

        //CalculateVisibleTextPosition();
        cheatPrevention.SetActive(false);
        GameManager.Instance.GetComponent<Volume>().profile = postProcessingProfile;
    }

    IEnumerator CloseCapsule()
    {
        cheatPrevention.SetActive(true);
        yield return new WaitForSeconds(respawnWaitTime);
        timetextHolder.gameObject.SetActive(false);
        CalculateNearestDoors();

        //yield return new WaitForSeconds(capsuleAnim.GetCurrentAnimatorStateInfo(0).length);

        if (timesOpened >= timesToOpen)
        {
            StartCoroutine(StopMusic());
            GetComponent<GridManager>().Clean();
            WaveManager.Instance.GetNextWave();
            GameManager.Instance.ResetPPX();
            yield break;
        }
        ClearListsForRespawn();
        SetNewRotation();
    }

    #region Rotation
    void SetNewRotation()
    {
        newRotation = RepickAngle(newRotation);
        if (newRotation == 45 || newRotation == 135 || newRotation == 225 || newRotation == 315)
        {
            doorsToOpenAtOnce = 2;
        }
        else if (newRotation == 90 || newRotation == 180 || newRotation == 270)
        {
            doorsToOpenAtOnce = 1;
        }
        rotating = true;
    }

    float RepickAngle(float angle)
    {
        float newAngle = 0f;
        float[] angles = { 45, 90, 135, 180, 225, 270, 315 };
        float chosenAngle = angles[Random.Range(0, angles.Length)];
        if (!HelperFunctions.FastApproximately(chosenAngle, angle, 2f))
        {
            newAngle = chosenAngle;
        }
        else if (HelperFunctions.FastApproximately(chosenAngle, angle, 6f))
        {
            newAngle = RepickAngle(angle);
        }

        //TODO: Mister Tycho-chan, als deze wordt aangeroepen, dan gaat er iets fout bij het draaien van de capsule maar ik weet niet precies hoe je code dit voor elkaar krijgt. Ik heb geprobeerd voor draaien, na draaien, zodra die klaar is met draaien etc. dus je moet even in jouw code kijken.
        //if (remainingGifts <= 0)
        //{
        //    GetComponent<GridManager>().Reset();
        //}
        return newAngle;
    }
    void RotateCapsule()
    {
        this.transform.localEulerAngles = new Vector3(0f, Mathf.Lerp(this.transform.localEulerAngles.y, newRotation, rotationSpeed * Time.deltaTime), 0f);
        if (HelperFunctions.FastApproximately(this.transform.localEulerAngles.y, newRotation, 4f))
        {
            rotating = false;
            OpenFreshCapsule();
        }
    }
    #endregion

    public void ClearListsForRespawn()
    {
        foreach (GameObject go in spawnedGifts)
        {
            //GameManager.Instance.AddScore(-10);
            Destroy(go);
        }
        chosenLocations.Clear();
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
    void CalculateVisibleTextPosition()
    {
        //Not needed with the timetext array. These have static positions

        //Vector3 textToCapsule = this.transform.position - timeText.transform.parent.position;
        //float textDistance = textToCapsule.magnitude;
        //Vector3 playerToCapsule = this.transform.position - GameManager.Instance.player.transform.position;
        //timeText.transform.parent.position = this.transform.position + (playerToCapsule.normalized * textDistance);
        //timeText.transform.parent.LookAt(GameManager.Instance.player.transform);
        //timeText.transform.parent.localEulerAngles += new Vector3(0f, 180f, 0f);
        //timeText.transform.parent.position = new Vector3(timeText.transform.parent.position.x, textStartingY, timeText.transform.parent.position.z);
    }

    private void CalculateNearestDoors()
    {
        if (openDoors.Count <= 0) // No doors are open, open closest doors 
        {
            List<CapsuleAnimations> clonedAnimations = new List<CapsuleAnimations>(animationHandlers);
            List<GameObject> clonedPillars = new List<GameObject>(connectingWalls);

            for (int i = 0; i < clonedAnimations.Count; i++)
            {
                distancesDoors.Add((clonedAnimations[i].distancePivot.transform.position - GameManager.Instance.player.transform.position).sqrMagnitude);
            }

            for (int i = 0; i < clonedPillars.Count; i++)
            {
                distancesPillars.Add((clonedPillars[i].transform.position - GameManager.Instance.player.transform.position).sqrMagnitude);
            }

            for (int i = 0; i < doorsToOpenAtOnce; i++)
            {
                float minimum = distancesDoors.Min();

                int index = distancesDoors.IndexOf(minimum);
                clonedAnimations[index].Animate(true);
                openDoors.Add(clonedAnimations[index]);
                clonedAnimations.RemoveAt(index);
                distancesDoors.RemoveAt(index);
            }

            if (doorsToOpenAtOnce > 1) // Also remove the pillar connecting these doors
            {
                for (int i = 0; i < doorsToOpenAtOnce; i++)
                {
                    float minimum = distancesPillars.Min();
                    int index = distancesPillars.IndexOf(minimum);
                    connectingWalls[index].SetActive(false);
                    clonedPillars.RemoveAt(index);
                    distancesPillars.RemoveAt(index);
                }
            }
        }
        else // Close open doors
        {
            if (openDoors.Count > 1)
            {
                Invoke("StopMusic", 5.5f);
            }

            for (int i = 0; i < openDoors.Count; i++)
            {
                openDoors[i].Animate(false);
            }

            openDoors.Clear();

            for (int i = 0; i < connectingWalls.Count; i++)
            {
                if (!connectingWalls[i].activeSelf)
                    connectingWalls[i].SetActive(true);
            }
        }

        distancesDoors.Clear();
        distancesPillars.Clear();
    }

    #region Music
    private IEnumerator FadeInVolume()
    {
        jukeBox.volume += audioLerpSpeed;
        yield return new WaitForSeconds(0.01f);

        if (jukeBox.volume < 0.6f)
        {
            StartCoroutine(FadeInVolume());
        }
    }

    private void PlayMusic()
    {
        if (jukeBox.isPlaying)
        {
            return;

        }
        jukeBox.volume = 0.0f;
        StartCoroutine(FadeInVolume());
        jukeBox.Play();
    }


    private IEnumerator StopMusic()
    {
        jukeBox.volume -= audioLerpSpeed;
        yield return new WaitForSeconds(0.01f);

        if (jukeBox.volume <= 0f)
        {
            jukeBox.Stop();
            yield return new WaitForEndOfFrame();
        }
        else if (jukeBox.volume > 0f)
        {
            StartCoroutine(StopMusic());
        }
    }
    #endregion
}
