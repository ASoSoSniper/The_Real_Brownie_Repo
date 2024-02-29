using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    MeshRenderer renderer;
    Material defaultMaterial;
    Material currentDefault;
    [SerializeField] Material highlightedMaterial;
    [SerializeField] Material pathMaterial;
    [SerializeField] Material violenceMaterial;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        if (!renderer) renderer = GetComponentInChildren<MeshRenderer>();

        defaultMaterial = renderer.material;
        currentDefault = defaultMaterial;
    }

    public bool SetHighlighted(bool highlighted)
    {
        if (highlighted)
        {
            renderer.material = highlightedMaterial;
            return true;
        }

        renderer.material = currentDefault;
        return false;
    }

    public bool SetAsRouteTile(bool set, bool tileHasEnemy = false)
    {
        if (!pathMaterial || !violenceMaterial) return false;

        if (set)
        {
            currentDefault = tileHasEnemy ? violenceMaterial : pathMaterial;
        }
        else
        {
            currentDefault = defaultMaterial;
        }

        renderer.material = currentDefault;

        return set;
    }
}
