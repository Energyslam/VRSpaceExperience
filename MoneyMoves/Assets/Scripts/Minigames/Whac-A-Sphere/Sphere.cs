using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    Renderer rend;
    Material unlit, lit;
    WhacASphere whacASphere;
    bool isActive;
    float activeTime;

    // Start is called before the first frame update
    void  Awake()
    {
        rend = this.GetComponent<Renderer>();
    }

    public void Initialize(Material unlit, Material lit, WhacASphere whacASphere, float activeTime)
    {
        this.unlit = unlit;
        this.lit = lit;
        this.whacASphere = whacASphere;
        this.activeTime = activeTime;
    }

    public void Sphereoooo()
    {
        StartCoroutine(c_ActivateSphere());
    }
    IEnumerator c_ActivateSphere()
    {
        ActivateSphere();
        yield return new WaitForSeconds(activeTime);
        DeactivateSphere();
    }

    public void ActivateSphere()
    {
        rend.material = lit;
        whacASphere.activatedSpheres.Add(this.gameObject);
        isActive = true;
    }

    public void DeactivateSphere()
    {
        StopAllCoroutines();
        whacASphere.activatedSpheres.Remove(this.gameObject);
        rend.material = unlit;
        isActive = false;
    }

    public void Hit()
    {
        if (!isActive)
        {
            return;
        }
        whacASphere.ActivateRandomSphere();
        whacASphere.UpdateScore(10);
        DeactivateSphere();
    }


}
