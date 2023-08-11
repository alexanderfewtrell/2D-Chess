using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    //References
    public GameObject controller;
    public GameObject movePlate;

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
        controller = GameObject.FindGameObjectWithTag("GameController");

        //take te instantiated location and adjust the transform
        SetCoords();

        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;

            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = black_queen; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }
}
