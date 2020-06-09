using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Area : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> corners;

    public float minX, maxX, minY, maxY, minZ, maxZ;

    private void Update()
    {
        CalculateX();
        CalculateY();
        CalculateZ();
    }

    private void CalculateX()
    {
        float tempMinX = Mathf.Infinity, tempMaxX = -Mathf.Infinity;

        for (int i = 0; i < corners.Count; i++)
        {
            if (corners[i].transform.position.x < tempMinX)
                tempMinX = corners[i].transform.position.x;

            if (corners[i].transform.position.x > tempMaxX)
                tempMaxX = corners[i].transform.position.x;
        }

        minX = tempMinX;
        maxX = tempMaxX;
    }

    private void CalculateY()
    {
        float tempMinY = Mathf.Infinity, tempMaxY = -Mathf.Infinity;

        for (int i = 0; i < corners.Count; i++)
        {
            if (corners[i].transform.position.y < tempMinY)
                tempMinY = corners[i].transform.position.y;

            if (corners[i].transform.position.y > tempMaxY)
                tempMaxY = corners[i].transform.position.y;
        }

        minY = tempMinY;
        maxY = tempMaxY;
    }

    private void CalculateZ()
    {
        float tempMinZ = Mathf.Infinity, tempMaxZ = -Mathf.Infinity;

        for (int i = 0; i < corners.Count; i++)
        {
            if (corners[i].transform.position.z < tempMinZ)
                tempMinZ = corners[i].transform.position.z;

            if (corners[i].transform.position.z > tempMaxZ)
                tempMaxZ = corners[i].transform.position.z;
        }

        minZ = tempMinZ;
        maxZ = tempMaxZ;
    }
}
