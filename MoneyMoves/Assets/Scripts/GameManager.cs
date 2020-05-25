using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    [SerializeField] WhacASphereVariables variables;
    [SerializeField] GameObject deliverPoint;
    [SerializeField] GameObject destinationsParent;
    [SerializeField] GameObject capsulesParent;
    public GameObject minigame;
    public GameObject cam;
    public int timesToOpenCapsules;
    public int totalOpenTime;
    public bool rotateText;
    public bool hasShownInstruction;
    public float respawnWaitTime;

    public Platform platform;

    [SerializeField] SteamVR_LaserPointer laserpointer;
    public List<GameObject> destinations = new List<GameObject>();
    public List<PlaytestCapsule> capsules = new List<PlaytestCapsule>();
    List<PlaytestCapsule> capsulesThatHaveBeenMovedAlready = new List<PlaytestCapsule>();
    public GameObject DeliverPoint { get { return deliverPoint; } }
    public GameObject player;
    public int priority = 20;
    int higherPrio = 21;

    public TextMeshProUGUI scoreAndLifeText;

    public int lives = 3;
    public int score = 0;

    public int sentCapsuleAmount = 0;
    public int difficultyRaiser = 2;
    public int currentDifficulty = 1;
    public int difficultyMax = 3;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        foreach (Transform t in destinationsParent.transform)
        {
            destinations.Add(t.gameObject);
        }
        foreach(Transform t in capsulesParent.transform)
        {
            capsules.Add(t.GetComponentInChildren<PlaytestCapsule>());
        }
        variables.SetHumanDefaults();
        //laserpointer.PointerClick += PointerClick;
    }

    public void ActivateLaserPointer()
    {
        laserpointer.enabled = true;
        laserpointer.ActivatePointer();
    }

    public void DeactivateLaserPointer()
    {
        laserpointer.DeactivatePointer();
        laserpointer.enabled = false;
    }
    public int AssignHighPriority()
    {
        return UnityEngine.Random.Range(0, 50);
    }
    public int AssignLowPriority()
    {
        return UnityEngine.Random.Range(51, 99);
    }
    private void Start()
    {
        difficultyMax = destinations.Count;
        currentDifficulty = 1;
        //StartCoroutine(WaitBeforeMoving());
    }
    IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForEndOfFrame();
        MoveACapsule();
    }

    public void MoveACapsule()
    {
        for(int i = 0; i < currentDifficulty; i++)
        {
            StartCoroutine(SendCapsule());
        }
        ManageDifficulty();
    }

    IEnumerator SendCapsule()
    {
        if (destinations.Count > 0)
        {
            foreach (PlaytestCapsule capsule in capsules)
            {
                if (!capsulesThatHaveBeenMovedAlready.Contains(capsule))
                {
                    if (capsule.available)
                    {
                        capsule.MoveToDestination();
                        capsulesThatHaveBeenMovedAlready.Add(capsule);
                        yield break;
                    }
                }
            }
            if (capsulesThatHaveBeenMovedAlready.Count > 0)
            {
                capsulesThatHaveBeenMovedAlready.Clear();
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(SendCapsule());
        }
    }

    void ManageDifficulty()
    {
        sentCapsuleAmount++;
        if (sentCapsuleAmount >= difficultyRaiser)
        {
            currentDifficulty++;
            sentCapsuleAmount = 0;
            if (currentDifficulty > difficultyMax)
            {
                currentDifficulty = difficultyMax;
            }
        }
    }
    public void AddScore(int scoreValue)
    {
        score += scoreValue;
        UpdateText();
        //TODO: CHeck for death later
    }

    public void UpdateText()
    {
        if (scoreAndLifeText != null)
        scoreAndLifeText.text = "Score: " + score + "\nLives: " + lives;
    }
    //public void PointerClick(object sender, PointerEventArgs e)
    //{
    //    if (e.target.name == "arrowA")
    //    {
    //        platform.ChangeStateToA();
    //        DeactivateLaserPointer();
    //    }
    //    else if (e.target.name == "arrowB")
    //    {
    //        platform.ChangeStateToB();
    //        DeactivateLaserPointer();
    //    }
    //}
    #region Vibration
    [Header("Vibration settings")]
    public SteamVR_Action_Vibration hapticAction;
    public float duration, frequency;
    [Range(0.0f, 1.0f)]
    public float amplitude;
    /// <summary>
    /// Default vibration
    /// </summary>
    public void VibrateControllers()
    {
        try
        {
            hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.LeftHand);
            hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.RightHand);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            Debug.Log("Hands not found");
        }

    }
    /// <summary>
    /// Customizable vibration
    /// </summary>
    /// <param name="duration">Duration of vibration in seconds</param>
    /// <param name="frequency">Frequency of vibrations</param>
    /// <param name="amplitude">Amplitude of the vibration between 0.0f and 1.0f</param>
    public void VibrateControllers(float duration, float frequency, float amplitude)
    {
        try
        {
            hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.LeftHand);
            hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.RightHand);
        }catch(Exception e)
        {
            Debug.LogError(e.Message);
            Debug.Log("Hands not found");
        }
    }
    #endregion
}
