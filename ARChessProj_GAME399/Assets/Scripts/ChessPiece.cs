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
    public GameObject currentTile;
    [SerializeField] float groundCheckDistance = 8f;
    //protected List<List<GameObject>> possibleRoutes = new List<List<GameObject>>();
    protected Dictionary<List<GameObject>, ChessPiece> possibleRoutes = new Dictionary<List<GameObject>, ChessPiece>();

    [SerializeField] protected List<GameObject> selectedRoute = new List<GameObject>();
    protected ChessPiece targetPiece;

    protected int selectedTileIndex = 0;
    protected bool moving;

    protected Teams ally;
    protected Teams enemy;

    [HideInInspector] public bool firstMove = false;
    

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<ChessGrid>();

        enemy = team == Teams.Black ? Teams.White : Teams.Black;
        ally = team == Teams.Black ? Teams.Black : Teams.White;
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
            moving = true;
            CastlingCheck();
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

        float time = moveTime / selectedTileIndex;

        transform.position = Vector3.Lerp(startPoint, endPoint, time * Time.deltaTime);
        if (Vector3.Distance(transform.position, endPoint) < 0.01f)
        {
            currentTile = null;
            moving = false;

            if (targetPiece)
            {
                Debug.Log("Destroyed target");
                Destroy(targetPiece.gameObject);
                targetPiece = null;
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

        if (selectedRoute == king.rightCastling)
        {
            king.savedRightRook.selectedRoute = king.rightCastling;
            king.savedRightRook.selectedTileIndex = 1;

            moving = true;
            return true;
        }
        else if (selectedRoute == king.leftCastling)
        {
            king.savedLeftRook.selectedRoute = king.leftCastling;
            king.savedLeftRook.selectedTileIndex = 1;

            moving = true;
            return true;
        }
        
        return false;
    }
}
