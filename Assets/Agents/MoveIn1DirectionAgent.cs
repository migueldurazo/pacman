using UnityEngine;


public class MoveIn1DirectionAgent : IAgent {
    
    PacmanMovement.Direction direction = PacmanMovement.Direction.Idle;
    
    public MoveIn1DirectionAgent(PacmanMovement.Direction dir )
    {

        direction = dir;


    }

    public override PacmanMovement.Direction getDirection(Level level, Place place)
    {
        return direction;
    }

    public override IAgent copy()
    {
        return new MoveIn1DirectionAgent(this.direction);
    }

}
