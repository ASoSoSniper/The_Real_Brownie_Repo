using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Queen : ChessPiece
{
    public override Dictionary<List<GameObject>, ChessPiece> CreatePossibleRoutes()
    {
        Dictionary<List<GameObject>, ChessPiece> allRoutes = new Dictionary<List<GameObject>, ChessPiece>();

        int x = 0;
        int z = 0;
        grid.GetTileCoordinates(currentTile, out x, out z);

        //Forward
        ChessPiece route1TargetPiece = null;
        List<GameObject> route1 = CreateRouteInDirection(new Vector2(x, z), new Vector2(0, 1), out route1TargetPiece);
        allRoutes.Add(route1, route1TargetPiece);

        //Back
        ChessPiece route2TargetPiece = null;
        List<GameObject> route2 = CreateRouteInDirection(new Vector2(x, z), new Vector2(0, -1), out route2TargetPiece);
        allRoutes.Add(route2, route2TargetPiece);

        //Left
        ChessPiece route3TargetPiece = null;
        List<GameObject> route3 = CreateRouteInDirection(new Vector2(x, z), new Vector2(-1, 0), out route3TargetPiece);
        allRoutes.Add(route3, route3TargetPiece);

        //Right
        ChessPiece route4TargetPiece = null;
        List<GameObject> route4 = CreateRouteInDirection(new Vector2(x, z), new Vector2(1, 0), out route4TargetPiece);
        allRoutes.Add(route4, route4TargetPiece);

        //Forward + Right
        ChessPiece route5TargetPiece = null;
        List<GameObject> route5 = CreateRouteInDirection(new Vector2(x, z), new Vector2(1, 1), out route5TargetPiece);
        allRoutes.Add(route5, route5TargetPiece);

        //Forward + Left
        ChessPiece route6TargetPiece = null;
        List<GameObject> route6 = CreateRouteInDirection(new Vector2(x, z), new Vector2(-1, 1), out route6TargetPiece);
        allRoutes.Add(route6, route6TargetPiece);

        //Back + Right
        ChessPiece route7TargetPiece = null;
        List<GameObject> route7 = CreateRouteInDirection(new Vector2(x, z), new Vector2(1, -1), out route7TargetPiece);
        allRoutes.Add(route7, route7TargetPiece);

        //Back + Left
        ChessPiece route8TargetPiece = null;
        List<GameObject> route8 = CreateRouteInDirection(new Vector2(x, z), new Vector2(-1, -1), out route8TargetPiece);
        allRoutes.Add(route8, route8TargetPiece);

        return allRoutes;
    }
}
