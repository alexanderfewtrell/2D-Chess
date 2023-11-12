using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject chesspiece;

    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    private string currentPlayer = "white";

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
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

        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

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

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 ||y < 0 || x >= positions.GetLength(0) || y>= positions.GetLength(1)) return false;
        return true;

    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    public void Update()
    {
        //CheckCheck();
        //DestroyMovePlates();
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void CheckCheck()
    {
        GlobalVariables.InCheck = false;

        InCheck();

        if (GlobalVariables.InCheck)
        {
            Debug.Log("Turn Check text on");
            GameObject.FindGameObjectWithTag("CheckText").GetComponent<Text>().enabled = true;
        }
        else
        {
            Debug.Log("didnt turn check on");
            GameObject.FindGameObjectWithTag("CheckText").GetComponent<Text>().enabled = false;
        }
    }
    public void InCheck()
    {
        object[] whitePieces = GameObject.FindGameObjectsWithTag("white");
        object[] blackPieces = GameObject.FindGameObjectsWithTag("black");
        object[] AllPieces = new object[whitePieces.Length + blackPieces.Length];

        whitePieces.CopyTo(AllPieces, 0);
        blackPieces.CopyTo(AllPieces, whitePieces.Length);

        LoopPieces(AllPieces);

    }

    public void MovePlateCheck()
    {
        object[] AllMovePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        LoopPieces(AllMovePlates);
    }

    public void LoopPieces(object[] obj)
    {
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (CanTakeKing(g))
            {
                break;
            }

        }

    }

    public bool CanTakeKing(GameObject piece)
    {
        Chessman cm = piece.GetComponent<Chessman>();
        
        cm.InitiateMovePlates();

        if (GlobalVariables.InCheck)
        {
            //Debug.Log("Can take king true");
            return true;
        }
        else
        {
            //Debug.Log("can take king false");
            return false;
        }
    }
}
