using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class ExpectiMaxAgent : IAgent
{
    int agents = 0;
    int depth = 0;
    MultiAgentTree multiAgentTree;
    List<MultiAgentTree.AgentType> players;


    public ExpectiMaxAgent(int agents, int depth)
    {
        this.agents = agents;
        this.depth = depth;
        multiAgentTree = new MultiAgentTree();

        players = new List<MultiAgentTree.AgentType>();

    }

    public void setAgentNumber(int number)
    {
        this.agents = number;

        //pacman is max
        players.Add(MultiAgentTree.AgentType.MAXIMIZER);

        //the rest are minimizers
        for (int i = 1; i < agents; i++)
        {
            players.Add(MultiAgentTree.AgentType.CHANCE);
        }

    }

    public override IAgent copy()
    {
        return new ExpectiMaxAgent(this.agents, this.depth);
    }

    public override PacmanMovement.Direction getDirection(Level level, Place place)
    {

        PacmanMovement.Direction dir = PacmanMovement.Direction.Idle;

        dir = multiAgentTree.solve(players,
            new Assets.Agents.Search.PacmanSearchState(level),
            this.depth,
            false);

        return dir;

    }
}

