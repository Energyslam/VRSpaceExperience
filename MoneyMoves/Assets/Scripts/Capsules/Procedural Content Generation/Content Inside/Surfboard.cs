using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surfboard : MonoBehaviour
{
    [SerializeField]
    private List<Texture> patterns = new List<Texture>();

    [SerializeField]
    private float maxRotation;

    // Start is called before the first frame update
    void Start()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(-maxRotation, maxRotation));
        float newScale = Random.Range(0.8f, 1.2f);
        transform.localScale *= newScale;

        int index = Random.Range(0, patterns.Count);

        Color color = ColorManager._instance.RandomBrightColor();

        // You can re-use this block between calls rather than constructing a new one each time.
        var block = new MaterialPropertyBlock();

        // You can look up the property by ID instead of the string to be more efficient.
        block.SetTexture("PatternTexture", patterns[index]);
        block.SetColor("BaseColor", color);

        // You can cache a reference to the renderer to avoid searching for it.
        GetComponent<Renderer>().SetPropertyBlock(block);
    }
}
