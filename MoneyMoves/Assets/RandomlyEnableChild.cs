using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyEnableChild : MonoBehaviour
{
    [SerializeField]
    List<GameObject> children = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < children.Count; i++)
        {
            bool chance = Random.value > 0.5f ? true : false;

            children[i].SetActive(chance);
        }
    }
}
