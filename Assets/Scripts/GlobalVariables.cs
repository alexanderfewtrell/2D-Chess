using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    //Defines the Global Variables
    public static bool WhiteInCheck = false;
    public static bool BlackInCheck = false;

    public static int XStartCoord = 0;
    public static int YStartCoord = 0;
    public static string CurrentPiece = "";
    public static string CurrentPieceTaken = "none";

    public static long GameId = 0;
    public static int WhiteScore = 0;
    public static int BlackScore = 0;

    public static string currentPlayer = "white";

    public static string Mode = "2Player";
}
