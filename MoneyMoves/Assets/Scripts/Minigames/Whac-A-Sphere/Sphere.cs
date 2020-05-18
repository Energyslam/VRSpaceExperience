using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    Renderer rend;
    public Material unlit, positiveLit, negativeLit;
    WhacASphere whacASphere;
    bool isActive;
    float activeTime;

    public enum Mood
    {
        Positive,
        Negative
    }
    Mood mood;
    // Start is called before the first frame update
    void  Awake()
    {
        rend = this.GetComponent<Renderer>();
    }

    public void Initialize(WhacASphere whacASphere, float activeTime)
    {
        this.whacASphere = whacASphere;
        this.activeTime = activeTime;
    }

    public void Sphereoooo(Mood mood)
    {
        this.mood = mood;
        StartCoroutine(c_ActivateSphere());
    }
    IEnumerator c_ActivateSphere()
    {
        if (this.mood == Mood.Positive)
        {
            ActivateSphere();
        }
        else if (this.mood == Mood.Negative)
        {
            ActivateNegativeSphere();
        }
        yield return new WaitForSeconds(activeTime);
        DeactivateSphere();
    }

    public void ActivateSphere()
    {
        rend.material = positiveLit;
        whacASphere.activatedSpheres.Add(this.gameObject);
        isActive = true;
    }
    
    public void ActivateNegativeSphere()
    {
        rend.material = negativeLit;
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
        //whacASphere.ActivateRandomSphere();
        if (mood == Mood.Negative)
        {
            whacASphere.UpdateScore(-10);
        }
        else if (mood == Mood.Positive)
        {
            whacASphere.UpdateScore(10);
        }
        DeactivateSphere();
    }


}
