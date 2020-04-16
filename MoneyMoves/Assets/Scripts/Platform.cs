using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool isMoving;
    [SerializeField] Vector3 target;
    public float speed, rotationSpeed;
    public GameObject dockingSpotA, dockingSpotB, trackVisual, RotatorObjectBecauseMathIsTooHardForMe, arrow;
    public Vector3 woop;
    int currentIndex = 0;
    GameObject visualParent;
    List<GameObject> arrows = new List<GameObject>();
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
        visualParent = new GameObject();
        CreateVisuals();
        ChangeStateToSplit();
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
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, RotatorObjectBecauseMathIsTooHardForMe.transform.rotation, rotationSpeed * Time.deltaTime);
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        }
    }

    public void ChangeStateToSplit()
    {
        State = MovementState.MovingToSplit;
    }

    public void ChangeStateToA()
    {
        State = MovementState.MovingToA;
    }

    public void ChangeStateToB()
    {
        State = MovementState.MovingToB;
    }
    public void MoveToSplit()
    {
        target = Tracks.OriginToSplit[currentIndex];
        if (this.transform.position == target)
        {
            if (target == Tracks.OriginToSplit[Tracks.OriginToSplit.Count - 1])
            {
                currentIndex = 0;
                State = MovementState.Idle;
                GameManager.Instance.ActivateLaserPointer();
            }
            currentIndex++;
        }
    }

    public void MoveToDestinationA()
    {
        State = MovementState.MovingToA;
        target = Tracks.SplitToA[currentIndex];
        if (this.transform.position == target)
        {
            if (target == Tracks.SplitToA[Tracks.SplitToA.Count - 1])
            {
                currentIndex = 0;
                State = MovementState.Idle;
                dockingSpotA.GetComponent<DockingSpot>().capsule.OpenUp();
                ClearVisuals();
            }
            currentIndex++;
        }
    }

    public void MoveToDestinationB()
    {
        State = MovementState.MovingToB;
        target = Tracks.SplitToB[currentIndex];
        if (this.transform.position == target)
        {
            if (target == Tracks.SplitToB[Tracks.SplitToB.Count - 1])
            {
                currentIndex = 0;
                State = MovementState.Idle;
                dockingSpotB.GetComponent<DockingSpot>().capsule.OpenUp();
                ClearVisuals();
            }
            currentIndex++;
        }
    }

    public void ClearVisuals()
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
        CreateTheNiceCursors();
    }

    void CreateTheNiceCursors()
    {
        GameObject arrowA = Instantiate(arrow, Tracks.SplitToA[7], Quaternion.identity);
        GameObject arrowB = Instantiate(arrow, Tracks.SplitToB[7], Quaternion.identity);
        arrowA.name = "arrowA";
        arrowB.name = "arrowB";
        arrowA.transform.LookAt(this.transform.position);
        arrowB.transform.LookAt(this.transform.position);
        arrowA.transform.position += new Vector3(0, 3f, 0);
        arrowB.transform.position += new Vector3(0, 3f, 0);
        arrowA.transform.localEulerAngles += new Vector3(0, 180f, 0);

        arrows.Clear();
        arrows.Add(arrowA);
        arrows.Add(arrowB);
    }
}
