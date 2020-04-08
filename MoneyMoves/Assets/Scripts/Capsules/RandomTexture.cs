using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTexture : MonoBehaviour
{
    [SerializeField]
    private Renderer rend;

    public List<Texture> textures;

    Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        ChangeMaterial();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeMaterial();
        }
    }

    void ChangeMaterial()
    {
        int tex = Random.Range(0, textures.Count);

        float width = textures[tex].width;
        float height = textures[tex].height;

        if (width > height)
        {
            float scaling = width / height;
            transform.localScale = new Vector3(originalScale.x, 1 / scaling * originalScale.y, originalScale.z);
        }
        else if (width < height)
        {
            float scaling = height / width;
            transform.localScale = new Vector3(1 / scaling * originalScale.x, originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = originalScale;
        }

        // You can re-use this block between calls rather than constructing a new one each time.
        var block = new MaterialPropertyBlock();

        // You can look up the property by ID instead of the string to be more efficient.
        block.SetTexture("_BaseMap", textures[tex]);

        // You can cache a reference to the renderer to avoid searching for it.
        rend.SetPropertyBlock(block);
    }
}
