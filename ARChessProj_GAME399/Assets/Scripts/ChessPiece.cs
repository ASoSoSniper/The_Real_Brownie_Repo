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

    [SerializeField] float raisedHeight = 10f;

    [SerializeField] float raiseTime = 1f;
    [SerializeField] float moveTime = 0.5f;

    protected ChessGrid grid;
    public GameObject currentTile;
    [SerializeField] float groundCheckDistance = 8f;
    protected List<List<GameObject>> possibleRoutes = new List<List<GameObject>>();
    [SerializeField] List<GameObject> selectedRoute = new List<GameObject>();

    int selectedRouteIndex = 0;
    bool moving;

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

    public void SelectRoute(GameObject gameObject)
    {
        GameObject tile = grid.GetTile(gameObject);

        for (int i = 0; i < possibleRoutes.Count; i++)
        {
            for (int j = 0; j < possibleRoutes[i].Count; j++)
            {
                if (tile == possibleRoutes[i][j])
                {
                    selectedRouteIndex = i;
                    break;
                }
            }
            
        }
        selectedRoute = possibleRoutes[selectedRouteIndex];
        moving = true;
    }
    public virtual List<List<GameObject>> CreatePossibleRoutes()
    {
        possibleRoutes.Clear();
        return possibleRoutes;
    }

    public virtual void Move()
    {
        if (!moving) return;

        Vector3 startPoint = transform.position;
        Vector3 endPoint = possibleRoutes[selectedRouteIndex][possibleRoutes[selectedRouteIndex].Count - 1].transform.GetChild(0).position;
        //endPoint.y = 1f;
        float time = moveTime * possibleRoutes[selectedRouteIndex].Count;

        transform.position = Vector3.Lerp(startPoint, endPoint, time * Time.deltaTime);
        //Debug.Log(Vector3.Distance(transform.position, endPoint));
        if (Vector3.Distance(transform.position, endPoint) < 0.0001f)
        {
            currentTile = null;
            moving = false;
        }
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
