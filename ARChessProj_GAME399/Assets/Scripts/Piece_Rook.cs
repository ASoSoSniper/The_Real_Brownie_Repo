using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChessPiece;

public class Piece_Rook : ChessPiece
{
    public override List<List<GameObject>> CreatePossibleRoutes()
    {
        Teams enemy = team == Teams.Black ? Teams.White : Teams.Black;
        Teams ally = team == Teams.Black ? Teams.Black : Teams.White;

        List<List<GameObject>> allRoutes = new List<List<GameObject>>();

        List<GameObject> route1 = new List<GameObject>();
        List<GameObject> route2 = new List<GameObject>();
        List<GameObject> route3 = new List<GameObject>();
        List<GameObject> route4 = new List<GameObject>();

        route1.Add(currentTile);
        route2.Add(currentTile);
        route3.Add(currentTile);
        route4.Add(currentTile);

        int dir = team == Teams.Black ? 1 : -1;
        
        int x = 0;
        int z = 0;
        grid.GetTileCoordinates(currentTile, out x, out z);

        //Route 1: Forward
        int forwardZ = z;
        bool endOfForward = false;
        while (!endOfForward)
        {
            forwardZ += dir;
            if (grid.TileExists(x, forwardZ))
            {
                Teams occupation = grid.GetPieceOnTile(grid.GetTile(x, forwardZ));
                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    endOfForward = true;
                }
                route1.Add(grid.GetTile(x, forwardZ));
            }
            else
            {
                endOfForward = true;
            }
        }
        allRoutes.Add(route1);

        //Route 2: Back
        int backZ = z;
        bool endOfBack = false;
        while (!endOfBack)
        {
            backZ -= dir;
            if (grid.TileExists(x, backZ))
            {
                Teams occupation = grid.GetPieceOnTile(grid.GetTile(x, backZ));
                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    endOfBack = true;
                }
                route2.Add(grid.GetTile(x, backZ));
            }
            else
            {
                endOfBack = true;
            }
        }
        allRoutes.Add(route2);

        //Route 3: Left
        int leftX = x;
        bool endOfLeft = false;
        while (!endOfLeft)
        {
            --leftX;
            if (grid.TileExists(leftX, z))
            {
                Teams occupation = grid.GetPieceOnTile(grid.GetTile(leftX, z));
                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    endOfLeft = true;
                }
                route3.Add(grid.GetTile(leftX, z));
            }
            else
            {
                endOfLeft = true;
            }
        }
        allRoutes.Add(route3);

        //Route 4: Right
        int rightX = x;
        bool endOfRight = false;
        while (!endOfRight)
        {
            ++rightX;
            if (grid.TileExists(rightX, z))
            {
                Teams occupation = grid.GetPieceOnTile(grid.GetTile(rightX, z));
                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    endOfRight = true;
                }
                route4.Add(grid.GetTile(rightX, z));
            }
            else
            {
                endOfRight = true;
            }
        }
        allRoutes.Add(route4);

        foreach (List<GameObject> route in allRoutes)
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
