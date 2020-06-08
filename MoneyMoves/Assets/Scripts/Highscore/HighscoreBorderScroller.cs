using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreBorderScroller : MonoBehaviour
{
    Renderer rend;
    public float ScrollX = 0.5f;
    public float ScrollY = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float xOffset = Time.time * ScrollX;
        float yOffset = Time.time * ScrollY;
        Vector2 offset = new Vector2(xOffset, yOffset);
        rend.material.SetTextureOffset("_BaseMap" , offset);
    }
}
