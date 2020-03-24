using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleManager : MonoBehaviour, IObserver
{
    #region Singleton
    private static CapsuleManager instance = null;

    public static CapsuleManager _instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CapsuleManager>();

                if (instance == null)
                {
                    instance = new CapsuleManager();
                }
            }

            return instance;
        }
    }
    #endregion

    public List<GameObject> capsules = new List<GameObject>();
    public List<GameObject> dockingPlaces = new List<GameObject>();

    private void Start()
    {
        int newCapsule = Random.Range(0, capsules.Count);

        SendToPlayer(capsules[newCapsule], 0);
    }

    public void SendToPlayer(GameObject dockAble, int? dock)
    {
        Debug.LogError(dockAble.name + " + " + dock);
        var dockingMovement = dockAble.GetComponent<CapsuleMovement>();
        dockingMovement.GoToPlayer(dock);
    }

    public void LeftPlayer(GameObject dockAble, int? dock)
    {
        Destroy(dockAble);

        for (int i = 0; i < capsules.Count; i++)
        {
            if (capsules[i] == null)
            {
                capsules.RemoveAt(i);
            }
        }

        if (capsules.Count > 0)
        {
            int newCapsule = Random.Range(0, capsules.Count);

            SendToPlayer(capsules[newCapsule], dock);
        }
    }

    public void AddObservable(GameObject observable)
    {
        capsules.Add(observable);
    }

    public void RemoveObservable(GameObject observable)
    {
        capsules.Remove(observable);
    }
}
