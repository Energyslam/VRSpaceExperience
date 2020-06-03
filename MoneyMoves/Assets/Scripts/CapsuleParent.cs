using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CapsuleParent : MonoBehaviour
{
    //public List<GameObject> capsules = new List<GameObject>();
    public GameObject dockingSpot;
    public TextMeshProUGUI timeText;
    //public GameObject capsule;
    public StaticCapsule InstantiateCapsule(GameObject capsule)
    {
        GameObject capsuleGo = Instantiate(capsule, transform);
        capsuleGo.GetComponent<StaticCapsule>().dockingSpot = dockingSpot;
        capsuleGo.GetComponent<StaticCapsule>().timeText = timeText;
        return capsuleGo.GetComponent<StaticCapsule>();
    }
}
