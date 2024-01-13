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
    //public GameObject chesspiece;
    //private GameObject[,] positions = new GameObject[8, 8];
    //private GameObject[] playerBlack = new GameObject[16];
    //private GameObject[] playerWhite = new GameObject[16];

    public void RespawnPiece()
    {
        //respawns the piece
    }

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
        chessman.MovePlateSpawn(StartXCoord, StartYCoord);

        GameObject movePlateObject = GameObject.FindGameObjectWithTag("MovePlate");
        MovePlate movePlateScript = movePlateObject.GetComponent<MovePlate>();
        movePlateScript.MakeMove(StartXCoord, StartYCoord, cp);

        //recreates the piece if one was taken
        //if(move.PieceTaken != "none")
        //{
        //    ReCreatePiece(EndXCoord, EndYCoord);
        //}

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

    public void ReCreatePiece(int x, int y)
    {
        DataAccess dataAccess = new DataAccess();
        Move move = dataAccess.GetLastRowData(GlobalVariables.GameId);
        Chessman chessman = new Chessman();
        Game game = new Game();
        string color = "";
        if (GlobalVariables.currentPlayer == "white")
        {
            color = "black";
        }
        else
        {
            color = "white";
        }
        //GameObject chesspiece = move.PieceTaken
        game.Create(move.PieceTaken, 0, 0, "white");
        chessman.PutPieceOnBoard(PieceTypeConvert(move.PieceTaken), color);
    }

    public Sprite PieceTypeConvert(string pieceName)
    {
        switch (pieceName)
        {
            case "black_queen":
                return black_queen;
            case "white_queen":
                return white_queen;
            case "black_knight":
                return black_knight;
            case "white_knight":
                return white_knight;
            case "black_bishop":
                return black_bishop;
            case "white_bishop":
                return white_bishop;
            case "black_rook":
                return black_rook;
            case "white_rook":
                return white_rook;
            case "black_pawn":
                return black_pawn;
            case "white_pawn":
                return white_pawn;
        }
        return white_pawn;
    }
}
