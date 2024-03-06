using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Pawn : ChessPiece
{
    int moveCount = 0;
    [SerializeField] bool firstMove = false;
    public override Dictionary<List<GameObject>, ChessPiece> CreatePossibleRoutes()
    {
        Dictionary<List<GameObject>, ChessPiece> allRoutes = new Dictionary<List<GameObject>, ChessPiece>();

        List<GameObject> route1 = new List<GameObject>();
        List<GameObject> route2 = new List<GameObject>();
        List<GameObject> route3 = new List<GameObject>();

        int dir = team == Teams.Black ? 1 : -1;
        int x = 0;
        int z = 0;

        grid.GetTileCoordinates(currentTile, out x, out z);
        route1.Add(currentTile);
        route2.Add(currentTile);
        route3.Add(currentTile);

        //Forward
        GameObject forward1 = CreateStep(x, z + dir, Teams.None);
        if (forward1) route1.Add(forward1);

        //Second Forward (if first move)
        if (!firstMove)
        {
            GameObject forward2 = CreateStep(x, z + dir * 2, Teams.None);
            if (forward2) route1.Add(forward2);
        }

        //Forward Left
        GameObject forwardLeft = CreateStep(x - 1, z + dir, enemy);

        ChessPiece forwardLeftTarget = grid.GetChessPiece(x - 1, z + dir, enemy);
        Piece_Pawn enPassantLeft = GetEnemyPawn(x - 1, z, enemy);
        if (forwardLeft || enPassantLeft) route2.Add(grid.GetTile(x - 1, z + dir));

        //Forward Right
        GameObject forwardRight = CreateStep(x + 1, z + dir, enemy);

        ChessPiece forwardRightTarget = grid.GetChessPiece(x + 1, z + dir, enemy);
        Piece_Pawn enPassantRight = GetEnemyPawn(x + 1, z, enemy);
        if (forwardRight || enPassantRight) route3.Add(grid.GetTile(x + 1, z + dir));

        allRoutes.Add(route1, null);
        allRoutes.Add(route2, enPassantLeft ? enPassantLeft : forwardLeftTarget);
        allRoutes.Add(route3, enPassantRight ? enPassantRight : forwardRightTarget);

        foreach (List<GameObject> route in allRoutes.Keys)
        {
            foreach (GameObject tile in route)
            {
                if (tile != currentTile)
                    tile.GetComponentInChildren<Highlight>().SetAsRouteTile(true, grid.TileHasEnemy(tile, this));
            }
        }

        return allRoutes;
    }

    GameObject CreateStep(int x, int z, Teams teamCheck)
    {
        if (grid.TileExists(x, z))
        {
            GameObject tile = grid.GetTile(x, z);
            if (grid.GetPieceOnTile(tile) == teamCheck)
            {
                return tile;
            }
        }

        return null;
    }

    Piece_Pawn GetEnemyPawn(int x, int z, Teams enemy)
    {
        RaycastHit result;
        bool hit = Physics.Raycast(grid.GetTile(x, z).transform.GetChild(0).transform.position + Vector3.down * 5f, Vector3.up, out result, 10f);

        if (!hit) return null;
        
        Piece_Pawn piece = result.collider.transform.GetComponent<Piece_Pawn>();
        if (!piece) return null;

        if (piece.team == enemy && piece.moveCount == 2) return piece;

        return null;
    }

    public override bool Move()
    {
        if (!base.Move()) return false;

        if (!firstMove) firstMove = true;

        return true;
    }

    public override bool SelectRoute(GameObject gameObject)
    {
        if (!base.SelectRoute(gameObject)) return false;

        moveCount = selectedTileIndex;

        return true;
    }
}
