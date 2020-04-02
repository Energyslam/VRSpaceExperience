﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBehaviour : ICollisionBehaviour
{
    [SerializeField] bool grabbable;
    [SerializeField] float speed = 1f;
    [SerializeField] GameObject fireworkGO, grabber, deliverPoint;
    bool canMove;
    void Start()
    {
        deliverPoint = GameManager.Instance.DeliverPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, deliverPoint.transform.position, speed * Time.deltaTime);
        }
        if (this.transform.position == deliverPoint.transform.position)
        {
            canMove = false;
            grabber.SetActive(false);
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
    }


    public void StartMoving()
    {
        canMove = true;
    }
    public override void SolveCollision()
    {
        if (!grabbable)
        {
            fireworkGO.SetActive(true);
            fireworkGO.transform.parent = null;
            Destroy(this.gameObject);
            GameManager.Instance.AddScore(10);
        }
        else if (grabbable)
        {
            grabber.SetActive(true);
        }
    }
}
