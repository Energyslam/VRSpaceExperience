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

    public List<GameObject> dockingPlaces = new List<GameObject>();

    private List<GameObject> idleCapsules = new List<GameObject>();

    public float timeInSong = 0.0f;

    private void Start()
    {
        //for (int i = 0; i < dockingPlaces.Count; i++)
        //{
        //    int newCapsule = Random.Range(0, idleCapsules.Count);

        //    SendCapsuleToDock(idleCapsules[newCapsule], i);
        //}
    }

    // Grabs random idle Capsule from list and sends it to the dock
    public void SendCapsuleToDock(GameObject dockAble, int? dock)
    {
        idleCapsules.Remove(dockAble);
        var dockingMovement = dockAble.GetComponent<CapsuleMovement>();
        dockingMovement.GoToPlayer(dock);
    }

    // Called by a Capsule. Informs the CapsuleManager that a docking spot is now open. 
    public void CapsuleLeftDock(GameObject dockAble, int? dock)
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

            SendCapsuleToDock(idleCapsules[newCapsule], dock);
        }
    }

    // Add gameobject to lists of observables. Method from IObserver interface
    public void AddObservable(GameObject observable)
    {
        capsules.Add(observable);
        idleCapsules.Add(observable);
    }

    // Removes gameobject from lists of observables. Method from IObserver interface
    public void RemoveObservable(GameObject observable)
    {
        capsules.Remove(observable);
        idleCapsules.Remove(observable);
    }
}
