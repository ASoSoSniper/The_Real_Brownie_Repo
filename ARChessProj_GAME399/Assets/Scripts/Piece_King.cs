using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_King : ChessPiece
{
    public override Dictionary<List<GameObject>, ChessPiece> CreatePossibleRoutes()
    {
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
}
