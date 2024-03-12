using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Android;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

public class PieceCasting : MonoBehaviour
{
    [Header("UI")]
    public Button confirmSelectionButton;
    public Button cancelSelectionButton;
    public GameObject pawnPromotionMenu;
    public GameObject turnUI;
    public GameObject reticle;
    private TMP_Text buttonText;
    private string initialText;

    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject blueVictory;
    [SerializeField] GameObject redVictory;

    [SerializeField] Sprite blueTurn;
    [SerializeField] Sprite redTurn;
    [SerializeField] Sprite grayTurn;
    [SerializeField] Image blueTurnBox;
    [SerializeField] Image redTurnBox;

    private Material prevMat;

    [Header("Selection")]
    [SerializeField] private GameObject objectHovered;
    [SerializeField] ChessPiece selectedChessPiece;
    ChessGrid grid;

    RaycastHit hit;

    private AudioSource buttonSource;
    public AudioClip selectClip;
    public AudioClip unselectClip;
    public AudioClip nextTurnClip;

    public enum SelectionMode
    {
        Piece,
        Tile,
        Observe
    }
    public SelectionMode selectionMode = SelectionMode.Piece;

    ChessPiece.Teams playerTurn = ChessPiece.Teams.White;

    [SerializeField] float lookSensitivity = 0.5f;
    ChessPiece pawnToPromote = null;
    Piece_King whiteKing = null;
    Piece_King blackKing = null;
    bool gameStart = false;
    bool turnSwitched = false;

    [Header("Prefabs")]
    [SerializeField] GameObject whiteKnightPrefab;
    [SerializeField] GameObject blackKnightPrefab;
    [SerializeField] GameObject whiteBishopPrefab;
    [SerializeField] GameObject blackBishopPrefab;
    [SerializeField] GameObject whiteRookPrefab;
    [SerializeField] GameObject blackRookPrefab;
    [SerializeField] GameObject whiteQueenPrefab;
    [SerializeField] GameObject blackQueenPrefab;

    private void Start()
    {
        buttonText = confirmSelectionButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        initialText = confirmSelectionButton.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text;

        grid = FindObjectOfType<ChessGrid>();
        buttonSource = gameObject.GetComponent<AudioSource>();

        Piece_King[] foundKings = FindObjectsOfType<Piece_King>();

        for (int i = 0; i < foundKings.Length; i++)
        {
            switch(foundKings[i].team)
            {
                case ChessPiece.Teams.White:
                    whiteKing = foundKings[i]; 
                    break;
                case ChessPiece.Teams.Black:
                    blackKing = foundKings[i];
                    break;
            }
        }

        StartCoroutine(GameStartDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (!grid)
        {
            grid = FindObjectOfType<ChessGrid>();
        }

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
                    //Debug.Log("The Raycast hit " + hit.collider.gameObject.name);
                    confirmSelectionButton.gameObject.SetActive(true);

                    if (!hit.collider.gameObject) return null;
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
            Highlight prevHighlight = objectHovered.GetComponent<Highlight>();
            if (prevHighlight)
            {
                prevHighlight.SetHighlighted(false);
            }
        }

        if (!gameObject || selectionMode == SelectionMode.Observe) return;

        Highlight highlight = gameObject.GetComponent<Highlight>();
        if (highlight)
        {
            highlight.SetHighlighted(true);
        }
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
            buttonSource.PlayOneShot(selectClip);
        }
        else if (!piece && selectionMode == SelectionMode.Tile)
        {
            if (selectedChessPiece)
            {
                if (selectedChessPiece.SelectRoute(objectHovered))
                {
                    selectionMode = SelectionMode.Observe;
                    turnSwitched = false;
                }
            }
        }

        ChangeButtonText();
    }

    bool CorrectSelectionMode(GameObject gameObject)
    {
        ChessPiece piece = GetChessPiece(gameObject);
        if (piece)
        {
            if (selectionMode == SelectionMode.Piece)
            {
                if (piece.team == playerTurn) return true;
            }

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
        double tiltAroundY = Input.GetAxisRaw("Horizontal") * lookSensitivity;
        double tiltAroundX = Input.GetAxisRaw("Vertical") * lookSensitivity;

        Vector3 rotate = new Vector3((float)tiltAroundX, (float)tiltAroundY * -1, 0);
        transform.eulerAngles = transform.eulerAngles - rotate;
    }

    public void Deselect()
    {
        if (selectionMode == SelectionMode.Tile)
        {
            selectedChessPiece = null;
            selectionMode = SelectionMode.Piece;
            ChangeButtonText();
            if (grid) grid.ResetBoardHighlighting();

            buttonSource.PlayOneShot(unselectClip);
        }
    }

    public void NextPlayerTurn()
    {
        if (pawnToPromote || !gameStart || turnSwitched) return;

        
        if (blackKing.InCheck())
        {
            Debug.Log("Black is in check");
        }
        if (whiteKing.InCheck())
        {
            Debug.Log("White is in check");
        }

        bool checkMate = false;
        switch (playerTurn)
        {
            case ChessPiece.Teams.White:
                if (!blackKing || blackKing.InCheckMate())
                {
                    checkMate = true;
                    Debug.Log("White wins!");
                    break;
                }
                
                break;
            case ChessPiece.Teams.Black:
                if (!whiteKing || whiteKing.InCheckMate())
                {
                    checkMate = true;
                    Debug.Log("Black wins!");
                    break;
                }
                
                break;
        }

        if (checkMate)
        {
            EndGame();
            return;
        }

        playerTurn = playerTurn == ChessPiece.Teams.White ? ChessPiece.Teams.Black : ChessPiece.Teams.White;
        TurnDisplay();

        if (grid) grid.ResetBoardHighlighting();
        EnPassantExpiration();

        buttonSource.PlayOneShot(nextTurnClip);
        selectionMode = SelectionMode.Piece;
        turnSwitched = true;
    }

    public void PawnPromotion(ChessPiece pawn)
    {
        if (pawnPromotionMenu) pawnPromotionMenu.SetActive(true);
        pawnToPromote = pawn;
    }

    public void SelectPawnPromotion(int type)
    {
        GameObject pieceToSpawn = null;
        switch (type)
        {
            case 0:
                pieceToSpawn = playerTurn == ChessPiece.Teams.White ? whiteKnightPrefab : blackKnightPrefab;
                break;
            case 1:
                pieceToSpawn = playerTurn == ChessPiece.Teams.White ? whiteBishopPrefab : blackBishopPrefab;
                break;
            case 2:
                pieceToSpawn = playerTurn == ChessPiece.Teams.White ? whiteRookPrefab : blackRookPrefab;
                break;
            case 3:
                pieceToSpawn = playerTurn == ChessPiece.Teams.White ? whiteQueenPrefab : blackQueenPrefab;
                break;
        }

        if (!pieceToSpawn) return;

        GameObject spawn = Instantiate(pieceToSpawn, pawnToPromote.transform.parent, false);
        spawn.transform.position = pawnToPromote.currentTile.transform.position;
        spawn.GetComponent<ChessPiece>().currentTile = pawnToPromote.currentTile;

        Destroy(pawnToPromote.gameObject);
        pawnToPromote = null;
        pawnPromotionMenu.SetActive(false);

        StartCoroutine(PlayerTurnSwitchDelay());
    }

    IEnumerator GameStartDelay()
    {
        yield return new WaitForSeconds(0.5f);

        gameStart = true;
        TurnDisplay();
    }
    IEnumerator PlayerTurnSwitchDelay()
    {
        yield return new WaitForSeconds(0.5f);

        NextPlayerTurn();
    }

    void EnPassantExpiration()
    {
        Piece_Pawn[] pawns = FindObjectsOfType<Piece_Pawn>();

        for (int i = 0; i < pawns.Length; i++)
        {
            if (pawns[i].team == playerTurn) pawns[i].moveCount = 0;
        }
    }

    void TurnDisplay()
    {
        switch (playerTurn)
        {
            case ChessPiece.Teams.White:
                blueTurnBox.sprite = blueTurn;
                redTurnBox.sprite = grayTurn;
                break;
            case ChessPiece.Teams.Black:
                blueTurnBox.sprite = grayTurn;
                redTurnBox.sprite = redTurn;
                break;
        }
    }

    void EndGame()
    {
        selectionMode = SelectionMode.Observe;

        victoryScreen.SetActive(true);

        confirmSelectionButton.gameObject.SetActive(false);
        cancelSelectionButton.gameObject.SetActive(false);
        turnUI.SetActive(false);
        reticle.SetActive(false);
        pawnPromotionMenu.SetActive(false);

        switch (playerTurn)
        {
            case ChessPiece.Teams.White:
                blueVictory.SetActive(true);
                break;
            case ChessPiece.Teams.Black:
                redVictory.SetActive(true);
                break;
        }
    }
}
