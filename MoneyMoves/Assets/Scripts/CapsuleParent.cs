using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CapsuleParent : MonoBehaviour
{
    public GameObject dockingSpot;
    public GameObject timetextHolder;
    public StaticCapsule InstantiateCapsule(GameObject capsule)
    {
        GameObject capsuleGo = Instantiate(capsule, transform);
        capsuleGo.GetComponent<StaticCapsule>().dockingSpot = dockingSpot;
        capsuleGo.GetComponent<StaticCapsule>().timetextHolder = timetextHolder;
        return capsuleGo.GetComponent<StaticCapsule>();
    }
}
