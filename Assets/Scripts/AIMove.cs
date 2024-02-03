using System;
using System.Collections;
using System.Collections.Generic;
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

    public GameObject[] CreateListOfMovePlates()
    {
        GameObject[] blackPieces = GameObject.FindGameObjectsWithTag("black");

        Controller = GameObject.FindGameObjectWithTag("GameController");

        for (int i = 0; i < blackPieces.Length; i++)
        {
            var piece = blackPieces[i];
            int PieceXCoord = piece.GetComponent<Chessman>().GetXBoard();
            int PieceYCoord = piece.GetComponent<Chessman>().GetYBoard();

            GameObject cp = Controller.GetComponent<Game>().GetPosition(PieceXCoord, PieceYCoord);

            Chessman chessman = cp.GetComponent<Chessman>();

            chessman.InitiateMovePlates(cp.name);
        }

        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        return movePlates;
    }

    public Vector3 FindRandomMovePlate()
    {
        GameObject[] movePlatesList = CreateListOfMovePlates();

        int RandomMovePlateNumber = random.Next(1, movePlatesList.Length);
        Debug.Log(RandomMovePlateNumber);

        GameObject FinalMovePlate = movePlatesList[RandomMovePlateNumber];

        Transform FMPTransform = FinalMovePlate.transform;

        Vector3 FMPPosition = FMPTransform.position;

        Debug.Log(FMPPosition);

        return FMPPosition;
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
        Debug.Log("MakeMove)");

        Vector3 RandomMovePlate = FindRandomMovePlate();
        int MovePlateXCoord = ConvertToXBoardCoords(RandomMovePlate.x);
        int MovePlateYCoord = ConvertToYBoardCoords(RandomMovePlate.y);

        Debug.Log(MovePlateXCoord);
        Debug.Log(MovePlateYCoord);

        //MovePlate movePlate = Controller.GetComponent<MovePlate>();
        //movePlate.MakeMove(MovePlateXCoord, MovePlateYCoord, piece);
    }
}
