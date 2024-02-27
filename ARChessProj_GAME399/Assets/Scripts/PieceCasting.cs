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

    private Material prevMat;

    [SerializeField] private GameObject objectHovered;
    [SerializeField] ChessPiece selectedChessPiece;

    RaycastHit hit;

    public enum SelectionMode
    {
        Piece,
        Tile
    }
    public SelectionMode selectionMode = SelectionMode.Piece;

    private void Start()
    {
        buttonText = confirmSelectionButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        CastingForPieces();

        //Debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectObject();
        }
    }

    GameObject CastingForPieces()
    {
        bool didHit = Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity);
        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

        if(didHit)
        {
            if(hit.collider.gameObject != objectHovered)
            {
                if (CorrectSelectionMode(hit.collider.gameObject))
                {
                    Debug.Log("The Raycast hit " + hit.collider.gameObject.name);
                    confirmSelectionButton.gameObject.SetActive(true);

                    HoverOverObject(hit.collider.gameObject);
                }
            }
            
            return hit.collider.gameObject;
        }

        confirmSelectionButton.gameObject.SetActive(false);
        return null;
    }

    void HoverOverObject(GameObject gameObject)
    {
        if(objectHovered)
        {
            objectHovered.GetComponent<Highlight>().SetHighlighted(false);
        }

        gameObject.GetComponent<Highlight>().SetHighlighted(true);
        objectHovered = gameObject;
    }

    public void SelectObject()
    {
        if (!objectHovered) return;

        ChessPiece piece = GetChessPiece(objectHovered);

        if (piece && selectionMode == SelectionMode.Piece)
        {
            selectedChessPiece = piece;
            selectedChessPiece.SelectPiece();
            selectionMode = SelectionMode.Tile;
        }
        else if (!piece && selectionMode == SelectionMode.Tile)
        {
            if (selectedChessPiece)
            {
                selectedChessPiece.SelectRoute(objectHovered);
            }
            selectionMode = SelectionMode.Piece;
        }
    }

    bool CorrectSelectionMode(GameObject gameObject)
    {
        if (GetChessPiece(gameObject))
        {
            if (selectionMode == SelectionMode.Piece) return true;

            return false;
        }
        else
        {
            if (selectionMode == SelectionMode.Tile) return true;

            return false;
        }
    }

    ChessPiece GetChessPiece(GameObject gameObject)
    {
        ChessPiece piece = null;

        if (piece = gameObject.GetComponent<ChessPiece>())
        {
            return piece;
        }
        else if (piece = gameObject.GetComponentInParent<ChessPiece>())
        {
            return piece;
        }
        else if (piece = gameObject.GetComponentInChildren<ChessPiece>())
        {
            return piece;
        }

        return piece;
    }
}
