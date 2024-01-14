using System;
using UnityEngine;

public class GameHelper
{
    // sets game name to the current date and time
    public string CreateGameName()
    {
        DateTime CurrentTime = DateTime.Now;

        return CurrentTime.ToString();
    }

    // gets the current score for the color from the database and increments the value then updates it and puts it back in the database
    public void UpdateScore(string piece)
    {
        DataAccess dataAccess = new DataAccess();
        int CurrentScore = dataAccess.GetScore(GlobalVariables.currentPlayer);

        int AdditionalScore = GetAdditionalScore(piece);

        int NewScore = CurrentScore + AdditionalScore;
        dataAccess.UpdateScore(NewScore, GlobalVariables.currentPlayer);
        switch (GlobalVariables.currentPlayer)
        {
            case"white":
                GlobalVariables.WhiteScore = NewScore;
                break;
            case "black":
                GlobalVariables.BlackScore = NewScore;
                break;
        }
    }

    //gives each pieces numerical values for how many points they are worth
    public int GetAdditionalScore(string piece)
    {
        switch (piece)
        {
            case "black_queen":
            case "white_queen":
                return 9;

            case "black_knight":
            case "white_knight":
            case "black_bishop":
            case "white_bishop":
                return 3;

            case "black_rook":
            case "white_rook":

                return 5;

            case "black_pawn":
            case "white_pawn":
                return 1;
        }
        return 0;
    }

    public string XCoordsConverter(string x)
    {
        switch (x)
        {
            case "0":
                return "A";
            case "1":
                return "B";
            case "2":
                return "C";
            case "3":
                return "D";
            case "4":
                return "E";
            case "5":
                return "F";
            case "6":
                return "G";
            case "7":
                return "H";
        }
        return "";
    }

    public string UndoXCoordsConverter(string x)
    {
        switch (x)
        {
            case "A":
                return "0";
            case "B":
                return "1";
            case "C":
                return "2";
            case "D":
                return "3";
            case "E":
                return "4";
            case "F":
                return "5";
            case "G":
                return "6";
            case "H":
                return "7";
        }
        return "";
    }

    public string YCoordsConverter(string y)
    {
        switch (y)
        {
            case "0":
                return "1";
            case "1":
                return "2";
            case "2":
                return "3";
            case "3":
                return "4";
            case "4":
                return "5";
            case "5":
                return "6";
            case "6":
                return "7";
            case "7":
                return "8";
        }
        return "";
    }

    public string UndoYCoordsConverter(string y)
    {
        switch (y)
        {
            case "1":
                return "0";
            case "2":
                return "1";
            case "3":
                return "2";
            case "4":
                return "3";
            case "5":
                return "4";
            case "6":
                return "5";
            case "7":
                return "6";
            case "8":
                return "7";
        }
        return "";
    }

    public string CompleteCoordinates(string x,string y)
    {
        string XCoords = XCoordsConverter(x);
        string YCoords = YCoordsConverter(y);
        string CompleteCoords = XCoords + YCoords;
        return CompleteCoords;
    }

    public string FixStringFormat(string pieceName)
    {
        switch (pieceName)
        {
            case "black_queen (UnityEngine.GameObject)":
                return "black_queen";
            case "white_queen (UnityEngine.GameObject)":
                return "white_queen";
            case "black_knight (UnityEngine.GameObject)":
                return "black_knight";
            case "white_knight (UnityEngine.GameObject)":
                return "white_knight";
            case "black_bishop (UnityEngine.GameObject)":
                return "black_bishop";
            case "white_bishop (UnityEngine.GameObject)":
                return "white_bishop";
            case "black_rook (UnityEngine.GameObject)":
                return "black_rook";
            case "white_rook (UnityEngine.GameObject)":
                return "white_rook";
            case "black_pawn (UnityEngine.GameObject)":
                return "black_pawn";
            case "white_pawn (UnityEngine.GameObject)":
                return "white_pawn";
        }
        return "white_pawn";
    }

    public int CheckIfArrayLocationEmpty(GameObject[] listToCheck)
    {
        int i = 0;
        foreach (GameObject obj in listToCheck)
        {
            if(obj == null)
            {
                return i;
            }
            i = i + 1;
        }
        return 100;
    }
}
