using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable
{
    void Subscribe();
}

public interface IObserver
{
    void AddObservable(GameObject observable);
    void RemoveObservable(GameObject observable);
}

