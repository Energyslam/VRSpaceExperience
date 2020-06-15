using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] WhacASphereVariables variables;

    public List<GameObject> destinations = new List<GameObject>();

    [SerializeField] GameObject deliverPoint;
    [SerializeField] GameObject destinationsParent;
    [SerializeField] GameObject capsulesParent;
    public GameObject minigame;
    public GameObject cam;
    public GameObject DeliverPoint { get { return deliverPoint; } }
    public GameObject player;
    public GameObject beginScreen;

    public int timesToOpenCapsules;
    public int totalOpenTime;
    public int lives = 3;
    public int score = 0;

    public bool hasShownInstruction;

    public float respawnWaitTime;

    public Platform platform;

    [SerializeField] SteamVR_LaserPointer laserpointer;

    public TextMeshProUGUI scoreAndLifeText;

    [SerializeField] private VolumeProfile defaultProfile;

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
        variables.SetHumanDefaults();
        //laserpointer.PointerClick += PointerClick;
    }

    public void ResetPPX()
    {
        GetComponent<Volume>().profile = defaultProfile;
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

    public void StartGame()
    {
        beginScreen.GetComponent<BeginScreen>().BeginGame();
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
        scoreAndLifeText.text = "Total score: " + score;
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
