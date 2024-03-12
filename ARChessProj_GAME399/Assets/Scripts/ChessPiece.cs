using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public enum PieceType
    {
        None,
        Pawn,
        Knight,
        Bishop,
        Rook,
        King,
        Queen
    }

    public enum Teams
    {
        None,
        White,
        Black
    }

    public PieceType type;
    public Teams team;

    [SerializeField] protected float moveTime = 0.5f;

    protected ChessGrid grid;
    protected PieceCasting controller;

    public GameObject currentTile;
    [SerializeField] float groundCheckDistance = 8f;
    protected Dictionary<List<GameObject>, ChessPiece> possibleRoutes = new Dictionary<List<GameObject>, ChessPiece>();

    [SerializeField] protected List<GameObject> selectedRoute = new List<GameObject>();
    protected ChessPiece targetPiece;

    protected int selectedTileIndex = 0;
    protected bool moving;

    protected Teams ally;
    protected Teams enemy;

    [HideInInspector] public bool firstMove = false;
    [HideInInspector] public int specialMoveSpeedMod = 1;


    public GameObject explosionParticle;

    public AudioClip deathClip;
    public AudioClip moveClip;
    public AudioClip attackClip;
    private AudioSource pieceSource;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<ChessGrid>();
        controller = FindObjectOfType<PieceCasting>();

        enemy = team == Teams.Black ? Teams.White : Teams.Black;
        ally = team == Teams.Black ? Teams.Black : Teams.White;

        pieceSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentTile)
        {
            GroundCheck();
        }

        if (moving || possibleRoutes.Count == 0)
        {
            Move();
        }
    }

    public void SelectPiece()
    {
        possibleRoutes.Clear();

        possibleRoutes = CreatePossibleRoutes();

        foreach (List<GameObject> route in possibleRoutes.Keys)
        {
            foreach (GameObject tile in route)
            {
                if (tile != currentTile)
                    tile.GetComponentInChildren<Highlight>().SetAsRouteTile(true, grid.TileHasEnemy(tile, this));
            }
        }
    }

    public virtual bool SelectRoute(GameObject gameObject)
    {
        GameObject tile = grid.GetTile(gameObject);
        bool validDestination = false;

        foreach (KeyValuePair<List<GameObject>, ChessPiece> route in possibleRoutes)
        {
            for (int i = 0; i < route.Key.Count; i++)
            {
                if (tile == route.Key[i].transform.GetChild(0).gameObject)
                {
                    if (tile == currentTile) return false;

                    selectedRoute = route.Key;

                    ChessPiece piece = grid.GetChessPiece(tile, enemy);
                    if (piece == route.Value || type == PieceType.Pawn)
                    {
                        targetPiece = route.Value;
                    }
                    else
                    {
                        targetPiece = null;
                    }
                    
                    selectedTileIndex = i;
                    validDestination = true;
                    break;
                }
            }
        }

        if (validDestination)
        {
            CastlingCheck();
            moving = true;

            if (targetPiece)
            {
                pieceSource.PlayOneShot(attackClip);
                Debug.Log("Attacked");
            }
            else
            {
                pieceSource.PlayOneShot(moveClip);
                Debug.Log("Moved");
            }
            
            return true;
        }

        return false;
    }
    public virtual Dictionary<List<GameObject>, ChessPiece> CreatePossibleRoutes()
    {
        return possibleRoutes;
    }

    public virtual bool Move()
    {
        if (!moving) return false;

        if (!firstMove) firstMove = true;

        Vector3 startPoint = transform.position;
        Vector3 endPoint = selectedRoute[selectedTileIndex].transform.GetChild(0).position;

        float time = moveTime / (Mathf.Min(selectedTileIndex, 3) * specialMoveSpeedMod);

        transform.position = Vector3.Lerp(startPoint, endPoint, time * Time.deltaTime);
        if (Vector3.Distance(transform.position, endPoint) < 0.1f)
        {
            currentTile = null;
            moving = false;
            specialMoveSpeedMod = 1;

            if (targetPiece)
            {
                Debug.Log("Destroyed target");
                DestroyLogic(targetPiece.gameObject);
            }
        }

        return true;
    }

    void GroundCheck()
    {
        RaycastHit rayHit;
        Ray ray = new Ray();
        ray.origin = transform.position + Vector3.up * 2f;
        ray.direction = Vector3.down;

        bool hit = Physics.Raycast(ray, out rayHit, groundCheckDistance);
        Debug.DrawLine(ray.origin, ray.origin + Vector3.down * groundCheckDistance, Color.red, 0.5f);

        if (!hit) return;
        
        currentTile = grid.GetTile(rayHit.collider.gameObject);

        if (currentTile == null) return;
        transform.position = currentTile.transform.position;

        PawnPromotionCheck();
        controller.NextPlayerTurn();
    }

    protected List<GameObject> CreateRouteInDirection(Vector2 initPos, Vector2 direction, out ChessPiece foundEnemy)
    {
        List<GameObject> route = new List<GameObject>();
        route.Add(currentTile);

        int x = (int)initPos.x;
        int z = (int)initPos.y;
        bool endOfDirection = false;
        ChessPiece enemyPiece = null;

        while (!endOfDirection)
        {
            x += (int)direction.x;
            z += (int)direction.y;
            if (grid.TileExists(x, z))
            {
                GameObject tile = grid.GetTile(x, z);
                Teams occupation = grid.GetPieceOnTile(tile);
                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    enemyPiece = grid.GetChessPiece(x, z, enemy);
                    endOfDirection = true;
                }
                route.Add(tile);
            }
            else
            {
                endOfDirection = true;
            }
        }

        foundEnemy = enemyPiece;
        return route;
    }

    protected List<GameObject> CreateRoute2Point(int xPos, int zPos, out ChessPiece foundEnemy)
    {
        List<GameObject> route = new List<GameObject>();
        ChessPiece enemyPiece = null;

        if (grid.TileExists(xPos, zPos))
        {
            GameObject tile = grid.GetTile(xPos, zPos);
            Teams occupation = grid.GetPieceOnTile(tile);

            if (occupation != ally)
            {
                route.Add(currentTile);
                route.Add(tile);

                enemyPiece = grid.GetChessPiece(xPos, zPos, enemy);
            }
        }

        foundEnemy = enemyPiece;
        return route;
    }

    bool CastlingCheck()
    {
        if (type != PieceType.King || firstMove) return false;

        Piece_King king = GetComponent<Piece_King>();
        if (!king) return false;

        bool isPath = true;

        if (king.rightCastling != null)
        {
            for (int i = 0; i < selectedRoute.Count; i++)
            {
                if (!selectedRoute[i].Equals(king.rightCastling[i]))
                {
                    isPath = false;
                    break;
                }
            }
            if (isPath)
            {
                king.savedRightRook.selectedRoute = king.rightCastling;
                king.savedRightRook.selectedTileIndex = 1;

                king.savedRightRook.specialMoveSpeedMod = 2;
                king.savedRightRook.moving = true;
                return true;
            }
        }

        isPath = true;

        if (king.leftCastling != null)
        {
            for (int i = 0; i < selectedRoute.Count; i++)
            {
                if (!selectedRoute[i].Equals(king.leftCastling[i]))
                {
                    isPath = false;
                    break;
                }
            }
            if (isPath)
            {
                king.savedLeftRook.selectedRoute = king.leftCastling;
                king.savedLeftRook.selectedTileIndex = 1;

                king.savedLeftRook.specialMoveSpeedMod = 2;
                king.savedLeftRook.moving = true;
                return true;
            }
        }
        
        return false;
    }


    bool PawnPromotionCheck()
    {
        if (type != PieceType.Pawn) return false;

        int dir = team == Teams.Black ? 1 : -1;
        int x = 0;
        int z = 0;

        grid.GetTileCoordinates(currentTile, out x, out z);
        if (!grid.TileExists(x, z + dir))
        {
            controller.PawnPromotion(this);
            return true;
        }

        return false;
    }

    protected void DestroyLogic(GameObject pieceToDestroy)
    {
        GameObject particle = Instantiate(explosionParticle, transform.position, transform.rotation);
        particle.GetComponent<AudioSource>().PlayOneShot(deathClip);

        Destroy(pieceToDestroy);
        targetPiece = null;
    }

    public virtual bool TileUnderAttack(GameObject tileToCheck)
    {
        ChessPiece[] allPieces = FindObjectsOfType<ChessPiece>();

        for (int i = 0; i < allPieces.Length; i++)
        {
            if (allPieces[i].team != enemy || allPieces[i] == this ||
                (allPieces[i].type == PieceType.King && !allPieces[i].firstMove)) continue;

            Dictionary<List<GameObject>, ChessPiece> routes = allPieces[i].CreatePossibleRoutes();
            foreach (List<GameObject> route in routes.Keys)
            {
                foreach (GameObject tile in route)
                {
                    if (tile == tileToCheck)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    protected virtual bool TileUnderAttack(GameObject tileToCheck, out List<ChessPiece> attackers)
    {
        bool underAttack = false;
        attackers = new List<ChessPiece>();

        ChessPiece[] allPieces = FindObjectsOfType<ChessPiece>();

        for (int i = 0; i < allPieces.Length; i++)
        {
            if (allPieces[i].team != enemy || allPieces[i] == this ||
                (allPieces[i].type == PieceType.King && !allPieces[i].firstMove)) continue;

            Dictionary<List<GameObject>, ChessPiece> routes = allPieces[i].CreatePossibleRoutes();
            foreach (List<GameObject> route in routes.Keys)
            {
                foreach (GameObject tile in route)
                {
                    if (tile == tileToCheck)
                    {
                        underAttack = true;
                        attackers.Add(allPieces[i]);
                    }
                }
            }
        }
        return underAttack;
    }
}
