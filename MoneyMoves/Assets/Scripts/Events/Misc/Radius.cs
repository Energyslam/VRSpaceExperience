using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Radius : MonoBehaviour
{
    [SerializeField]
    private GameObject areaPrefab;
    private GameObject eventArea;

    [SerializeField]
    private float xRadius = 1.0f, yRadius = 1.0f, zRadius = 1.0f;

    void OnEnable()
    {
        if (eventArea == null)
        {
            if (GetComponentInChildren<Area>() == null)
            {
                eventArea = Instantiate(areaPrefab, Vector3.zero, Quaternion.identity, transform);
            }
            else
            {
                eventArea = GetComponentInChildren<Area>().gameObject;
            }
        }
    }

    void OnValidate()
    {
        if (eventArea != null)
            eventArea.transform.localScale = new Vector3(xRadius, yRadius, zRadius);
    }
}
