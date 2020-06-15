using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBaker : MonoBehaviour
{
    #region Singleton
    private static LightBaker instance = null;

    public static LightBaker _instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<LightBaker>();

                if (instance == null)
                {
                    instance = new LightBaker();
                }
            }

            return instance;
        }
    }
    #endregion

    [SerializeField]
    private List<GameObject> bakedCapsules, diners, catCafes, techs, beaches;

    public GameObject GetRandomCapsule()
    {
        return bakedCapsules[Random.Range(0, bakedCapsules.Count)];
    }

    public GameObject GetBakedDiner()
    {
        return diners[Random.Range(0, diners.Count)];
    }

    public GameObject GetBakedCatCafe()
    {
        return catCafes[Random.Range(0, catCafes.Count)];
    }

    public GameObject GetBakedTech()
    {
        return techs[Random.Range(0, techs.Count)];
    }

    public GameObject GetBakedBeach()
    {
        return beaches[Random.Range(0, beaches.Count)];
    }

    public void MoveCapsule(GameObject capsule)
    {
        if (bakedCapsules.Contains(capsule))
            bakedCapsules.Remove(capsule);

        if (beaches.Contains(capsule))
            beaches.Remove(capsule);

        if (techs.Contains(capsule))
            techs.Remove(capsule);

        if (diners.Contains(capsule))
            diners.Remove(capsule);

        if (catCafes.Contains(capsule))
            catCafes.Remove(capsule);
    }
}
