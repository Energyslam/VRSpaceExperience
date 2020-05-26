using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreEntryData : MonoBehaviour
{
    public int position;
    public int score;
    public string name;

    [SerializeField] TextMeshProUGUI posText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI nameText;

    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    public void SetName(string name)
    {
        this.name = name;
        nameText.text = name;
    }

    public void SetPosition(int pos)
    {
        this.position = pos;
        posText.text = position.ToString();
    }
}
