using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Knight : ChessPiece
{
    [SerializeField] float raiseHeight = 10f;
    public override List<List<GameObject>> CreatePossibleRoutes()
    {
        List<List<GameObject>> allRoutes = new List<List<GameObject>>();

        int x = 0;
        int z = 0;
        grid.GetTileCoordinates(currentTile, out x, out z);

        //Top 2
        List<GameObject> route1 = CreateRoute(x + 1, z + 2);
        if (route1.Count > 0) allRoutes.Add(route1);

        List<GameObject> route2 = CreateRoute(x - 1, z + 2);
        if (route2.Count > 0) allRoutes.Add(route2);

        //Right 2
        List<GameObject> route3 = CreateRoute(x + 2, z + 1);
        if (route3.Count > 0) allRoutes.Add(route3);

        List<GameObject> route4 = CreateRoute(x + 2, z - 1);
        if (route4.Count > 0) allRoutes.Add(route4);

        //Left 2
        List<GameObject> route5 = CreateRoute(x - 2, z + 1);
        if (route5.Count > 0) allRoutes.Add(route5);

        List<GameObject> route6 = CreateRoute(x - 2, z - 1);
        if (route6.Count > 0) allRoutes.Add(route6);

        //Bottom 2
        List<GameObject> route7 = CreateRoute(x + 1, z - 2);
        if (route7.Count > 0) allRoutes.Add(route7);

        List<GameObject> route8 = CreateRoute(x - 1, z - 2);
        if (route8.Count > 0) allRoutes.Add(route8);

        return allRoutes;
    }

    List<GameObject> CreateRoute(int xPos, int zPos)
    {
        Teams ally = team == Teams.Black ? Teams.Black : Teams.White;

        List<GameObject> route = new List<GameObject>();

        if (grid.TileExists(xPos, zPos))
        {
            GameObject tile = grid.GetTile(xPos, zPos);
            Teams occupation = grid.GetPieceOnTile(tile);

            if (occupation != ally)
            {
                route.Add(currentTile);
                route.Add(tile);
            }
        }

        return route;
    }

    public override void Move()
    {
        if (!moving) return;

        Vector3 start = transform.position;
        Vector3 end = currentTile.transform.position;

        //transform.position = Vector3.Lerp(start, end, )
    }
}
