using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float animationSpeed = 1;

    public void Animate(bool hasToOpen)
    {
        float speed = hasToOpen ? animationSpeed : -animationSpeed;
        animator.SetFloat("Speed", speed);
        animator.SetTrigger("OpeningTrigger");
    }
}
