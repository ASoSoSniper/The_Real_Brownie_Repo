using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    MeshRenderer renderer;
    Material defaultMaterial;
    [SerializeField] Material highlightedMaterial;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        if (!renderer) renderer = GetComponentInChildren<MeshRenderer>();

        defaultMaterial = renderer.material;
    }

    public bool SetHighlighted(bool highlighted)
    {
        if (highlighted)
        {
            renderer.material = highlightedMaterial;
            return true;
        }

        renderer.material = defaultMaterial;
        return false;
    }
}
