using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using TMPro;

public class PieceCasting : MonoBehaviour
{
    public Button confirmSelectionButton;
    private TMP_Text buttonText;

    private GameObject objectSelected;

    RaycastHit hit;

    private void Start()
    {
        buttonText = confirmSelectionButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        CastingForPieces();
    }

    GameObject CastingForPieces()
    {
        bool didHit = Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity);
        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

        if(didHit)
        {
            Debug.Log("The Raycast hit " + hit.collider.gameObject.name);
            confirmSelectionButton.gameObject.SetActive(true);
            return hit.collider.gameObject;
        }

        confirmSelectionButton.gameObject.SetActive(false);
        return null;
    }


}
