using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public List<GameObject> capsuleTypes = new List<GameObject>();
    public GameObject leftCapsuleParent, rightCapsuleParent;
    public StaticCapsule leftCapsule, rightCapsule;
    public DockingSpot leftDockingSpot, rightDockingSpot;

    private void Awake()
    {
        GameObject leftObj, rightObj;
        leftObj = LightBaker._instance.GetRandomCapsule();
        LightBaker._instance.MoveCapsule(leftObj);
        rightObj = LightBaker._instance.GetRandomCapsule();
        LightBaker._instance.MoveCapsule(rightObj);

        leftCapsule = leftCapsuleParent.GetComponent<CapsuleParent>().InstantiateCapsule(leftObj);
        rightCapsule = rightCapsuleParent.GetComponent<CapsuleParent>().InstantiateCapsule(rightObj);
        leftDockingSpot.capsule = leftCapsule;
        rightDockingSpot.capsule = rightCapsule;
    }
}
