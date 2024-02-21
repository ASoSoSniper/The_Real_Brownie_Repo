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
    [SerializeField] int maxTileMoves = 1;

    [SerializeField] List<Vector3> movePath = new List<Vector3>();

    [SerializeField] float raisedHeight = 10f;

    [SerializeField] float raiseTime = 1f;
    [SerializeField] float moveTime = 0.5f;

    protected ChessGrid grid;
    public GameObject currentTile;
    [SerializeField] float groundCheckDistance = 8f;
    List<List<GameObject>> possibleRoutes = new List<List<GameObject>>();
    

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
            CreatePossibleRoutes();
        }
    }
    public virtual List<List<GameObject>> CreatePossibleRoutes()
    {
        possibleRoutes.Clear();
        return possibleRoutes;
    }
    public void CreatePath(GameObject selectedTile)
    {
        bool hasEnemy = grid.TileHasEnemy(selectedTile);
        bool canMoveToTile = false;

        switch(type)
        {
            case PieceType.Pawn:
                break;
        }
    }
    bool CanMoveToTile(GameObject selectedTile)
    {
        bool hasEnemy = grid.TileHasEnemy(selectedTile);
        float dot;
        Vector3 referenceDirection = Vector3.zero;
        Vector3 directionToTarget = currentTile.transform.position - selectedTile.transform.position;
        directionToTarget.Normalize();

        referenceDirection = GetReferenceVector(directionToTarget);

        dot = Vector3.Dot(referenceDirection, directionToTarget);

        switch (type)
        {
            case PieceType.Pawn:
                if (dot == 1f && !hasEnemy) return true;
                
                break;
        }

        return false;
    }

    Vector3 GetReferenceVector(Vector3 direction)
    {
        if (direction.z > 0.5f)
        {
            return Vector3.forward;
        }
        else if (direction.z < 0.5f)
        {
            return Vector3.back;
        }

        if (direction.x > 0.5f)
        {
            return Vector3.right;
        }
        else if (direction.x < 0.5f)
        {
            return Vector3.left;
        }

        return Vector3.zero;
    }
    void Move()
    {
        
    }

    void GroundCheck()
    {
        RaycastHit rayHit;
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = Vector3.down;

        bool hit = Physics.Raycast(ray, out rayHit, groundCheckDistance);
        Debug.DrawLine(ray.origin, ray.origin + Vector3.down * groundCheckDistance, Color.red);

        if (!hit) return;

        currentTile = grid.GetTile(rayHit.collider.gameObject);

        if (currentTile == null) return;
        transform.position = currentTile.transform.position;
    }
}
