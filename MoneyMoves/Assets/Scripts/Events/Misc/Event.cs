using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Radius))]
public class Event : MonoBehaviour
{
    [Header("Event information")]
    [SerializeField]
    protected float eventTime = 5.0f, pointMultiplier = 2.0f;

    public Area area;
}
