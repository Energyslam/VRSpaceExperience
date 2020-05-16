using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTarget : MonoBehaviour
{
    public Transform target;
    public float speed;
    public Minigame minigame;
    public TinyGame.GameSide gameSide;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Hit()
    {
            minigame.UpdateScore(this.gameSide);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (this.transform.position == target.position)
        {
            Destroy(this.gameObject);
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
    }
}
