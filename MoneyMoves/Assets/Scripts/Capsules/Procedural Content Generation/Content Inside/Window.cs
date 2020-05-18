using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : WallObject
{
    [SerializeField]
    private List<MeshRenderer> rend;

    private void Start()
    {
        int randRot = Random.Range(0, 360);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, randRot);
    }

    public override void NotifyWallChange(bool isOpen)
    {
        for (int i = 0; i < rend.Count; i++)
        {
            rend[i].enabled = !isOpen;
        }
    }
}
