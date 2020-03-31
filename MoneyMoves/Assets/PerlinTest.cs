using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinTest : MonoBehaviour
{
    public int width = 256, height = 256, scale = 1;
    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void OnDrawGizmosSelected()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Gizmos.color = CalculateColor(x, z);
                Gizmos.DrawCube(new Vector3(x, 0, z), new Vector3(0.9f, 0.9f, 0.9f));
            }
        }
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / (float)width * (float)scale;
        float yCoord = (float)y / (float)height * (float)scale;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        // Debug.LogError(sample);
        return new Color(sample, sample, sample);
    }
}
