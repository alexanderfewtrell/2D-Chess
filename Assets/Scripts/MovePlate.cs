using System.Collections;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    //creates the controller game object
    public GameObject controller;

    GameObject reference = null;

    //Board positions, not world positions
    int matrixX;
    int matrixY;

    // false: movement, true: attack
    public bool attack = false;

    public void Start()
    {
        //changes the attack move plate color to red
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        MakeMove(matrixX, matrixY, reference);
        //Updates the Database
        GameHelper gameHelper = new GameHelper();
        DataAccess dataAccess = new DataAccess();
        dataAccess.InsertMove(GlobalVariables.CurrentPiece, gameHelper.CompleteCoordinates(GlobalVariables.XStartCoord.ToString(), GlobalVariables.YStartCoord.ToString()),
            gameHelper.CompleteCoordinates(matrixX.ToString(), matrixY.ToString()), GlobalVariables.GameId, GlobalVariables.CurrentPieceTaken);
        ///////////////
        //END OF TURN//
        ///////////////
        if (GlobalVariables.currentPlayer == "black" && GlobalVariables.Mode == "AI")
        {
            AIMove aiMove = new AIMove();
            aiMove.MakeMove();
        }
    }

    public void MakeMove(int x, int y, GameObject chessPiece)
    {
        Debug.Log("X" + x + GlobalVariables.Mode);
        Debug.Log("Y" + y + GlobalVariables.Mode);
        Debug.Log(chessPiece + GlobalVariables.Mode);
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameHelper gameHelper = new GameHelper();

        //checks if the king has been taken
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(x, y);

            if (cp != null)
            {
                //if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");

                //if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

                gameHelper.UpdateScore(cp.name);

                //destroys pieces when taken
                Destroy(cp);
                GlobalVariables.CurrentPieceTaken = gameHelper.FixStringFormat(cp.ToString());
                GlobalVariables.CurrentPieceTakenXCoord = x;
                GlobalVariables.CurrentPieceTakenYCoord = y;
            }
        }
        else
        {
            //if there is no piece taken then CurrentPieceTaken is set to none
            GlobalVariables.CurrentPieceTaken = "none";
        }
        if(GlobalVariables.currentPlayer == "black" && GlobalVariables.Mode == "AI")
        {
            if (GlobalVariables.AIMoveDetailsList[GlobalVariables.RandomNumber].Attack)
            {
                GameObject cp = controller.GetComponent<Game>().GetPosition(x, y);
                Destroy(cp);
                //Debug.Log("Destroy"+cp.ToString());
                GlobalVariables.CurrentPieceTaken = gameHelper.FixStringFormat(cp.ToString());
                GlobalVariables.CurrentPieceTakenXCoord = x;
                GlobalVariables.CurrentPieceTakenYCoord = y;
            }
        }

        //checks if any pieces can take the king
        controller.GetComponent<Game>().CheckCheck();

        //gets the coordinates
        controller.GetComponent<Game>().SetPositionEmpty(chessPiece.GetComponent<Chessman>().GetXBoard(),
            chessPiece.GetComponent<Chessman>().GetYBoard());

        //Sets the new coordinates
        chessPiece.GetComponent<Chessman>().SetXBoard(x);
        chessPiece.GetComponent<Chessman>().SetYBoard(y);
        chessPiece.GetComponent<Chessman>().SetCoords();

        //updates the coordinates
        controller.GetComponent<Game>().SetPosition(chessPiece);

        //checks if any pieces can take the king
        controller.GetComponent<Game>().CheckCheck();

        //Destroyes any leftover move plates
        chessPiece.GetComponent<Chessman>().DestroyMovePlates();

        GlobalVariables.MovePlateGameObjectList.Clear();

        //changes the player to the opposite player
        controller.GetComponent<Game>().NextTurn();

        GlobalVariables.AIMoveDetailsList.Clear();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
