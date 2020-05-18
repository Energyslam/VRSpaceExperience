using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private bool openOnStartUp;

    public GameObject distancePivot;

    public bool isOpen = true;

    [SerializeField]
    public List<WallObject> wallObjects;

    [SerializeField]
    private float animationSpeed = 1;

    private void Start()
    {
        if (openOnStartUp) Animate(true);
    }

    public void Animate(bool hasToOpen)
    {
        float speed = hasToOpen ? animationSpeed : -animationSpeed;
        isOpen = speed > 0 ? true : false;

        for (int i = 0; i < wallObjects.Count; i++)
        {
            wallObjects[i].NotifyWallChange(isOpen);
        }

        animator.SetFloat("Speed", speed);

        animator.SetTrigger("OpeningTrigger");
    }
}
