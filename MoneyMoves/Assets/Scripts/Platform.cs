using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    [SerializeField] Vector3 target;

    Quaternion targetRotation;

    public GameObject dockingSpotA;
    public GameObject dockingSpotB;
    public GameObject trackVisual;
    public GameObject RotatorObjectBecauseMathIsTooHardForMe;
    GameObject visualParent;

    List<GameObject> splitToATracks = new List<GameObject>();
    List<GameObject> splitToBTracks = new List<GameObject>();

    public int currentIndex = 0;
    int maxSpeedIndex;

    public float resetSpeed;
    public float speed; 
    public float rotationSpeed;
    float maxSpeed;

    public bool immediateStart;
    public bool overrideSpeed;
    bool isMoving;

    public enum MovementState
    {
        Idle,
        MovingToSplit,
        MovingToA,
        MovingToB,
        Rotating
    }
    public MovementState State = MovementState.Idle;

    private void Start()
    {
        maxSpeed = speed;
        visualParent = new GameObject();
        CreateVisuals();
        if (immediateStart)
        {
            ChangeStateToSplit();
        }
    }

    void SpeedHandler()
    {
        if (overrideSpeed)
        {
            return;
        }
        if (currentIndex > maxSpeedIndex - 10)
        {
            int distanceToSlowDown = maxSpeedIndex - (maxSpeedIndex - 10);
            int currentDistanceTo0 = maxSpeedIndex - currentIndex;
            float dividerThingy = (float)currentDistanceTo0 / (float)distanceToSlowDown;
            speed = Mathf.Lerp(1f, 8f, dividerThingy);
        }
        else speed = 8f;
    }
    void Update()
    {
        if (State == MovementState.MovingToSplit)
        {
            MoveToSplit();
        }
        else if (State == MovementState.MovingToA)
        {
            MoveToDestinationA();
        }
        else if (State == MovementState.MovingToB)
        {
            MoveToDestinationB();
        }
        if (State != MovementState.Idle)
        {
            RotatorObjectBecauseMathIsTooHardForMe.transform.LookAt(target);
            //this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, RotatorObjectBecauseMathIsTooHardForMe.transform.rotation, rotationSpeed * Time.deltaTime); // erger als je te snel gaat bij kleine rotaties
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, RotatorObjectBecauseMathIsTooHardForMe.transform.rotation, rotationSpeed * Time.deltaTime);
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        }
        if (State == MovementState.Idle)
        {
            //TODO: slowly go to neutral x and z
            targetRotation = Quaternion.Euler(new Vector3(0, this.transform.localEulerAngles.y, 0));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, resetSpeed * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.blue;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 10;
        Gizmos.DrawRay(transform.position, direction);
    }

    public void ChangeStateToSplit()
    {
        currentIndex = 0;
        State = MovementState.MovingToSplit;
        maxSpeedIndex = Tracks.OriginToSplit.Count - 1;
        rotationSpeed = 2f;
    }

    public void ChangeStateToA()
    {
        currentIndex = 0;
        State = MovementState.MovingToA;
        maxSpeedIndex = Tracks.SplitToA.Count - 1;
        rotationSpeed = 1f;
        dockingSpotA.transform.parent.GetComponentInChildren<GridManager>().enabled = true;
    }

    public void ChangeStateToB()
    {
        currentIndex = 0;
        State = MovementState.MovingToB;
        maxSpeedIndex = Tracks.SplitToA.Count - 1;
        rotationSpeed = 1f;
        dockingSpotB.transform.parent.GetComponentInChildren<GridManager>().enabled = true;
    }
    void MoveToSplit()
    {
        target = Tracks.OriginToSplit[currentIndex];
        if (HelperFunctions.FastVectorApproximately(this.transform.position, target, 0.1f))
        {
            SpeedHandler();
            if (target == Tracks.OriginToSplit[Tracks.OriginToSplit.Count - 1])
            {
                currentIndex = 0;
                State = MovementState.Idle;
                //MINIGAME:
                GameObject minigame = Instantiate(GameManager.Instance.minigame);
                //GameManager.Instance.ActivateLaserPointer();
                return;
            }
            currentIndex++;
        }
    }

    void MoveToDestinationA()
    {
        State = MovementState.MovingToA;
        target = Tracks.SplitToA[currentIndex];
        if (HelperFunctions.FastVectorApproximately(this.transform.position, target, 0.1f))
        {
            SpeedHandler();
            if (target == Tracks.SplitToA[Tracks.SplitToA.Count - 1])
            {
                currentIndex = 0;
                State = MovementState.Idle;
                dockingSpotA.GetComponent<DockingSpot>().capsule.OpenUp();
                ClearVisuals();
                return;
            }
            currentIndex++;
        }
    }

    void MoveToDestinationB()
    {
        State = MovementState.MovingToB;
        target = Tracks.SplitToB[currentIndex];
        if (HelperFunctions.FastVectorApproximately(this.transform.position, target, 0.1f))
        {
            SpeedHandler();
            if (target == Tracks.SplitToB[Tracks.SplitToB.Count - 1])
            {
                currentIndex = 0;
                State = MovementState.Idle;
                dockingSpotB.GetComponent<DockingSpot>().capsule.OpenUp();
                ClearVisuals();
                return;
            }
            currentIndex++;
        }
    }

    void ClearVisuals()
    {
        foreach(Transform t in visualParent.transform)
        {
            Destroy(t.gameObject);
        }
    }
    public void CreateVisuals()
    {
        Tracks.GenerateSplitTracks(dockingSpotA.transform.position, dockingSpotB.transform.position, this.transform.position);

        splitToATracks.Clear();
        splitToBTracks.Clear();

        for (int i = 0; i < Tracks.SplitToA.Count; i++)
        {
            GameObject visualGO = Instantiate(trackVisual, Tracks.SplitToA[i], Quaternion.identity, visualParent.transform);
            if (i + 1 < Tracks.SplitToA.Count)
            {
                visualGO.transform.LookAt(Tracks.SplitToA[i + 1]);
            }
            splitToATracks.Add(visualGO);
        }

        for (int i = 0; i < Tracks.SplitToB.Count; i++)
        {
            GameObject visualGO = Instantiate(trackVisual, Tracks.SplitToB[i], Quaternion.identity, visualParent.transform);
            if (i + 1 < Tracks.SplitToB.Count)
            {
                visualGO.transform.LookAt(Tracks.SplitToB[i + 1]);
            }
            splitToBTracks.Add(visualGO);
        }

        for (int i = 0; i < Tracks.OriginToSplit.Count; i++)
        {
            GameObject visualGO = Instantiate(trackVisual, Tracks.OriginToSplit[i], Quaternion.identity, visualParent.transform);
            if (i + 1 < Tracks.OriginToSplit.Count)
            {
                visualGO.transform.LookAt(Tracks.OriginToSplit[i + 1]);
            }
        }
    }

    public void ClearATrack()
    {
        foreach(GameObject go in splitToATracks)
        {
            go.SetActive(false);
        }
    }

    public void ClearBTrack()
    {
        foreach(GameObject go in splitToBTracks)
        {
            go.SetActive(false);
        }
    }
}
