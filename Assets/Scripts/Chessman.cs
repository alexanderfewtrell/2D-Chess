using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    //References
    public GameObject controller;
    public GameObject movePlate;
    internal float XScaleFactor = 0.7f;
    internal float YScaleFactor = 0.68f;
    internal float XOffset = -2.45f;
    internal float YOffset = -2.35f;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //Variable to keep track of weather the player is black or white
    private string player;

    //References for all the sprites that the chesspiece can be
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;

    public void Activate()
    {
        //creates the controller game object
        controller = GameObject.FindGameObjectWithTag("GameController");

        //take te instantiated location and adjust the transform
        SetCoords();

        //puts all of the pieces on the board
        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;

            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= XScaleFactor;
        y *= YScaleFactor;

        x += XOffset;
        y += YOffset;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            //Check = false;
            controller.GetComponent<Game>().CheckCheck();

            DestroyMovePlates();

            InitiateMovePlates();

        }
    }

    public void DestroyMovePlates()
    {
        //adds all of the move plates
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        //loops through all of the move plates and destroys them
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }


    #region MovePlates
    public void InitiateMovePlates()
    {
        //creates different move plate patterns depending on which piece is selected
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                LineMovePlate(1, 0);
                LineMovePlate(-1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(0, -1);
                break;

            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;

            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;

            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;

            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(-1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(0, -1);
                break;

            case "black_pawn":
                //if the pawn is in its starting position it spawns 2 move plates
                if (yBoard == 6)
                {
                    PawnDoubleMovePlate(xBoard, yBoard - 2);
                    PawnMovePlate(xBoard, yBoard - 1);
                }

                //if the pawn is not in its starting position it spawns its normal moveplates
                if (yBoard != 6)
                {
                    PawnMovePlate(xBoard, yBoard - 1);
                }

                break;

            case "white_pawn":
                //if the pawn is in its starting position it spawns 2 move plates
                if (yBoard == 1)
                {
                    PawnDoubleMovePlate(xBoard, yBoard + 2);
                    PawnMovePlate(xBoard, yBoard + 1);
                }

                //if the pawn is not in its starting position it spawns its normal moveplates
                if (yBoard != 1)
                {
                    PawnMovePlate(xBoard, yBoard + 1);
                }
                break;

        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        //adjusts the coordinates
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        //if the square is empty a move plate is spawned
        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        //checks if there is an enemy piece on the square
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(x, y);

            if (cp.name != "white_king" && cp.name != "black_king")
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    //defines the knights movement pattern
    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    //defines the kings movement pattern
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                //if the square is empty spawn a move plate
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                if (cp.name != "white_king" && cp.name != "black_king")
                {
                    MovePlateAttackSpawn(x, y);
                }
            }
        }
    }

    public void PawnDoubleMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                //spawns in move plate if the square is empty
                MovePlateSpawn(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            //spawns in move plate if the square is empty
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }

            //chacks for pieces and spawns in a red move plate if there are pieces
            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null
                && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                GameObject cp = controller.GetComponent<Game>().GetPosition(x + 1, y);

                if (cp.name != "white_king" && cp.name != "black_king")
                {
                    MovePlateAttackSpawn(x + 1, y);
                }
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                GameObject cp = controller.GetComponent<Game>().GetPosition(x - 1, y);

                if (cp.name != "white_king" && cp.name != "black_king")
                {
                    MovePlateAttackSpawn(x - 1, y);
                }
            }
        }
    }
    #endregion

    #region CheckPlates
    public void InitiateCheckPlates()
    {
        //creates different Check plate patterns depending on which piece is selected
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineCheckPlate(1, 1);
                LineCheckPlate(1, -1);
                LineCheckPlate(-1, 1);
                LineCheckPlate(-1, -1);
                LineCheckPlate(1, 0);
                LineCheckPlate(-1, 0);
                LineCheckPlate(0, 1);
                LineCheckPlate(0, -1);
                break;

            case "black_knight":
            case "white_knight":
                LCheckPlate();
                break;

            case "black_bishop":
            case "white_bishop":
                LineCheckPlate(1, 1);
                LineCheckPlate(1, -1);
                LineCheckPlate(-1, 1);
                LineCheckPlate(-1, -1);
                break;

            case "black_king":
            case "white_king":
                SurroundCheckPlate();
                break;

            case "black_rook":
            case "white_rook":
                LineCheckPlate(1, 0);
                LineCheckPlate(-1, 0);
                LineCheckPlate(0, 1);
                LineCheckPlate(0, -1);
                break;

            case "black_pawn":
                //if the pawn is not in its starting position it spawns its normal Checkplates                
                PawnCheckPlate(xBoard, yBoard - 1);

                break;

            case "white_pawn":
                //if the pawn is not in its starting position it spawns its normal Checkplates
                PawnCheckPlate(xBoard, yBoard + 1);

                break;

        }
    }

    public void LineCheckPlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        //adjusts the coordinates
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        //if the square is empty a Check plate is spawned
        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            //CheckPlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        //checks if there is an enemy piece on the square
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(x, y);

            //check if it is the king
            if (cp.name == "white_king")
            {
                //set InCheck to true if it is the king
                GlobalVariables.WhiteInCheck = true;
            }

            if(cp.name == "black_king")
            {
                GlobalVariables.BlackInCheck = true;
            }
        }
    }

    //defines the knights Checkment pattern
    public void LCheckPlate()
    {
        PointCheckPlate(xBoard + 1, yBoard + 2);
        PointCheckPlate(xBoard - 1, yBoard + 2);
        PointCheckPlate(xBoard + 2, yBoard + 1);
        PointCheckPlate(xBoard + 2, yBoard - 1);
        PointCheckPlate(xBoard + 1, yBoard - 2);
        PointCheckPlate(xBoard - 1, yBoard - 2);
        PointCheckPlate(xBoard - 2, yBoard + 1);
        PointCheckPlate(xBoard - 2, yBoard - 1);
    }

    //defines the kings Checkment pattern
    public void SurroundCheckPlate()
    {
        PointCheckPlate(xBoard, yBoard + 1);
        PointCheckPlate(xBoard, yBoard - 1);
        PointCheckPlate(xBoard - 1, yBoard - 1);
        PointCheckPlate(xBoard - 1, yBoard);
        PointCheckPlate(xBoard - 1, yBoard + 1);
        PointCheckPlate(xBoard + 1, yBoard - 1);
        PointCheckPlate(xBoard + 1, yBoard);
        PointCheckPlate(xBoard + 1, yBoard + 1);
    }

    public void PointCheckPlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp != null)
            {
                if (cp.GetComponent<Chessman>().player != player)
                {
                    //check if it is the king
                    if (cp.name == "white_king")
                    {
                        //set InCheck to true if it is the king
                        GlobalVariables.WhiteInCheck = true;
                    }

                    if (cp.name == "black_king")
                    {
                        GlobalVariables.BlackInCheck = true;
                    }
                }
            }
        }
    }

    public void PawnCheckPlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            //chacks for pieces and spawns in a red Check plate if there are pieces
            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null
                && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                GameObject cp = controller.GetComponent<Game>().GetPosition(x + 1, y);

                //check if it is the king
                if (cp.name == "white_king")
                {
                    //set InCheck to true if it is the king
                    GlobalVariables.WhiteInCheck = true;
                }

                if (cp.name == "black_king")
                {
                    GlobalVariables.BlackInCheck = true;
                }
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                GameObject cp = controller.GetComponent<Game>().GetPosition(x - 1, y);

                //check if it is the king
                if (cp.name == "white_king")
                {
                    //set InCheck to true if it is the king
                    GlobalVariables.WhiteInCheck = true;
                }

                if (cp.name == "black_king")
                {
                    GlobalVariables.BlackInCheck = true;
                }
            }
        }
    }
    #endregion

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= XScaleFactor;
        y *= YScaleFactor;

        x += XOffset;
        y += YOffset;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {

        float x = matrixX;
        float y = matrixY;

        x *= XScaleFactor;
        y *= YScaleFactor;

        x += XOffset;
        y += YOffset;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}