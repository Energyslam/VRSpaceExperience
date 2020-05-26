using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorOnStartup : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // You can re-use this block between calls rather than constructing a new one each time.
        var block = new MaterialPropertyBlock();

        block.SetColor("_BaseColor", ColorManager._instance.RandomBrightColor());

        // You can cache a reference to the renderer to avoid searching for it.
        GetComponent<Renderer>().SetPropertyBlock(block);
    }
}
