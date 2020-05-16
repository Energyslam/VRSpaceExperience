using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class DebugRaycast : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("MinigameTarget"))
                {
                    hit.collider.GetComponent<MinigameTarget>().Hit();
                }
                else if (hit.collider.CompareTag("Collideable"))
                {
                    GiftBehaviour behaviour = hit.collider.gameObject.GetComponent<GiftBehaviour>();
                    if (behaviour != null)
                    {
                        behaviour.SolveCollision();
                    }
                }
            }
        }
    }
}
