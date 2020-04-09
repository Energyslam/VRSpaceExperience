using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlingShooter : MonoBehaviour
{
    [SerializeField]
    GameObject centerPoint, normalProjectile;

    public bool isShooting, isPulling;
    public float reloadSpeed, projectileSpeed;

    public UnityEvent onPullReset;

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
    }
    public void StartPull()
    {
        isPulling = true;
    }

    void FixedUpdate()
    {
        //Only move the pullpoint back if it has been released
        if (isShooting)
        {
            float distance = (this.transform.position - centerPoint.transform.position).magnitude;
            this.transform.position = Vector3.MoveTowards(this.transform.position, centerPoint.transform.position, reloadSpeed * Time.deltaTime * distance);
            if (distance < 0.1f) //snap back into place
            {
                this.transform.position = centerPoint.transform.position;
            }
            if (this.transform.position == centerPoint.transform.position)
            {
                isShooting = false;
                onPullReset.Invoke();
            }
        }
    }
}
