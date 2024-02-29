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
    protected List<List<GameObject>> possibleRoutes = new List<List<GameObject>>();
    //protected Dictionary<List<GameObject>, ChessPiece> possibleRoutes = new Dictionary<List<GameObject>, ChessPiece>();
    protected ChessPiece targetPiece;

    [SerializeField] List<GameObject> selectedRoute = new List<GameObject>();

    protected int selectedRouteIndex = 0;
    protected int selectedTileIndex = 0;
    protected bool moving;
    

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<ChessGrid>();
        
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
    }

    public virtual bool SelectRoute(GameObject gameObject)
    {
        GameObject tile = grid.GetTile(gameObject);
        bool validDestination = false;

        for (int i = 0; i < possibleRoutes.Count; i++)
        {
            for (int j = 1; j < possibleRoutes[i].Count; j++)
            {
                if (tile == possibleRoutes[i][j].transform.GetChild(0).gameObject)
                {
                    if (tile == currentTile) return false;

                    selectedRouteIndex = i;
                    selectedTileIndex = j;
                    validDestination = true;
                    break;
                }
            }
        }

        if (validDestination)
        {
            selectedRoute = possibleRoutes[selectedRouteIndex];
            moving = true;
            return true;
        }

        return false;
    }
    public virtual List<List<GameObject>> CreatePossibleRoutes()
    {
        possibleRoutes.Clear();
        return possibleRoutes;
    }

    public virtual bool Move()
    {
        if (!moving) return false;

        Vector3 startPoint = transform.position;
        Vector3 endPoint = possibleRoutes[selectedRouteIndex][selectedTileIndex].transform.GetChild(0).position;

        float time = moveTime * selectedTileIndex;

        transform.position = Vector3.Lerp(startPoint, endPoint, time * Time.deltaTime);
        if (Vector3.Distance(transform.position, endPoint) < 0.0001f)
        {
            currentTile = null;
            moving = false;
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
}
