using UnityEngine;


public class MoveIn1DirectionAgent : IAgent {
    
    PacmanMovement.Direction direction = PacmanMovement.Direction.Idle;
    
    public MoveIn1DirectionAgent(PacmanMovement.Direction dir )
    {

        direction = dir;


    }

    public PacmanMovement.Direction getDirection(Transform pacman, PacmanMovement pacmanMovement)
    {
        return direction;
    }
}
