using UnityEngine;
using System.Collections;
using System;

public class MoveIn1DirectionAgent : IAgent {

    public enum Direction { Right, Left, Up, Down};


    Vector2 vector = Vector2.zero;

    public MoveIn1DirectionAgent( Direction dir )
    {

        switch (dir)
        {

            case Direction.Down: vector = Vector2.down; break;
            case Direction.Left: vector = Vector2.left; break;
            case Direction.Right: vector = Vector2.right; break;
            case Direction.Up: vector = Vector2.up; break;


        }

    }


    public Vector2 setDestination(Transform pacman, Vector2 dest)
    {

        dest = (Vector2)pacman.position + vector;

        Debug.Log(dest);

        return dest;

    }
}
