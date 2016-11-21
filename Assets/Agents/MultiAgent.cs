using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MultiAgent : IAgent
{

    public IAgent pacmanAgent;
    public IAgent ghostAgent;
    /*TODO
     * Implement more ghost agents
     */
    public MultiAgent( IAgent pacman, IAgent ghost)
    {
        this.pacmanAgent = pacman;
        this.ghostAgent = ghost;
    }

    public override PacmanMovement.Direction getDirection(Level level, Place place)
    {
        throw new NotImplementedException();
    }

    public override IAgent copy()
    {
        return new MultiAgent( this.pacmanAgent, this.ghostAgent );
    }

}

