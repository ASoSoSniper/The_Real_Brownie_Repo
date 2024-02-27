using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChessPiece;

public class Piece_Rook : ChessPiece
{
    public override List<List<GameObject>> CreatePossibleRoutes()
    {
        Teams enemy = team == Teams.Black ? Teams.White : Teams.Black;

        List<List<GameObject>> allRoutes = new List<List<GameObject>>();

        return allRoutes;
    }
}
