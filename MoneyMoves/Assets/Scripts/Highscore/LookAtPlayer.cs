using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public bool followsPlayer;
    public Vector3 target;
    public float speed;
    public bool isMoving;
    public bool isRotating;
    public Vector3 axis;
    public float angle;
    public GameObject followObject;
    bool hasArrived;
    public float resetSpeed;
    public Vector3 euler;
    [SerializeField] GameObject cheatPrevention;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.LookAt(GameManager.Instance.player.transform.position);
    }
    private void Update()
    {
        if (isRotating)
        {
            if (hasArrived)
            {
                followObject.transform.LookAt(GameManager.Instance.player.transform.position);
                if (cheatPrevention.activeInHierarchy)
                {
                    cheatPrevention.SetActive(false);
                }
                //Quaternion targetRotation = Quaternion.Euler(euler);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, followObject.transform.rotation, resetSpeed * Time.deltaTime);
                if (this.transform.rotation == followObject.transform.rotation)
                {
                    isRotating = false;
                }
            }
            else
            {
                this.transform.RotateAround(followObject.transform.position, axis, angle);
            }
        }
        if (isMoving)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            if (this.transform.position == target)
            {
                isMoving = false;
                hasArrived = true;
            }
        }

        if (followsPlayer)
        {
            this.transform.LookAt(GameManager.Instance.player.transform.position);
        }
    }
}
