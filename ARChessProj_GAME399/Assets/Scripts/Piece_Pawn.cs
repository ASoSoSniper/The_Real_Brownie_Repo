using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Pawn : ChessPiece
{
    public override List<List<GameObject>> CreatePossibleRoutes()
    {
        Teams enemy = team == Teams.Black ? Teams.White : Teams.Black;
        
        List<List<GameObject>> allRoutes = new List<List<GameObject>>();

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

        z += dir;
        if (grid.TileExists(x, z))
        {
            route1.Add(grid.GetTile(x, z));
            allRoutes.Add(route1);
        }

        x += 1;
        if (grid.TileExists(x, z) && grid.GetPieceOnTile(grid.GetTile(x, z)) == enemy)
        {
            route2.Add(grid.GetTile(x, z));
            allRoutes.Add(route2);
        }
        x -= 2;
        if (grid.TileExists(x, z) && grid.GetPieceOnTile(grid.GetTile(x, z)) == enemy)
        {
            route3.Add(grid.GetTile(x, z));
            allRoutes.Add(route3);
        }

        return allRoutes;
    }
}
