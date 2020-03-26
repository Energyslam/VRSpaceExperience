using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float animationSpeed = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Animate(bool hasToOpen)
    {
        float speed = hasToOpen ? animationSpeed : -animationSpeed;

        animator.SetTrigger("OpeningTrigger");
        animator.speed = speed;

        //Vector3 newPosition;

        //newPosition = hasToOpen ? openPosition : closedPosition;

        //transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, animationSpeed * Time.deltaTime);

        //if (transform.localPosition != newPosition)
        //{
        //    yield return new WaitForEndOfFrame();
        //    StartCoroutine(Animate(hasToOpen));
        //}
    }
}
