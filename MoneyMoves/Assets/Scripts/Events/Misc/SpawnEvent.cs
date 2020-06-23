using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEvent : Event
{
    public virtual Area GetArea()
    {
        return null;
    }

    public virtual void UpdateSpawnAmount(int addition)
    {

    }
}
