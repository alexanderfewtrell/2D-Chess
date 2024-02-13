using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveDetails
{
    public GameObject Piece { get; set; }
    public float MovePlateXCoord { get; set; }
    public float MovePlateYCoord { get; set; }

    public bool Attack { get; set; }

    public AIMoveDetails(GameObject piece, float movePlateXCoord, float movePlateYCoord, bool attack)
    {
        Piece = piece;
        MovePlateXCoord = movePlateXCoord;
        MovePlateYCoord = movePlateYCoord;
        Attack = attack;
    }
}
