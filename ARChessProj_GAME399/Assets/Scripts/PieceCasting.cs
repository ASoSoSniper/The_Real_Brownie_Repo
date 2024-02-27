using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Android;
using static UnityEngine.GraphicsBuffer;

public class PieceCasting : MonoBehaviour
{
    public Button confirmSelectionButton;
    private TMP_Text buttonText;
    private string initialText;

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
        initialText = confirmSelectionButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text;
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

        CameraMovement();
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

        ChangeButtonText();
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

    public void ChangeButtonText()
    {
        if (selectionMode == SelectionMode.Tile)
        {
            buttonText.text = "Confirm Move";
        }
        else
        {
            buttonText.text = initialText;
        }

    }

    void CameraMovement()
    {
        double tiltAroundY = Input.GetAxisRaw("Horizontal") * 90.0;
        double tiltAroundX = Input.GetAxisRaw("Vertical") * 90.0 * -1.0;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler((float)tiltAroundX, (float)tiltAroundY, 0);

        //gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2.0f);
    }
}
