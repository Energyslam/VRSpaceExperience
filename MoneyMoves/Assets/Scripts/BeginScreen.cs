using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginScreen : MonoBehaviour
{
    public Renderer[] renderers;
    public float dissolveSpeed = 0.005f;
    public float birdFadeoutSpeed = 0.001f;
    AudioSource audio;
    bool isDissolving;
    float transparancy = 0f;

    private void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }
    public void BeginGame()
    {
        isDissolving = true;
    }
    void FixedUpdate()
    {
        if (isDissolving && transparancy < 1f)
        {
            transparancy += dissolveSpeed;
            foreach (Renderer rend in renderers)
            {
                rend.material.SetFloat("_Transparancy", transparancy);
            }
        }

        if (isDissolving && audio.volume > 0f)
        {
            audio.volume -= birdFadeoutSpeed;
        }

        if (transparancy >= 1f && audio.volume <= 0f)
        {

            GameManager.Instance.platform.ChangeStateToSplit();
            Destroy(this.gameObject);
        }
    }
}
