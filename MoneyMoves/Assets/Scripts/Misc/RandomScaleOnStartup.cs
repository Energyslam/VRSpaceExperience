using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScaleOnStartup : MonoBehaviour
{
    [SerializeField]
    private Vector3 minScale, maxScale;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(Random.Range(minScale.x, maxScale.x), Random.Range(minScale.y, maxScale.y), Random.Range(minScale.z, maxScale.z));
    }
}
