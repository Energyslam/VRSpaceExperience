using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    Renderer rend;
    public Material unlit, positiveLit, negativeLit;
    WhacASphere whacASphere;
    bool isActive;
    float activeTime, negativeActiveTime;
    float lifetime = 0f;

    public enum Mood
    {
        Positive,
        Negative
    }
    Mood mood;

    void  Awake()
    {
        rend = this.GetComponent<Renderer>();
    }
    IEnumerator KeepTrackOfLifetime()
    {
        yield return new WaitForSeconds(0.1f);
            lifetime += 0.1f;
        StartCoroutine(KeepTrackOfLifetime());
    }

    public void Initialize(WhacASphere whacASphere, WhacASphereVariables variables)
    {
        this.whacASphere = whacASphere;
        this.activeTime = variables.activeTime;
        this.negativeActiveTime = variables.negativeActiveTime;
    }

    public void ActivateASphere(Mood mood)
    {
        this.mood = mood;
        lifetime = 0f;
        StartCoroutine(c_ActivateSphere());
        StartCoroutine(KeepTrackOfLifetime());
    }
    IEnumerator c_ActivateSphere()
    {
        if (this.mood == Mood.Positive)
        {
            ActivatePositiveSphere(); 
            yield return new WaitForSeconds(activeTime);
        }
        else if (this.mood == Mood.Negative)
        {
            ActivateNegativeSphere();
            yield return new WaitForSeconds(negativeActiveTime);
        }
        DeactivateSphere();
    }

    void ActivatePositiveSphere()
    {
        rend.material = positiveLit;
        whacASphere.activatedSpheres.Add(this.gameObject);
        isActive = true;
    }
    
    void ActivateNegativeSphere()
    {
        rend.material = negativeLit;
        whacASphere.activatedSpheres.Add(this.gameObject);
        isActive = true;
    }

    public void DeactivateSphere()
    {
        StopAllCoroutines();
        if (mood == Mood.Positive)
        {
            whacASphere.totalSphereLifetime += lifetime;
            Debug.Log("Lifetime was " + lifetime);
        }
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
