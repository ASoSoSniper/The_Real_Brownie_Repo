using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_King : ChessPiece
{
    [HideInInspector] public Piece_Rook savedLeftRook;
    [HideInInspector] public Piece_Rook savedRightRook;
    [HideInInspector] public List<GameObject> leftCastling;
    [HideInInspector] public List<GameObject> rightCastling;

    public override Dictionary<List<GameObject>, ChessPiece> CreatePossibleRoutes()
    {
        savedLeftRook = null;
        savedRightRook = null;
        leftCastling = null;
        rightCastling = null;

        Dictionary<List<GameObject>, ChessPiece> allRoutes = new Dictionary<List<GameObject>, ChessPiece>();

        int x = 0;
        int z = 0;
        grid.GetTileCoordinates(currentTile, out x, out z);

        //Right
        ChessPiece route1TargetPiece = null;
        List<GameObject> route1 = CreateRoute2Point(x + 1, z, out route1TargetPiece);
        if (route1.Count > 0) allRoutes.Add(route1, route1TargetPiece);

        //Left
        ChessPiece route2TargetPiece = null;
        List<GameObject> route2 = CreateRoute2Point(x - 1, z, out route2TargetPiece);
        if (route2.Count > 0) allRoutes.Add(route2, route2TargetPiece);

        //Forward
        ChessPiece route3TargetPiece = null;
        List<GameObject> route3 = CreateRoute2Point(x, z + 1, out route3TargetPiece);
        if (route3.Count > 0) allRoutes.Add(route3, route3TargetPiece);

        //Back
        ChessPiece route4TargetPiece = null;
        List<GameObject> route4 = CreateRoute2Point(x, z - 1, out route4TargetPiece);
        if (route4.Count > 0) allRoutes.Add(route4, route4TargetPiece);

        //Forward + Right
        ChessPiece route5TargetPiece = null;
        List<GameObject> route5 = CreateRoute2Point(x + 1, z + 1, out route5TargetPiece);
        if (route5.Count > 0) allRoutes.Add(route5, route5TargetPiece);

        //Forward + Left
        ChessPiece route6TargetPiece = null;
        List<GameObject> route6 = CreateRoute2Point(x - 1, z + 1, out route6TargetPiece);
        if (route6.Count > 0) allRoutes.Add(route6, route6TargetPiece);

        //Back + Right
        ChessPiece route7TargetPiece = null;
        List<GameObject> route7 = CreateRoute2Point(x + 1, z - 1, out route7TargetPiece);
        if (route7.Count > 0) allRoutes.Add(route7, route7TargetPiece);

        //Back + Left
        ChessPiece route8TargetPiece = null;
        List<GameObject> route8 = CreateRoute2Point(x - 1, z - 1, out route8TargetPiece);
        if (route8.Count > 0) allRoutes.Add(route8, route8TargetPiece);

        //Castling
        if (!firstMove)
        {
            //Left
            Piece_Rook leftRook = GetRookForCastling(x - 4, z);
            bool leftRookSpotClear = grid.GetPieceOnTile(grid.GetTile(x - 1, z)) == Teams.None;

            List<GameObject> leftRoute = new List<GameObject>();
            bool leftClear = ClearPathForCastling(new Vector2(x, z), new Vector2(x - 2, z), out leftRoute);

            if (leftRook && leftRookSpotClear && leftClear)
            {
                allRoutes.Add(leftRoute, null);
                savedLeftRook = leftRook;
                leftCastling = leftRoute;
            }

            //Right
            Piece_Rook rightRook = GetRookForCastling(x + 3, z);
            bool rightRookSpotClear = grid.GetPieceOnTile(grid.GetTile(x + 1, z)) == Teams.None;

            List<GameObject> rightRoute = new List<GameObject>();
            bool rightClear = ClearPathForCastling(new Vector2(x, z), new Vector2(x + 2, z), out rightRoute);

            if (rightRook && rightRookSpotClear && rightClear)
            {
                allRoutes.Add(rightRoute, null);
                savedRightRook = rightRook;
                rightCastling = rightRoute;
            }
        }

        return allRoutes;
    }

    Piece_Rook GetRookForCastling(int x, int z)
    {
        if (!grid.TileExists(x, z)) return null;

        RaycastHit result;
        bool hit = Physics.Raycast(grid.GetTile(x, z).transform.GetChild(0).transform.position + Vector3.down * 5f, Vector3.up, out result, 10f);

        if (!hit) return null;

        Piece_Rook piece = result.collider.transform.GetComponent<Piece_Rook>();
        if (!piece) return null;

        if (piece.team == ally && !piece.firstMove) return piece;

        return null;
    }

    bool ClearPathForCastling(Vector2 initTile, Vector2 targetTile, out List<GameObject> route)
    {
        if (InCheck())
        {
            route = null;
            return false;
        }

        int step = targetTile.x > initTile.x ? 1 : -1;

        List<GameObject> newRoute = new List<GameObject>();

        newRoute.Add(currentTile);

        int x = (int)initTile.x;
        int z = (int)initTile.y;
        bool endPath = false;

        while (!endPath)
        {
            x += step;
            if (grid.TileExists(x, z))
            {
                GameObject tile = grid.GetTile(x, z);
                Teams occupation = grid.GetPieceOnTile(tile);
                if (occupation != Teams.None || TileUnderAttack(x, z))
                {
                    route = null;
                    return false;
                }

                newRoute.Add(tile);
            }
            else
            {
                route = null;
                return false;
            }

            if (x == targetTile.x) endPath = true;
            
        }

        route = newRoute;
        return true;
    }

    bool TileUnderAttack(int x, int z)
    {
        ChessPiece[] allPieces = FindObjectsOfType<ChessPiece>();
        GameObject tileToCheck = grid.GetTile(x, z);

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

    bool TileUnderAttack(GameObject tileToCheck)
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

    public bool InCheck()
    {
        ChessPiece[] allPieces = FindObjectsOfType<ChessPiece>();

        for (int i = 0; i < allPieces.Length; i++)
        {
            if (allPieces[i].team != enemy || allPieces[i] == this ||
                (allPieces[i].type == PieceType.King && !allPieces[i].firstMove)) continue;

            Dictionary<List<GameObject>, ChessPiece> routes = allPieces[i].CreatePossibleRoutes();
            foreach (ChessPiece piece in routes.Values)
            {
                if (piece == this)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool InCheckMate()
    {
        bool inCheck = InCheck();
        Dictionary<List<GameObject>, ChessPiece> routes = CreatePossibleRoutes();

        bool notInCheckMate = false;

        foreach (List<GameObject> tiles in routes.Keys)
        {
            foreach (GameObject tile in tiles)
            {
                if (!TileUnderAttack(tile))
                {
                    notInCheckMate = true;
                }
            }
        }

        if (inCheck && !notInCheckMate)
        {
            Debug.Log("CheckMate");
            return true;
        }

        return false;
    }
}
