using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class MiniMaxAgent : IAgent
{
    int agents = 0;
    int depth = 0;
    bool alphaBeta = false;
    MultiAgentTree multiAgentTree;
    List<MultiAgentTree.AgentType> players;

    public MiniMaxAgent(int agents, int depth, bool alphabeta)
    {
        this.agents = agents;
        this.depth = depth;
        this.alphaBeta = alphabeta;

        multiAgentTree = new MultiAgentTree();

        players = new List<MultiAgentTree.AgentType>();

    }

    public void setAgentNumber( int number)
    {
        this.agents = number;

        //pacman is max
        players.Add(MultiAgentTree.AgentType.MAXIMIZER);

        //the rest are minimizers
        for (int i = 1; i < agents; i++)
        {
            players.Add(MultiAgentTree.AgentType.MINIMIZER);
        }

    }

    public override IAgent copy()
    {
        return new MiniMaxAgent(this.agents, this.depth, this.alphaBeta);
    }

    public override PacmanMovement.Direction getDirection(Level level, Place place)
    {

        PacmanMovement.Direction dir = PacmanMovement.Direction.Idle;

        dir = multiAgentTree.solve(players,
            new Assets.Agents.Search.PacmanSearchState(level),
            this.depth,
            this.alphaBeta);

        return dir;

    }
}

