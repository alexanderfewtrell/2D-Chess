using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoMove
{
    public GameObject controller;
    public GameObject movePlate;
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;

    public void GoBackMove()
    {
        DataAccess dataAccess = new DataAccess();
        Move move = dataAccess.GetLastRowData(GlobalVariables.GameId);
        int StartXCoord = int.Parse(GetXCoords(move.StartCoords));
        int StartYCoord = int.Parse(GetYCoords(move.StartCoords));

        int EndXCoord = int.Parse(GetXCoords(move.EndCoords));
        int EndYCoord = int.Parse(GetYCoords(move.EndCoords));

        controller = GameObject.FindGameObjectWithTag("GameController");
        GameObject cp = controller.GetComponent<Game>().GetPosition(EndXCoord, EndYCoord);

        Chessman chessman = cp.GetComponent<Chessman>();  
        chessman.MovePlateSpawn(StartXCoord, StartYCoord, cp);

        GameObject movePlateObject = GameObject.FindGameObjectWithTag("MovePlate");
        MovePlate movePlateScript = movePlateObject.GetComponent<MovePlate>();
        GlobalVariables.Undo = true;
        movePlateScript.MakeMove(StartXCoord, StartYCoord, cp);

        //recreates the piece if one was taken
        if (move.PieceTaken != "none")
        {
            GameHelper gameHelper = new GameHelper();
            string PieceColor = ReCreatePiece(EndXCoord, EndYCoord, move.PieceTaken);
            if (PieceColor == "black")
            {
                GlobalVariables.WhiteScore = GlobalVariables.WhiteScore - gameHelper.GetAdditionalScore(move.PieceTaken);
                dataAccess.UpdateScore(GlobalVariables.WhiteScore, "white");
            }
            else
            {
                GlobalVariables.BlackScore = GlobalVariables.BlackScore - gameHelper.GetAdditionalScore(move.PieceTaken);
                dataAccess.UpdateScore(GlobalVariables.BlackScore, "black");
            }
        }

        //delete the last row of the data base to finish the undo move
        dataAccess.DeleteLastRow(move.MoveId);
    }

    public string GetXCoords(string coords)
    {
        string XCoords = coords.Substring(0, 1);

        GameHelper gameHelper = new GameHelper();

        string RealXCoord = gameHelper.UndoXCoordsConverter(XCoords);

        return RealXCoord;
    }

    public string GetYCoords(string coords)
    {
        string YCoords = coords.Substring(1, 1);

        GameHelper gameHelper = new GameHelper();

        string RealYCoord = gameHelper.UndoYCoordsConverter(YCoords);

        return RealYCoord;
    }

    public string ReCreatePiece(int x, int y, string piece)
    {
        Chessman chessman = controller.GetComponent<Chessman>();
        Game game = controller.GetComponent<Game>();

        GameHelper gameHelper = new GameHelper();

        string pieceColor = piece.Substring(0, Math.Min(piece.Length, 5));
        if (pieceColor == "black")
        {
            GameObject[] newPlayerBlackPiece = new GameObject[]
            {
                game.Create(piece,x,y,"black")
            };

            game.playerBlack[gameHelper.CheckIfArrayLocationEmpty(game.playerBlack)] = newPlayerBlackPiece[0];
        }
        else if (pieceColor == "white")
        {
            GameObject[] newPlayerWhitePiece = new GameObject[]
            {
                game.Create(piece,x,y,"white")
            };

            game.playerWhite[gameHelper.CheckIfArrayLocationEmpty(game.playerWhite)] = newPlayerWhitePiece[0];
        }
        game.SetStartingPosition();

        return pieceColor;
    }

    public Sprite PieceTypeConvert(string pieceName)
    {
        switch (pieceName)
        {
            case "black_queen (UnityEngine.GameObject)":
                return black_queen;
            case "white_queen (UnityEngine.GameObject)":
                return white_queen;
            case "black_knight (UnityEngine.GameObject)":
                return black_knight;
            case "white_knight (UnityEngine.GameObject)":
                return white_knight;
            case "black_bishop (UnityEngine.GameObject)":
                return black_bishop;
            case "white_bishop (UnityEngine.GameObject)":
                return white_bishop;
            case "black_rook (UnityEngine.GameObject)":
                return black_rook;
            case "white_rook (UnityEngine.GameObject)":
                return white_rook;
            case "black_pawn (UnityEngine.GameObject)":
                return black_pawn;
            case "white_pawn (UnityEngine.GameObject)":
                return white_pawn;
        }
        return white_pawn;
    }
}
