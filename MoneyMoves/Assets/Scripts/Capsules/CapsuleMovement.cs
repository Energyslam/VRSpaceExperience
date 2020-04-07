using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CapsuleMovement : MonoBehaviour, IObservable
{
    private int? dockedAt;

    [SerializeField]
    private uint speed;

    [SerializeField]
    List<float> distances = new List<float>();

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private int doorsToOpenAtOnce = 1;

    [SerializeField]
    private List<CapsuleAnimations> animationHandlers;

    private Vector3 originalPosition, direction;

    public enum State { IDLE, MOVINGTODOCK, ATDOCK, LEAVINGDOCK };

    private State state = State.IDLE;

    private bool isOpen = false;

    // Adds itself to lists in CapsuleManager singleton
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
        // Decide how to move around
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
                Docked();
                break;
        }

        // Rotate towards target location
        transform.forward = direction;
    }

    // Move capsule from start position to the dock
    private void MoveTowardsDock()
    {
        if (transform.position == CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.position && state == State.MOVINGTODOCK)
        {
            state = State.ATDOCK;
            Invoke("CloseDoors", Random.Range(5, 15));
            CalculateNearestDoors(true); // Animation plays that opens the capsule
        }

        transform.position = Vector3.MoveTowards(transform.position, CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.position, speed * Time.deltaTime);
        direction = CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.position - transform.position;
    }

    // Moves capsule away from dock back to start position
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

    private void CalculateNearestDoors(bool hasToOpen)
    {
        for (int i = 0; i < animationHandlers.Count; i++)
        {
            distances.Add(Vector3.Distance(player.transform.position, animationHandlers[i].transform.position));
        }

        List<float> smallestDistances = new List<float>();

        for (int i = 0; i <= doorsToOpenAtOnce - 1; i++)
        {
            smallestDistances.Add(1000.0f);
        }

        for (int i = 0; i < distances.Count; i++)
        {
            Foo: 
            for (int j = 0; j < smallestDistances.Count; j++)
            {
                if (distances[i] < smallestDistances[j])
                {
                    smallestDistances[j] = distances[i];
                    animationHandlers[i].Animate(hasToOpen);
                    j++;
                    goto Foo;
                }
                j++;
            }
            i++;
        }

       // animationHandlers.OrderBy();
    }

    // Makes capsule look towards the middle of the player platform
    private void Docked()
    {
        // Look to middle
        direction = CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.forward;
        direction = new Vector3(direction.x, 0.0f, direction.z);
    }

    // Change state to make capsule move to dock. Called from CapsuleManager
    public void GoToPlayer(int? dock)
    {
        dockedAt = dock;
        originalPosition = transform.position;
        state = State.MOVINGTODOCK;
    }

    // Capsule left the dock, called with an Invoke upon entering dock or when all content has been grabbed
    public void LeaveDock()
    {
        state = State.LEAVINGDOCK;
        CapsuleManager._instance.RemoveObservable(gameObject);
        CapsuleManager._instance.CapsuleLeftDock(gameObject, dockedAt);
        dockedAt = null;
    }

    private void CloseDoors()
    {
        CalculateNearestDoors(false); // Animation plays that closes the capsule
        Invoke("LeaveDock", 1.0f);
    }
}
