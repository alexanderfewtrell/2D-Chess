using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoMove
{
    public GameObject controller;
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

        cp.GetComponent<Chessman>().SetXBoard(StartXCoord);
        cp.GetComponent<Chessman>().SetYBoard(StartYCoord);
        cp.GetComponent<Chessman>().SetCoords();

        //updates the coordinates
        controller.GetComponent<Game>().SetPosition(cp);

        //undos the turn and sets it back to the original player
        //controller.GetComponent<Game>().NextTurn();

        //recreates the piece that was taken
        //if(move.PieceTaken != "none")
        //{
        //    this.GetComponent<SpriteRenderer>().sprite = move.PieceTaken; player = GlobalVariables.currentPlayer;
        //    Game game = new Game();
        //    game.Create(move.PieceTaken, EndXCoord, EndXCoord, GlobalVariables.currentPlayer);
        //}

        //undos the turn and sets it back to the original player
        controller.GetComponent<Game>().NextTurn();

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
}
