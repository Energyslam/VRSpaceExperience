using System;
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
        this.transform.LookAt(target.transform);
    }

    public void Hit()
    {
        minigame.UpdateScore(this.gameSide);
        Destroy(this.gameObject);
    }
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
        if (this.transform.position == target.position)
        {
            Destroy(this.gameObject);
        }
    }

    class DefaultBehaviour : MonoBehaviour
    {
        public Transform target;
        public float speed;
    }

    class BobbingBehaviour : DefaultBehaviour
    {
        public float sinSpeed;
        public float sinMagnitude;
        Vector3 pos = Vector3.zero;
        void Start()
        {
            pos = this.transform.position;
            this.transform.LookAt(target.transform);
        }
        void Update()
        {
            pos = Vector3.MoveTowards(pos, target.position, speed * Time.deltaTime);
            this.transform.position = pos  + transform.up * (float)Math.Sin(Time.time * sinSpeed) * sinMagnitude;
            if (pos == target.position)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
