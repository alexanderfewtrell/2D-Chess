using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AIMove
{
    public Text ButtonText;
    public GameObject Controller;
    public GameObject chesspiece;

    private System.Random random = new System.Random();
    private Transform transform;

    internal float XScaleFactor = 0.7f;
    internal float YScaleFactor = 0.68f;
    internal float XOffset = -2.45f;
    internal float YOffset = -2.35f;

    public void ChangeMode()
    {
        if (GlobalVariables.Mode == "2Player")
        {
            GlobalVariables.Mode = "AI";
        }
        else
        {
            GlobalVariables.Mode = "2Player";
        }
    }

    public void ChangeText()
    {
        if (GlobalVariables.Mode == "2Player")
        {
            GameObject.FindGameObjectWithTag("AIToggleButtonTextTag").GetComponent<Text>().text = "Change to AI Mode";
        }
        else
        {
            GameObject.FindGameObjectWithTag("AIToggleButtonTextTag").GetComponent<Text>().text = "Change to 2 Player Mode";
        }
    }

    public void CreateAllMovePlates()
    {
        GameObject[] blackPieces = GameObject.FindGameObjectsWithTag("black");
        List<GameObject> newBlackPieces = blackPieces.ToList();
        GameHelper gameHelper = new GameHelper();
        for (int i = 0; i < blackPieces.Length; i++)
        {
            if (gameHelper.FixStringFormat(newBlackPieces[i].ToString()) == GlobalVariables.CurrentPieceTaken && newBlackPieces[i].GetComponent<Chessman>().GetXBoard() == GlobalVariables.CurrentPieceTakenXCoord && newBlackPieces[i].GetComponent<Chessman>().GetYBoard() == GlobalVariables.CurrentPieceTakenYCoord)
            {
                newBlackPieces.Remove(newBlackPieces[i]);
                break;
            }
        }

        Controller = GameObject.FindGameObjectWithTag("GameController");

        for (int i = 0; i < newBlackPieces.Count; i++)
        {
            var piece = newBlackPieces[i];
            int PieceXCoord = piece.GetComponent<Chessman>().GetXBoard();
            int PieceYCoord = piece.GetComponent<Chessman>().GetYBoard();

            GameObject cp = Controller.GetComponent<Game>().GetPosition(PieceXCoord, PieceYCoord);

            Chessman chessman = cp.GetComponent<Chessman>();

            chessman.InitiateMovePlates(cp.name, cp);
        }
    }

    public Vector3 FindRandomMovePlate(int randomNumber, GameObject[] movePlatesList)
    { 
        GameObject FinalMovePlate = movePlatesList[randomNumber];

        Transform FMPTransform = FinalMovePlate.transform;

        Vector3 FMPPosition = FMPTransform.position;

        Debug.Log(FMPPosition);

        return FMPPosition;
    }

    public int PickRandomNumber(int length)
    {
        return random.Next(1, length);
    }

    public int ConvertToXBoardCoords(float x)
    {
        x -= XOffset;
        x /= XScaleFactor;

        int intX = (int)Math.Round(x);

        return intX;
    }

    public int ConvertToYBoardCoords(float y)
    {
        y -= YOffset;
        y /= YScaleFactor;

        int intY = (int)Math.Round(y);

        return intY;
    }

    public void MakeMove()
    {
        CreateAllMovePlates();
        GlobalVariables.RandomNumber = PickRandomNumber(GlobalVariables.AIMoveDetailsList.Count);

        int MovePlateXCoord = ConvertToXBoardCoords(GlobalVariables.AIMoveDetailsList[GlobalVariables.RandomNumber].MovePlateXCoord);
        int MovePlateYCoord = ConvertToYBoardCoords(GlobalVariables.AIMoveDetailsList[GlobalVariables.RandomNumber].MovePlateYCoord);

        GameObject Piece = GlobalVariables.AIMoveDetailsList[GlobalVariables.RandomNumber].Piece;
        bool Attack = GlobalVariables.AIMoveDetailsList[GlobalVariables.RandomNumber].Piece;

        GameObject cp = Controller.GetComponent<Game>().GetPosition(Piece.GetComponent<Chessman>().GetXBoard(), Piece.GetComponent<Chessman>().GetYBoard());

        Chessman chessman = cp.GetComponent<Chessman>();
        chessman.DestroyMovePlates();

        if (Attack) chessman.MovePlateAttackSpawn(MovePlateXCoord, MovePlateYCoord, Piece);

        else chessman.MovePlateSpawn(MovePlateXCoord, MovePlateYCoord, Piece);

        GameObject movePlateObject = GameObject.FindGameObjectWithTag("MovePlate");
        MovePlate movePlateScript = movePlateObject.GetComponent<MovePlate>();
        movePlateScript.MakeMove(MovePlateXCoord, MovePlateYCoord, Piece);
    }
}
