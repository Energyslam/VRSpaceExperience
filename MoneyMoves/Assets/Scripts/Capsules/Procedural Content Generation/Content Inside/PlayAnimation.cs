using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    float animationSpeed;

    private void Start()
    {
        animator.SetTrigger("Start");
    }

    private void Update()
    {
        animator.SetFloat("Speed", animationSpeed);
    }
}
