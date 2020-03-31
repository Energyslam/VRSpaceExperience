using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int horizontal, vertical;
    public Color color;

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + new Vector3(0, 1, 0), color, 1000f);
    }
}
