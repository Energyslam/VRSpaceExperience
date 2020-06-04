using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class HighscoreContainer : MonoBehaviour
{
    [SerializeField] int visibleEntries;
    [SerializeField] Transform topOfContainer;
    [SerializeField] Transform bottomOfContainer;
    [SerializeField] GameObject entry;
    List<Vector3> textPositions = new List<Vector3>();
    public List<HighscoreEntryData> entries = new List<HighscoreEntryData>();
    [SerializeField] TextMeshProUGUI nameField;
    string recentName = "";
    [SerializeField] List<GameObject> objectsToTurnOff = new List<GameObject>();
    public bool rewriteDefaultsToPref;
    void Start()
    {
        if (rewriteDefaultsToPref)
        {
            if (PlayerPrefs.HasKey("highscores"))
            {
                PlayerPrefs.DeleteKey("highscores");
            }
        }
        if (!PlayerPrefs.HasKey("highscores"))
        {
            WriteDefaultsToPrefs();
        }
    }

    string Recapitalize(string str)
    {
        if (str.Length <= 0)
        {
            return null;
        }
        string tmp = "";
        tmp = str.Substring(0, 1).ToUpper();
        if (str.Length > 1)
        {
            tmp += str.Substring(1, str.Length - 1).ToLower();
        }
        return tmp;
    }

    public void CreateHighscores()
    {
        recentName = Recapitalize(nameField.text);
        Highscores highscores = JsonUtility.FromJson<Highscores>(PlayerPrefs.GetString("highscores"));
        HighscoreEntry recentHighscoreEntry = new HighscoreEntry { score = GameManager.Instance.score, name = recentName };
        highscores.highscores.Add(recentHighscoreEntry);
        List<HighscoreEntry> tmp = highscores.highscores.OrderByDescending(o => o.score).ToList();
        highscores.highscores = tmp;
        WriteNewHighscoresEntry(highscores);
        int totalEntries = 0;
        foreach(HighscoreEntry entry in highscores.highscores)
        {
            totalEntries++;
        }
        Debug.Log("total entries: " + totalEntries);
        Vector3 direction = bottomOfContainer.position - topOfContainer.position;
        for (int i = 0; i < visibleEntries; i++)
        {

            if (i < highscores.highscores.Count)
            {
                GameObject entryGO = Instantiate(entry, topOfContainer.position + (direction / visibleEntries) * i, transform.rotation, transform);
                HighscoreEntryData entryData = entryGO.GetComponent<HighscoreEntryData>();

                if (i == visibleEntries - 1)
                {

                    if (GameManager.Instance.score <= highscores.highscores[i].score)
                    {
                        entryData.SetName(recentName);
                        entryData.SetScore(GameManager.Instance.score);
                        entryData.SetPosition(tmp.IndexOf(recentHighscoreEntry) + 1);
                    }
                    else
                    {
                        entryData.SetName(highscores.highscores[i].name);
                        entryData.SetScore(highscores.highscores[i].score);
                        entryData.SetPosition(i + 1);
                        entries.Add(entryData);
                    }
                }
                else
                {
                    entryData.SetName(highscores.highscores[i].name);
                    entryData.SetScore(highscores.highscores[i].score);
                    entryData.SetPosition(i + 1);
                    entries.Add(entryData);
            }
        }
        }
    }

    void WriteNewHighscoresEntry(Highscores highscores)
    {
        string highscoresJSON = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscores", highscoresJSON);
        PlayerPrefs.Save();
    }


    public void TurnOffObjects()
    {
        foreach(GameObject go in objectsToTurnOff)
        {
            go.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TurnOffObjects();
            CreateHighscores();
        }
    }
    void WriteDefaultsToPrefs()
    {

        List<HighscoreEntry> tempEntries = new List<HighscoreEntry>();
        tempEntries.Add(new HighscoreEntry { name = "Max", score = 500 });
        tempEntries.Add(new HighscoreEntry { name = "Tycho", score = 2 });
        tempEntries.Add(new HighscoreEntry { name = "Wasili", score = 101 });
        tempEntries.Add(new HighscoreEntry { name = "Dzeneta", score = -73 });
        tempEntries.Add(new HighscoreEntry { name = "Sander", score = 7 });
        tempEntries.Add(new HighscoreEntry { name = "Donut", score = 45 });
        tempEntries.Add(new HighscoreEntry { name = "Lena", score = 25 });
        tempEntries.Add(new HighscoreEntry { name = "Frank", score = 65 });
        tempEntries.Add(new HighscoreEntry { name = "Patat", score = -34 });
        tempEntries.Add(new HighscoreEntry { name = "Sergio", score = 54 });
        tempEntries.Add(new HighscoreEntry { name = "Margrietje", score = -67 });
        tempEntries.Add(new HighscoreEntry { name = "Inna", score = -80 });
        tempEntries.Add(new HighscoreEntry { name = "Fangirl", score = -46 });
        tempEntries.Add(new HighscoreEntry { name = "Energyslam", score = -45 });
        Highscores highscores = new Highscores { highscores = tempEntries };
        string highscoresJSON = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscores", highscoresJSON);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscores;
    }

    [Serializable]
    private class HighscoreEntry
    {
        public string name;
        public int score;
    }

}
