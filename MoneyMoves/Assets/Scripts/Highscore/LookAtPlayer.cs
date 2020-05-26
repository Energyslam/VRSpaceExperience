using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public bool followsPlayer;
    public GameObject target;
    public float speed;
    public bool isMoving;

    public Vector3 axis;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.LookAt(GameManager.Instance.player.transform.position);
    }
    private void Update()
    {
        if (isMoving)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
            this.transform.RotateAround(target.transform.position, axis, angle);
        }

        if (followsPlayer)
        {
            this.transform.LookAt(GameManager.Instance.player.transform.position);
        }
    }
}
