using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScaleOnStartup : MonoBehaviour
{
    [SerializeField]
    private Vector3 minScale, maxScale;

    public enum ScaleMode { STATIC, DYNAMIC };

    [SerializeField]
    private ScaleMode scaleMode = ScaleMode.DYNAMIC;

    // Start is called before the first frame update
    void Awake()
    {
        switch (scaleMode)
        {
            case ScaleMode.DYNAMIC:
            transform.localScale = new Vector3(Random.Range(minScale.x, maxScale.x), Random.Range(minScale.y, maxScale.y), Random.Range(minScale.z, maxScale.z));
                break;

            case ScaleMode.STATIC:
                float randomScale = Random.Range(minScale.x, maxScale.x);
                transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                break;
        }
    }
}
