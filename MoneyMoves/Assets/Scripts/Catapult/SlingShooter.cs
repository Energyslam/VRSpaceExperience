using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlingShooter : MonoBehaviour
{
    [SerializeField]
    GameObject centerPoint, swingTarget, normalProjectile, swingObjectHolder;

    [SerializeField]
    List<GameObject> swingObjects;

    private int currentSwingObject = 0;

    public bool isShooting, isPulling;
    public float reloadSpeed, projectileSpeed;
    AudioSource audio;

    public UnityEvent onPullReset;

    private void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        //this.GetComponent<PlaySound>().PlayAudio();
        isShooting = true;
        isPulling = false;
        CreateProjectile(normalProjectile);
    }

    public void CreateProjectile(GameObject go)
    {
        Vector3 forward = centerPoint.transform.position - this.transform.position;
        GameObject projectileGO = Instantiate(go, this.transform.position, Quaternion.identity);
        Destroy(projectileGO, 5f);
        Rigidbody projectileRB = projectileGO.GetComponent<Rigidbody>();
        projectileRB.AddForce(forward * projectileSpeed, ForceMode.VelocityChange);
        audio.Play();
        swingTarget = swingObjects[0];
    }
    public void StartPull()
    {
        isPulling = true;
    }

    void FixedUpdate()
    {
        SlingSwing();
    }

    private void SlingSwing()
    {
        //Only move the pullpoint back if it has been released
        if (isShooting)
        {
            float distance = (transform.position - swingTarget.transform.position).magnitude;
            this.transform.position = Vector3.MoveTowards(this.transform.position, swingTarget.transform.position, reloadSpeed * Time.deltaTime * distance);

            if (distance < 0.1f) //snap back into place
            {
                this.transform.position = swingTarget.transform.position;

                if (currentSwingObject + 1 < swingObjects.Count)
                {
                    currentSwingObject++;
                    swingTarget = swingObjects[currentSwingObject];
                }

            }

            if (this.transform.position == centerPoint.transform.position)
            {
                isShooting = false;
                currentSwingObject = 0;
                onPullReset.Invoke();
            }
        }

        else if (isPulling)
        {
            swingObjectHolder.transform.LookAt(transform);
        }
    }
}
