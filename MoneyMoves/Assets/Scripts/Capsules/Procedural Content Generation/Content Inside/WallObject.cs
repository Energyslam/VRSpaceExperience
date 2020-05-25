using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour
{
    public virtual void NotifyWallChange(bool isOpen)
    {
        GetComponent<MeshRenderer>().enabled = isOpen;
    }
}
