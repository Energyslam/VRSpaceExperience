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

    [HideInInspector]
    public List<GameObject> capsules = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> dockingPlaces = new List<GameObject>();

    [SerializeField]
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
        Debug.LogError(dockAble.name + " + " + dock);
        idleCapsules.Remove(dockAble);
        var dockingMovement = dockAble.GetComponent<CapsuleMovement>();
        dockingMovement.GoToPlayer(dock);
    }

    public void LeftPlayer(GameObject dockAble, int? dock)
    {
        // Destroy(dockAble);

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
        idleCapsules.Add(observable);
    }

    public void RemoveObservable(GameObject observable)
    {
        capsules.Remove(observable);
        idleCapsules.Remove(observable);
    }
}
