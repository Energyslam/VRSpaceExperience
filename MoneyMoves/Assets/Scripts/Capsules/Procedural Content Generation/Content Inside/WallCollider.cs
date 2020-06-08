using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : WallObject
{
    public override void NotifyWallChange(bool isOpen)
    {
        gameObject.SetActive(isOpen);
    }
}
