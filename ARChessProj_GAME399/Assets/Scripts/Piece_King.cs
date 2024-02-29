using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_King : ChessPiece
{
    public override List<List<GameObject>> CreatePossibleRoutes()
    {
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

        //Right
        if (grid.TileExists(x + 1, z))
        {
            GameObject tile = grid.GetTile(x + 1, z);
            Teams occupation = grid.GetPieceOnTile(tile);

            if (occupation != ally)
            {
                route1.Add(tile);
                allRoutes.Add(route1);
            } 
        }
        //Left
        if (grid.TileExists(x - 1, z))
        {
            GameObject tile = grid.GetTile(x - 1, z);
            Teams occupation = grid.GetPieceOnTile(tile);

            if (occupation != ally)
            {
                route2.Add(tile);
                allRoutes.Add(route2);
            }
        }
        //Forward
        if (grid.TileExists(x, z + 1))
        {
            GameObject tile = grid.GetTile(x, z + 1);
            Teams occupation = grid.GetPieceOnTile(tile);

            if (occupation != ally)
            {
                route3.Add(tile);
                allRoutes.Add(route3);
            }
        }
        //Back
        if (grid.TileExists(x, z - 1))
        {
            GameObject tile = grid.GetTile(x, z - 1);
            Teams occupation = grid.GetPieceOnTile(tile);

            if (occupation != ally)
            {
                route4.Add(tile);
                allRoutes.Add(route4);
            }
        }

        //Forward + Right
        if (grid.TileExists(x + 1, z + 1))
        {
            GameObject tile = grid.GetTile(x + 1, z + 1);
            Teams occupation = grid.GetPieceOnTile(tile);

            if (occupation != ally)
            {
                route5.Add(tile);
                allRoutes.Add(route5);
            }
        }
        //Forward + Left
        if (grid.TileExists(x - 1, z + 1))
        {
            GameObject tile = grid.GetTile(x - 1, z + 1);
            Teams occupation = grid.GetPieceOnTile(tile);

            if (occupation != ally)
            {
                route6.Add(tile);
                allRoutes.Add(route6);
            }
        }
        //Back + Right
        if (grid.TileExists(x + 1, z - 1))
        {
            GameObject tile = grid.GetTile(x + 1, z - 1);
            Teams occupation = grid.GetPieceOnTile(tile);

            if (occupation != ally)
            {
                route7.Add(tile);
                allRoutes.Add(route7);
            }
        }
        //Back + Left
        if (grid.TileExists(x - 1, z - 1))
        {
            GameObject tile = grid.GetTile(x - 1, z - 1);
            Teams occupation = grid.GetPieceOnTile(tile);

            if (occupation != ally)
            {
                route8.Add(tile);
                allRoutes.Add(route8);
            }
        }

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
