using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool isMoving;
    [SerializeField] Vector3 target;
    public float speed, rotationSpeed;
    float maxSpeed;
    public GameObject dockingSpotA, dockingSpotB, trackVisual, RotatorObjectBecauseMathIsTooHardForMe, arrow;
    public Vector3 woop;
    public int currentIndex = 0;
    GameObject visualParent;
    List<GameObject> arrows = new List<GameObject>();
    int maxSpeedIndex;
    public enum MovementState
    {
        Idle,
        MovingToSplit,
        MovingToA,
        MovingToB,
        Rotating
    }
    public MovementState State = MovementState.Idle;

    public void RemoveArrowColliders()
    {
        foreach(GameObject go in arrows)
        {
            go.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Start()
    {
        maxSpeed = speed;
        visualParent = new GameObject();
        CreateVisuals();
        ChangeStateToSplit();
    }

    void SpeedHandler()
    {
        //return; //MINGAME DEBUG
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
    }

    public void ChangeStateToB()
    {
        currentIndex = 0;
        State = MovementState.MovingToB;
        maxSpeedIndex = Tracks.SplitToA.Count - 1;
        rotationSpeed = 1f;
    }
    void MoveToSplit()
    {
        target = Tracks.OriginToSplit[currentIndex];
        if (StaticCapsule.FastVectorApproximately(this.transform.position, target, 0.1f))
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
        if (StaticCapsule.FastVectorApproximately(this.transform.position, target, 0.1f))
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
        if (StaticCapsule.FastVectorApproximately(this.transform.position, target, 0.1f))
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
        foreach(GameObject go in arrows)
        {
            Destroy(go);
        }
    }
    public void CreateVisuals()
    {
        //GameObject splittoaparent = new GameObject();
        //splittoaparent.name = "SplitToA";
        //GameObject splittobparent = new GameObject();
        //splittobparent.name = "SplitToB";
        //GameObject origintosplitparent = new GameObject();
        //origintosplitparent.name = "OriginToSplit";

        Tracks.GenerateSplitTracks(dockingSpotA.transform.position, dockingSpotB.transform.position, this.transform.position);

        for (int i = 0; i < Tracks.SplitToA.Count; i++)
        {
            GameObject visualGO = Instantiate(trackVisual, Tracks.SplitToA[i], Quaternion.identity, visualParent.transform);
            if (i + 1 < Tracks.SplitToA.Count)
            {
                visualGO.transform.LookAt(Tracks.SplitToA[i + 1]);
            }
        }

        for (int i = 0; i < Tracks.SplitToB.Count; i++)
        {
            GameObject visualGO = Instantiate(trackVisual, Tracks.SplitToB[i], Quaternion.identity, visualParent.transform);
            if (i + 1 < Tracks.SplitToB.Count)
            {
                visualGO.transform.LookAt(Tracks.SplitToB[i + 1]);
            }
        }

        for (int i = 0; i < Tracks.OriginToSplit.Count; i++)
        {
            GameObject visualGO = Instantiate(trackVisual, Tracks.OriginToSplit[i], Quaternion.identity, visualParent.transform);
            if (i + 1 < Tracks.OriginToSplit.Count)
            {
                visualGO.transform.LookAt(Tracks.OriginToSplit[i + 1]);
            }
        }
        //CreateTheNiceCursors();
    }

    void CreateTheNiceCursors()
    {
        GameObject arrowA = Instantiate(arrow, Tracks.SplitToA[Tracks.SplitToA.Count / 4], Quaternion.identity);
        GameObject arrowB = Instantiate(arrow, Tracks.SplitToB[Tracks.SplitToB.Count / 4], Quaternion.identity);
        arrowA.name = "arrowA";
        arrowB.name = "arrowB";
        arrowA.transform.LookAt(this.transform.position);
        arrowB.transform.LookAt(this.transform.position);
        arrowA.transform.position += new Vector3(0, 3f, 0);
        arrowB.transform.position += new Vector3(0, 3f, 0);
        arrowA.transform.localEulerAngles += new Vector3(0, 180f, 0);
        arrowA.transform.localEulerAngles = new Vector3(arrowA.transform.localEulerAngles.x * -1f, arrowA.transform.localEulerAngles.y, arrowA.transform.localEulerAngles.z);

        arrows.Clear();
        arrows.Add(arrowA);
        arrows.Add(arrowB);
    }
}
