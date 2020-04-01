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

    [HideInInspector]
    public List<GameObject> capsules = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> dockingPlaces = new List<GameObject>();

    private List<GameObject> idleCapsules = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < dockingPlaces.Count; i++)
        {
            int newCapsule = Random.Range(0, idleCapsules.Count);

            SendToPlayer(idleCapsules[newCapsule], i);
        }
    }

    public void SendToPlayer(GameObject dockAble, int? dock)
    {
        idleCapsules.Remove(dockAble);
        var dockingMovement = dockAble.GetComponent<CapsuleMovement>();
        dockingMovement.GoToPlayer(dock);
    }

    public void LeftPlayer(GameObject dockAble, int? dock)
    {
        for (int i = 0; i < idleCapsules.Count; i++)
        {
            if (idleCapsules[i] == null)
            {
                idleCapsules.RemoveAt(i);
            }
        }

        if (idleCapsules.Count > 0)
        {
            int newCapsule = Random.Range(0, idleCapsules.Count);

            SendToPlayer(idleCapsules[newCapsule], dock);
        }
    }

    public void AddObservable(GameObject observable)
    {
        capsules.Add(observable);
        idleCapsules.Add(observable);
    }

    public void RemoveObservable(GameObject observable)
    {
        capsules.Remove(observable);
        idleCapsules.Remove(observable);
    }
}
