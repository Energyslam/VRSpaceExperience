using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftGrabber : MonoBehaviour
{
    [SerializeField] GameObject topPosition;
    [SerializeField] float speed;
    bool hasMoved;

    private void Update()
    {
        if (hasMoved) return;
        this.transform.position = Vector3.MoveTowards(this.transform.position, topPosition.transform.position, speed * Time.deltaTime);
        if (this.transform.position == topPosition.transform.position)
        {
            GetComponentInParent<GiftBehaviour>().StartMoving();
            hasMoved = true;
        }

    }
}
