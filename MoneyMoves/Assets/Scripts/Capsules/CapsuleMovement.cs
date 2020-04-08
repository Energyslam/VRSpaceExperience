using System.Collections;
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

    [SerializeField]
    private List<CapsuleAnimations> openDoors = new List<CapsuleAnimations>();

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
                direction = transform.forward;
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
            StartCoroutine(CalculateNearestDoors());
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

    private IEnumerator CalculateNearestDoors()
    {
        yield return new WaitForSeconds(0.1f);
        if (openDoors.Count <= 0) // No doors are open, open closest doors 
        {
            List<CapsuleAnimations> clonedAnimations = new List<CapsuleAnimations>(animationHandlers);

            for (int i = 0; i < clonedAnimations.Count; i++)
            {
                distances.Add((clonedAnimations[i].distancePivot.transform.position - player.transform.position).sqrMagnitude);
            }

            for (int i = 0; i < doorsToOpenAtOnce; i++)
            {
                float minimum = distances.Min();

                int index = distances.IndexOf(minimum);
                clonedAnimations[index].Animate(true);
                openDoors.Add(clonedAnimations[index]);
                clonedAnimations.RemoveAt(index);
                distances.RemoveAt(index);
            }
        }
        else
        {
            for (int i = 0; i < openDoors.Count; i++)
            {
                openDoors[i].Animate(false);
            }
            openDoors.Clear();
        }
        distances.Clear();
    }

    // Makes capsule look towards the middle of the player platform
    private void Docked()
    {
        // Look to middle, without rotating the y axis
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
        StartCoroutine(CalculateNearestDoors()); // Animation plays that closes the capsule
        Invoke("LeaveDock", 1.0f);
    }
}
