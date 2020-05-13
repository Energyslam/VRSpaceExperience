using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class DebugRaycast : MonoBehaviour
{
    public Camera debugCam;
    Vector3 mousePosition;
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
                    //Do something
                    Debug.Log("Implement logic here for minigame hits");
                    hit.collider.GetComponent<MinigameTarget>().Hit();
                }
            }
        }
    }
}
