using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenOutsideArea : MonoBehaviour
{
    private Area area;
    private MeteoriteShower showerMeteorites;
    private RubbleShower showerRubble;

    private void Start()
    {
        showerMeteorites = GetComponentInParent<MeteoriteShower>();
        showerRubble = GetComponentInParent<RubbleShower>();

        area = showerMeteorites.GetArea();
        area = showerRubble.GetArea();
    }

    private void FixedUpdate()
    {
        if (transform.position.x < area.minX || transform.position.x > area.maxX || transform.position.y < area.minY || transform.position.y > area.maxY || transform.position.z < area.minZ || transform.position.z > area.maxZ)
        {
            if (GetComponent<Meteor>())
            {
                showerMeteorites.UpdateSpawnAmount(-1);
            }
            else if (GetComponent<SpaceRubble>())
            {
                showerRubble.UpdateSpawnAmount(-1);
            }
            gameObject.SetActive(false);
        }
    }
}
