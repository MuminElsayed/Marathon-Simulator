using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color[] colors;
    
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        colors = new Color[7] { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta, Color.white };
    }

    void SetRandomColor()
    {
        int randomIndex = Random.Range(0, colors.Length);
        Vector4 color = colors[randomIndex];
        color[3] = 1;
        meshRenderer.material.SetColor("_EmissionColor", colors[randomIndex] * 20);
    }

    void OnEnable()
    {
        SetRandomColor();
    }
}
