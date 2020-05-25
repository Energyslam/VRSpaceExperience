using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreContainer : MonoBehaviour
{
    [SerializeField] int visibleEntries;
    [SerializeField] Transform topOfContainer;
    [SerializeField] Transform bottomOfContainer;
    [SerializeField] GameObject gizmo;
    List<Vector3> textPositions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        LookAtPlayer();
        for (int i = 0; i < visibleEntries; i++)
        {
            Vector3 direction = bottomOfContainer.position - topOfContainer.position;
            GameObject highscoreEntry = Instantiate(gizmo, topOfContainer.position + (direction / visibleEntries) * i, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LookAtPlayer()
    {
        this.transform.LookAt(GameManager.Instance.player.transform.position);
        this.transform.localEulerAngles = new Vector3(90, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z);
    }
}
