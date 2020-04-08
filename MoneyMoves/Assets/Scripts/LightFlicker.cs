using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light light;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        light = this.GetComponent<Light>();
        anim = this.GetComponent<Animator>();
    }

    public void FlickerLights()
    {
        anim.SetTrigger("Flicker");
    }
    public void TurnOffLight()
    {
        light.enabled = false;
    }
    public void TurnOnLight()
    {
        light.enabled = true;
    }
}
