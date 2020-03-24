using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleMovement : MonoBehaviour, IObservable
{
    private int? dockedAt;

    public enum State { IDLE, MOVINGTODOCK, ATDOCK, LEAVINGDOCK };
    private State state;

    private void Subscribe()
    {
        CapsuleManager._instance.AddObservable(gameObject);
    }

    // Start is called before the first frame update
    void Awake()
    {
        Subscribe();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.IDLE:
                break;
            case State.MOVINGTODOCK:
                break;
            case State.LEAVINGDOCK:
                break;
            case State.ATDOCK:
                break;
        }
    }

    private void MoveTowardsDock()
    {
        transform.position = Vector3.MoveTowards()
    }

    public void GoToPlayer(int? dock)
    {
        dockedAt = dock;
        state = State.MOVINGTODOCK;
        Invoke("Leave", 10f);
    }

    public void Leave()
    {
        CapsuleManager._instance.RemoveObservable(gameObject);
        CapsuleManager._instance.LeftPlayer(gameObject, dockedAt);
        dockedAt = null;
        state = State.LEAVINGDOCK;
    }
}
