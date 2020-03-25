using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleMovement : MonoBehaviour, IObservable
{
    private int? dockedAt;

    [SerializeField]
    private uint speed;

    [SerializeField]
    private Vector3 originalPosition, direction;

    public enum State { IDLE, MOVINGTODOCK, ATDOCK, LEAVINGDOCK };

    [SerializeField]
    private State state = State.IDLE;

    public void Subscribe()
    {
        CapsuleManager._instance.AddObservable(gameObject);
    }

    // Start is called before the first frame update
    void Start()
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
                MoveTowardsDock();
                break;
            case State.LEAVINGDOCK:
                MoveAwayFromDock();
                break;
            case State.ATDOCK:
                break;
        }

        // Rotate towards target location
        transform.forward = direction;
    }

    private void MoveTowardsDock()
    {
        if (transform.position == CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.position && state == State.MOVINGTODOCK)
        {
            state = State.ATDOCK;
            Invoke("Leave", Random.Range(1, 3));
        }

        transform.position = Vector3.MoveTowards(transform.position, CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.position, speed * Time.deltaTime);
        direction = CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.position - transform.position;
    }

    private void MoveAwayFromDock()
    {
        if (transform.position == originalPosition && state == State.LEAVINGDOCK)
        {
            state = State.IDLE;
            Subscribe();
        }

        transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
        direction = originalPosition - transform.position;
    }

    public void GoToPlayer(int? dock)
    {
        dockedAt = dock;
        originalPosition = transform.position;
        state = State.MOVINGTODOCK;
    }

    public void Leave()
    {
        state = State.LEAVINGDOCK;
        CapsuleManager._instance.RemoveObservable(gameObject);
        CapsuleManager._instance.LeftPlayer(gameObject, dockedAt);
        dockedAt = null;
    }
}
