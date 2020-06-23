using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenOutsideArea : MonoBehaviour
{
    private Area area;
    private SpawnEvent shower;

    private void Start()
    {
        shower = GetComponentInParent<SpawnEvent>();

        area = shower.GetArea();
    }

    private void FixedUpdate()
    {
        if (transform.position.x < area.minX || transform.position.x > area.maxX || transform.position.y < area.minY || transform.position.y > area.maxY || transform.position.z < area.minZ || transform.position.z > area.maxZ)
        {
            shower.UpdateMeteorCount(-1);
            gameObject.SetActive(false);
        }
    }
}
