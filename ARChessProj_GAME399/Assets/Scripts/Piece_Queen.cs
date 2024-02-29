using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Queen : ChessPiece
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
        List<GameObject> route5 = new List<GameObject>();
        List<GameObject> route6 = new List<GameObject>();
        List<GameObject> route7 = new List<GameObject>();
        List<GameObject> route8 = new List<GameObject>();

        route1.Add(currentTile);
        route2.Add(currentTile);
        route3.Add(currentTile);
        route4.Add(currentTile);
        route5.Add(currentTile);
        route6.Add(currentTile);
        route7.Add(currentTile);
        route8.Add(currentTile);

        int x = 0;
        int z = 0;
        grid.GetTileCoordinates(currentTile, out x, out z);

        //Forward
        int forZ = z;
        bool forEnd = false;

        while (!forEnd)
        {
            ++forZ;
            if (grid.TileExists(x, forZ))
            {
                GameObject tile = grid.GetTile(x, forZ);
                Teams occupation = grid.GetPieceOnTile(tile);

                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    forEnd = true;
                }
                route1.Add(tile);
            }
            else
            {
                forEnd = true;
            }
        }
        allRoutes.Add(route1);

        //Back
        int backZ = z;
        bool backEnd = false;

        while (!backEnd)
        {
            --backZ;
            if (grid.TileExists(x, backZ))
            {
                GameObject tile = grid.GetTile(x, backZ);
                Teams occupation = grid.GetPieceOnTile(tile);

                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    backEnd = true;
                }
                route2.Add(tile);
            }
            else
            {
                backEnd = true;
            }
        }
        allRoutes.Add(route2);

        //Left
        int leftX = x;
        bool leftEnd = false;

        while (!leftEnd)
        {
            --leftX;
            if (grid.TileExists(leftX, z))
            {
                GameObject tile = grid.GetTile(leftX, z);
                Teams occupation = grid.GetPieceOnTile(tile);

                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    leftEnd = true;
                }
                route3.Add(tile);
            }
            else
            {
                leftEnd = true;
            }
        }
        allRoutes.Add(route3);

        //Right
        int rightX = x;
        bool rightEnd = false;

        while (!rightEnd) 
        {
            ++rightX;
            if (grid.TileExists(rightX, z))
            {
                GameObject tile = grid.GetTile(rightX, z);
                Teams occupation = grid.GetPieceOnTile(tile);

                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    rightEnd = true;
                }
                route4.Add(tile);
            }
            else
            {
                rightEnd = true;
            }
        }
        allRoutes.Add(route4);

        //Forward + Right
        int forRightX = x;
        int forRightZ = z;
        bool forRightEnd = false;

        while (!forRightEnd)
        {
            ++forRightX;
            ++forRightZ;
            if (grid.TileExists(forRightX, forRightZ))
            {
                GameObject tile = grid.GetTile(forRightX, forRightZ);
                Teams occupation = grid.GetPieceOnTile(tile);

                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    forRightEnd = true;
                }
                route5.Add(tile);
            }
            else
            {
                forRightEnd = true;
            }
        }
        allRoutes.Add(route5);

        //Forward + Left
        int forLeftX = x;
        int forLeftZ = z;
        bool forLeftEnd = false;

        while (!forLeftEnd)
        {
            --forLeftX;
            ++forLeftZ;
            if (grid.TileExists(forLeftX, forLeftZ))
            {
                GameObject tile = grid.GetTile(forLeftX, forLeftZ);
                Teams occupation = grid.GetPieceOnTile(tile);

                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    forLeftEnd = true;
                }
                route6.Add(tile);
            }
            else
            {
                forLeftEnd = true;
            }
        }
        allRoutes.Add(route6);

        //Back + Right
        int backRightX = x;
        int backRightZ = z;
        bool backRightEnd = false;

        while (!backRightEnd)
        {
            ++backRightX;
            --backRightZ;
            if (grid.TileExists(backRightX, backRightZ))
            {
                GameObject tile = grid.GetTile(backRightX, backRightZ);
                Teams occupation = grid.GetPieceOnTile(tile);

                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    backRightEnd = true;
                }
                route7.Add(tile);
            }
            else
            {
                backRightEnd = true;
            }
        }
        allRoutes.Add(route7);

        //Back + Left
        int backLeftX = x;
        int backLeftZ = z;
        bool backLeftEnd = false;

        while (!backLeftEnd)
        {
            --backLeftX;
            --backLeftZ;
            if (grid.TileExists(backLeftX, backLeftZ))
            {
                GameObject tile = grid.GetTile(backLeftX, backLeftZ);
                Teams occupation = grid.GetPieceOnTile(tile);

                if (occupation == ally) break;

                if (occupation == enemy)
                {
                    backLeftEnd = true;
                }
                route8.Add(tile);
            }
            else
            {
                backLeftEnd = true;
            }
        }
        allRoutes.Add(route8);

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
