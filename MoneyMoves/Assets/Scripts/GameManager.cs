using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public TextMeshProUGUI scoreAndLifeText;

    public int lives = 3;
    public int score = 0;

    [Header("Vibration settings")]
    public SteamVR_Action_Vibration hapticAction;
    public float duration, frequency;
    [Range(0.0f, 1.0f)]
    public float amplitude;

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
    }

    public void AddScore(int scoreValue)
    {
        score += scoreValue;
        UpdateText();
        //TODO: CHeck for death later
    }

    public void UpdateText()
    {
        scoreAndLifeText.text = "Score: " + score + "\nLives: " + lives;
    }
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
            Debug.Log("Hands not found");
        }
    }
}
