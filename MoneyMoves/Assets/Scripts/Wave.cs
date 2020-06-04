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
        List<GameObject> tempList = capsuleTypes;
        GameObject randomCapsuleLeft = tempList[Random.Range(0, capsuleTypes.Count)];
        tempList.Remove(randomCapsuleLeft);
        GameObject randomCapsuleRight = tempList[Random.Range(0, capsuleTypes.Count)];

        leftCapsule = leftCapsuleParent.GetComponent<CapsuleParent>().InstantiateCapsule(randomCapsuleLeft);
        rightCapsule = rightCapsuleParent.GetComponent<CapsuleParent>().InstantiateCapsule(randomCapsuleRight);
        leftDockingSpot.capsule = leftCapsule;
        rightDockingSpot.capsule = rightCapsule;
    }
}
