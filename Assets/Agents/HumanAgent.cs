using UnityEngine;
using System.Collections;
using System;

public class HumanAgent : IAgent {

  
    public override PacmanMovement.Direction getDirection(Level level, Place place)
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

    public override IAgent copy()
    {
        return new HumanAgent();
    }


}
