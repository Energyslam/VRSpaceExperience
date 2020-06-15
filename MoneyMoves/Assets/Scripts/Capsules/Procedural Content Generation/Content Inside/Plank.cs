using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : WallObject
{
    public override void NotifyWallChange(bool isOpen)
    {
        List<MeshRenderer> rend = new List<MeshRenderer>();
        rend.Add(GetComponent<MeshRenderer>());

        var rendChild = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < rendChild.Length; i++)
        {
            rend.Add(rendChild[i]);
        }

        for (int i = 0; i < rend.Count; i++)
        {
            rend[i].enabled = !isOpen;
        }

        GetComponent<BoxCollider>().enabled = !isOpen;
    }
}
