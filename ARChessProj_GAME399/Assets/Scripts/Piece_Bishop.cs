using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Bishop : ChessPiece
{
    public override Dictionary<List<GameObject>, ChessPiece> CreatePossibleRoutes()
    {
        Dictionary<List<GameObject>, ChessPiece> allRoutes = new Dictionary<List<GameObject>, ChessPiece>();

        int x = 0;
        int z = 0;
        grid.GetTileCoordinates(currentTile, out x, out z);

        //Forward + Left
        ChessPiece route1TargetPiece = null;
        List<GameObject> route1 = CreateRouteInDirection(new Vector2(x, z), new Vector2(-1, 1), out route1TargetPiece);
        allRoutes.Add(route1, route1TargetPiece);

        //Forward + Right
        ChessPiece route2TargetPiece = null;
        List<GameObject> route2 = CreateRouteInDirection(new Vector2(x, z), new Vector2(1, 1), out route2TargetPiece);
        allRoutes.Add(route2, route2TargetPiece);

        //Back + Left
        ChessPiece route3TargetPiece = null;
        List<GameObject> route3 = CreateRouteInDirection(new Vector2(x, z), new Vector2(-1, -1), out route3TargetPiece);
        allRoutes.Add(route3, route3TargetPiece);

        //Back + Right
        ChessPiece route4TargetPiece = null;
        List<GameObject> route4 = CreateRouteInDirection(new Vector2(x, z), new Vector2(1, -1), out route4TargetPiece);
        allRoutes.Add(route4, route4TargetPiece);

        return allRoutes;
    }


}
