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
        capsule.transform.parent = transform;
        capsule.transform.localPosition = Vector3.zero;
        capsule.GetComponent<GridManager>().insideContext.SetActive(false);
        capsule.GetComponent<StaticCapsule>().dockingSpot = dockingSpot;
        capsule.GetComponent<StaticCapsule>().timetextHolder = timetextHolder;
        return capsule.GetComponent<StaticCapsule>();
    }
}
