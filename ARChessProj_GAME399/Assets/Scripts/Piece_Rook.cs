using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChessPiece;

public class Piece_Rook : ChessPiece
{
    public override Dictionary<List<GameObject>, ChessPiece> CreatePossibleRoutes()
    {
        Dictionary<List<GameObject>, ChessPiece> allRoutes = new Dictionary<List<GameObject>, ChessPiece>();

        int dir = team == Teams.Black ? 1 : -1;
        
        int x = 0;
        int z = 0;
        grid.GetTileCoordinates(currentTile, out x, out z);

        //Route 1: Forward
        ChessPiece route1TargetPiece = null;
        List<GameObject> route1 = CreateRouteInDirection(new Vector2(x, z), new Vector2(0, 1), out route1TargetPiece);
        allRoutes.Add(route1, route1TargetPiece);

        //Route 2: Back
        ChessPiece route2TargetPiece = null;
        List<GameObject> route2 = CreateRouteInDirection(new Vector2(x, z), new Vector2(0, -1), out route2TargetPiece);
        allRoutes.Add(route2, route2TargetPiece);

        //Route 3: Left
        ChessPiece route3TargetPiece = null;
        List<GameObject> route3 = CreateRouteInDirection(new Vector2(x, z), new Vector2(-1, 0), out route3TargetPiece);
        allRoutes.Add(route3, route3TargetPiece);

        //Route 4: Right
        ChessPiece route4TargetPiece = null;
        List<GameObject> route4 = CreateRouteInDirection(new Vector2(x, z), new Vector2(1, 0), out route4TargetPiece);
        allRoutes.Add(route4, route4TargetPiece);

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
