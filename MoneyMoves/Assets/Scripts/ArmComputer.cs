using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArmComputer : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI tmpText;
    void Start()
    {
        tmpText = this.GetComponent<TextMeshProUGUI>();
        tmpText.text = "Score: " + GameManager.Instance.score + "\nLives: " + GameManager.Instance.lives;
        GameManager.Instance.scoreAndLifeText = tmpText;
    }
}
