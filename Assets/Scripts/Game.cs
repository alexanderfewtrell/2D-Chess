using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    // creates the chesspiece game object that will be used for all of the pieces
    public GameObject chesspiece;
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    //sets the gameOver variable to false as the game is being played
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        //calls the function to create the objects for all of the pieces in the game and passes in variables
        playerWhite = new GameObject[]
        {
            Create("white_rook",0,0,"white"),
            Create("white_knight",1,0,"white"), 
            Create("white_bishop",2,0,"white"), 
            Create("white_queen",3,0,"white"),
            Create("white_king",4,0,"white"), 
            Create("white_bishop",5,0,"white"), 
            Create("white_knight",6,0,"white"), 
            Create("white_rook",7,0,"white"),
            Create("white_pawn",0,1,"white"), Create("white_pawn",1,1,"white"), Create("white_pawn",2,1,"white"), Create("white_pawn",3,1,"white"),
            Create("white_pawn",4,1,"white"), Create("white_pawn",5,1,"white"), Create("white_pawn",6,1,"white"), Create("white_pawn",7,1,"white") };
        playerBlack = new GameObject[]{
            Create("black_rook",0,7,"black"),
            Create("black_knight",1,7,"black"),
            Create("black_bishop",2,7,"black"), 
            Create("black_queen",3,7,"black"),
            Create("black_king",4,7,"black"), 
            Create("black_bishop",5,7,"black"), 
            Create("black_knight",6,7,"black"), 
            Create("black_rook",7,7,"black"),
            Create("black_pawn",0,6,"black"), Create("black_pawn",1,6,"black"), Create("black_pawn",2,6,"black"), Create("black_pawn",3,6,"black"),
            Create("black_pawn",4,6,"black"), Create("black_pawn",5,6,"black"), Create("black_pawn",6,6,"black"), Create("black_pawn",7,6,"black") };

        //sets the pieces positions
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
        GameHelper gameHelper = new GameHelper();
        string gameName = gameHelper.CreateGameName();
        DataAccess dataAccess = new DataAccess();
        dataAccess.NewDataBase();
        GlobalVariables.GameId = dataAccess.AddGameName(gameName);
    }

    //creates the objects for all of the pieces
    public GameObject Create(string name, int x, int y, string tag)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.tag = tag;
        cm.Activate();
        return obj;
    }

    //Gives all of the objects coordinates on the board
    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    //Clears the coordinates so it does not have a position
    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    //returns the positions
    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    //returns true or false depending on the position of the piece
    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 ||y < 0 || x >= positions.GetLength(0) || y>= positions.GetLength(1)) return false;
        return true;

    }

    //returns which players move it is
    public string GetCurrentPlayer()
    {
        return GlobalVariables.currentPlayer;
    }

    //Checks if the game is over and returns true if it is
    public bool IsGameOver()
    {
        return gameOver;
    }

    //swaps the player over for the next move
    public void NextTurn()
    {
        if (GlobalVariables.currentPlayer == "white")
        {
            GlobalVariables.currentPlayer = "black";
        }
        else
        {
            GlobalVariables.currentPlayer = "white";
        }
    }
    
    public void Update()
    {
        //starts the game
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }

        GameObject.FindGameObjectWithTag("WhiteScoreTag").GetComponent<Text>().text = GlobalVariables.WhiteScore.ToString();
        GameObject.FindGameObjectWithTag("BlackScoreTag").GetComponent<Text>().text = GlobalVariables.BlackScore.ToString();

        //ends the game if the escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    //if the king is taken it declares a winner and makes the text appear saying who the winner is
    public void Winner(string playerWinner)
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }

    //exits the unity editor and shuts the game down
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void CheckCheck()
    {
        //sets the check variable to false
        GlobalVariables.InCheck = false;

        //calls the function InCheck function which checks if any pieces on the board can take the king
        InCheck();

        //if the variable InCheck is set to true then the check text is displayed
        if (GlobalVariables.InCheck)
        {
            GameObject.FindGameObjectWithTag("CheckText").GetComponent<Text>().enabled = true;
        }
        else
        {
            GameObject.FindGameObjectWithTag("CheckText").GetComponent<Text>().enabled = false;
        }
    }
    public void InCheck()
    {
        //puts all of the pieces into a list
        object[] AllPieces = GetAllCurrentPieces();
        
        //calls the loop pieces function
        LoopPieces(AllPieces);

    }

    private object[] GetAllCurrentPieces()
    {
        //puts all the white pieces in a list
        object[] whitePieces = GameObject.FindGameObjectsWithTag("white");
        //puts all of the black pieces
        object[] blackPieces = GameObject.FindGameObjectsWithTag("black");
        //creates a list for all of the pieces
        object[] AllPieces = new object[whitePieces.Length + blackPieces.Length];

        //adds all of the black pieces and white pieces to the all pieces list and returns the list
        whitePieces.CopyTo(AllPieces, 0);
        blackPieces.CopyTo(AllPieces, whitePieces.Length);
        return AllPieces;
    }

    public void LoopPieces(object[] obj)
    {
        //loops through all of the pieces and calls canTakeKing for each piece
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (CanTakeKing(g))
            {
                //if any of the pieces can take the king the loop breaks
                break;
            }

        }

    }

    public bool CanTakeKing(GameObject piece)
    {
        Chessman cm = piece.GetComponent<Chessman>();
        
        //runs the InitiateMovePlates function which will change the Check variable to true if any of the pieces can take the king
        cm.InitiateCheckPlates();

        if (GlobalVariables.InCheck)
        {
            //if InCheck is true CanTakeKing returns true
            return true;
        }
        else
        {
            //if InCheck is true CanTakeKing returns false
            return false;
        }
    }
}
