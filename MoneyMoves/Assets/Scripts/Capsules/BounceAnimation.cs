using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    float bpm;

    private void Start()
    {
        animator.SetTrigger("Start");
    }

    private void Update()
    {
        animator.SetFloat("Speed", bpm);
    }
}
