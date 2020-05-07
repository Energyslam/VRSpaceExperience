using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public GameObject distancePivot;

    public bool isClosed = true;

    [SerializeField]
    public List<WallObject> wallObjects;

    [SerializeField]
    private float animationSpeed = 1;

    private void Start()
    {
        isClosed = animationSpeed > 0 ? true : false;

        for (int i = 0; i < wallObjects.Count; i++)
        {
            wallObjects[i].NotifyWallChange(isClosed);
        }
    }

    public void Animate(bool hasToOpen)
    {
        float speed = hasToOpen ? animationSpeed : -animationSpeed;
        isClosed = speed > 0 ? true : false;

        for (int i = 0; i < wallObjects.Count; i++)
        {
            wallObjects[i].NotifyWallChange(isClosed);
        }

        animator.SetFloat("Speed", speed);

        animator.SetTrigger("OpeningTrigger");
    }
}
