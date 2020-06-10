using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorOnStartup : MonoBehaviour
{
    [SerializeField]
    private List<Color> colors;

    [SerializeField]
    private List<Renderer> renderers;

    // Start is called before the first frame update
    private void Start()
    {
        // You can re-use this block between calls rather than constructing a new one each time.
        var block = new MaterialPropertyBlock();

        if (colors.Count > 0)
        {
            block.SetColor("_BaseColor", colors[Random.Range(0, colors.Count)]);
        }
        else
        {
            block.SetColor("_BaseColor", ColorManager._instance.RandomBrightColor());
        }

        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].SetPropertyBlock(block);
        }
    }
}
