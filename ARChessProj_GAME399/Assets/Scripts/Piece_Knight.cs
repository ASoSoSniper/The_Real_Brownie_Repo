using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Piece_Knight : ChessPiece
{
    [SerializeField] float raiseHeight = 10f;
    [SerializeField] float raiseTime = 1f;
    int movePhase = 0;
    public override Dictionary<List<GameObject>, ChessPiece> CreatePossibleRoutes()
    {
        Dictionary<List<GameObject>, ChessPiece> allRoutes = new Dictionary<List<GameObject>, ChessPiece>();

        int x = 0;
        int z = 0;
        grid.GetTileCoordinates(currentTile, out x, out z);

        //Top 2
        ChessPiece route1TargetPiece = null;
        List<GameObject> route1 = CreateRoute2Point(x + 1, z + 2, out route1TargetPiece);
        if (route1.Count > 0) allRoutes.Add(route1, route1TargetPiece);

        ChessPiece route2TargetPiece = null;
        List<GameObject> route2 = CreateRoute2Point(x - 1, z + 2, out route2TargetPiece);
        if (route2.Count > 0) allRoutes.Add(route2, route2TargetPiece);

        //Right 2
        ChessPiece route3TargetPiece = null;
        List<GameObject> route3 = CreateRoute2Point(x + 2, z + 1, out route3TargetPiece);
        if (route3.Count > 0) allRoutes.Add(route3, route3TargetPiece);

        ChessPiece route4TargetPiece = null;
        List<GameObject> route4 = CreateRoute2Point(x + 2, z - 1, out route4TargetPiece);
        if (route4.Count > 0) allRoutes.Add(route4, route4TargetPiece);

        //Left 2
        ChessPiece route5TargetPiece = null;
        List<GameObject> route5 = CreateRoute2Point(x - 2, z + 1, out route5TargetPiece);
        if (route5.Count > 0) allRoutes.Add(route5, route5TargetPiece);

        ChessPiece route6TargetPiece = null;
        List<GameObject> route6 = CreateRoute2Point(x - 2, z - 1, out route6TargetPiece);
        if (route6.Count > 0) allRoutes.Add(route6, route6TargetPiece);

        //Bottom 2
        ChessPiece route7TargetPiece = null;
        List<GameObject> route7 = CreateRoute2Point(x + 1, z - 2, out route7TargetPiece);
        if (route7.Count > 0) allRoutes.Add(route7, route7TargetPiece);

        ChessPiece route8TargetPiece = null;
        List<GameObject> route8 = CreateRoute2Point(x - 1, z - 2, out route8TargetPiece);
        if (route8.Count > 0) allRoutes.Add(route8, route8TargetPiece);

        return allRoutes;
    }

    public override bool Move()
    {
        if (!moving) return false;
        switch(movePhase)
        {
            case 0:
                Vector3 raiseStart = transform.position;
                Vector3 raiseEnd = currentTile.transform.position + Vector3.up * raiseHeight;
                transform.position = Vector3.Lerp(raiseStart, raiseEnd, raiseTime * Time.deltaTime);

                if (Vector3.Distance(transform.position, raiseEnd) < 0.1f)
                {
                    movePhase = 1;
                }
                break;
            case 1:
                Vector3 startPoint = transform.position;
                Vector3 endPoint = selectedRoute[selectedTileIndex].transform.GetChild(0).position + Vector3.up * raiseHeight;

                transform.position = Vector3.Lerp(startPoint, endPoint, moveTime * Time.deltaTime);
                if (Vector3.Distance(transform.position, endPoint) < 0.1f)
                {
                    movePhase = 2;
                }
                break;
            case 2:
                Vector3 lowerStart = transform.position;
                Vector3 lowerEnd = selectedRoute[selectedTileIndex].transform.GetChild(0).position;
                transform.position = Vector3.Lerp(lowerStart, lowerEnd, raiseTime * Time.deltaTime);

                if (Vector3.Distance(transform.position, lowerEnd) < 0.1f)
                {
                    currentTile = null;
                    movePhase = 0;
                    moving = false;

                    if (targetPiece)
                    {
                        Debug.Log("Destroyed target");
                        DestroyLogic(targetPiece.gameObject);
                    }
                }
                break;

        }

        return true;
    }
}
