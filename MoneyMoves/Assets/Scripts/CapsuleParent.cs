using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CapsuleParent : MonoBehaviour
{
    public GameObject dockingSpot;
    public TextMeshProUGUI timeText;
    public StaticCapsule InstantiateCapsule(GameObject capsule)
    {
        GameObject capsuleGo = Instantiate(capsule, transform);
        capsuleGo.GetComponent<StaticCapsule>().dockingSpot = dockingSpot;
        capsuleGo.GetComponent<StaticCapsule>().timeText = timeText;
        return capsuleGo.GetComponent<StaticCapsule>();
    }
}
