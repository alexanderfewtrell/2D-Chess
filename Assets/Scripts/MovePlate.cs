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
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameHelper gameHelper = new GameHelper();

        //checks if the king has been taken
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");

            if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

            gameHelper.UpdateScore(cp.name);

            //destroys pieces when taken
            Destroy(cp);
            GlobalVariables.CurrentPieceTaken = cp.ToString();
        }

        //checks if any pieces can take the king
        controller.GetComponent<Game>().CheckCheck();

        //gets the coordinates
        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(), 
            reference.GetComponent<Chessman>().GetYBoard());

        //Sets the new coordinates
        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        //updates the coordinates
        controller.GetComponent<Game>().SetPosition(reference);

        //checks if any pieces can take the king
        controller.GetComponent<Game>().CheckCheck();

        //changes the player to the opposite player
        controller.GetComponent<Game>().NextTurn();

        //Destroyes any leftover move plates
        reference.GetComponent<Chessman>().DestroyMovePlates();

        //Updates the Database
        DataAccess dataAccess = new DataAccess();
        dataAccess.InsertMove(GlobalVariables.CurrentPiece, gameHelper.CompleteCoordinates(GlobalVariables.XStartCoord.ToString(), GlobalVariables.YStartCoord.ToString()), 
            gameHelper.CompleteCoordinates(matrixX.ToString(), matrixY.ToString()), GlobalVariables.GameId, GlobalVariables.CurrentPieceTaken);

        //resets the values of the variables
        GlobalVariables.CurrentPieceTaken = "none";
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
