using UnityEngine;
using System.Collections;
using System;

public class HumanAgent : IAgent {

  
    public PacmanMovement.Direction getDirection(Transform pacman, PacmanMovement pacmanMovement)
    {

        if (Input.GetKey(KeyCode.UpArrow))
            return PacmanMovement.Direction.Up;
        if (Input.GetKey(KeyCode.RightArrow))
            return PacmanMovement.Direction.Right;
        if (Input.GetKey(KeyCode.DownArrow))
            return PacmanMovement.Direction.Down;
        if (Input.GetKey(KeyCode.LeftArrow))
            return PacmanMovement.Direction.Left;

        return PacmanMovement.Direction.Idle;
     
    }


}
