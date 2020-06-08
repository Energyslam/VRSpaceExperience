﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : WallObject
{
    [SerializeField]
    private List<Material> ledMaterials = new List<Material>();

    [SerializeField]
    private MeshRenderer rend;

    [SerializeField]
    private Material targetMaterial;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color col1, col2;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rend.materials.Length - 1; i++)
        {
            ledMaterials.Add(rend.materials[i]);
            float randomTime = Random.Range(0, 2);
            if (randomTime < 0.5f) randomTime = 0.5f;
            StartCoroutine(ChangeColor(ledMaterials[i], randomTime));
        }
    }

    private IEnumerator ChangeColor(Material mat, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Color col = mat.GetColor("_BaseColor") == col1 ? col2 : col1;
        mat.SetColor("_BaseColor", col);
        mat.SetColor("_EmissionColor", col);
        StartCoroutine(ChangeColor(mat, waitTime));
    }

    public override void NotifyWallChange(bool isOpen)
    {
        rend.enabled = !isOpen;
    }
}
