using Assets.Agents.Search;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MultiAgentTree : MonoBehaviour
{
    public enum AgentType
    {
        MINIMIZER,MAXIMIZER,CHANCE
    };

    bool debug = true;

    //Dictionary<PacmanSearchState, double> stateValuesCache = new Dictionary<PacmanSearchState, double>();

    int nodes = 0;

    public PacmanMovement.Direction solve( List<AgentType> players, 
        PacmanSearchState root, int depth, bool alphabeta )
    {
        PacmanMovement.Direction dir = PacmanMovement.Direction.Idle;

        //Alpha beta hint: You need to initialize alpha and beta values and feed it to the max and min functions
        //each time you get a new value on a node, you should check if the alpha or beta has changed
        // e.g. if after you calculate the value for a state, you see that the value is better than your current
        // alpha or beta, replace the alpha or beta value with this one and use it for the rest of the state successors.
        if( alphabeta && !canPrune(players))
        {
            alphabeta = false;
        }

        calculateValue(root, 1, depth*players.Count, players, alphabeta);

        Debug.Log("Nodes explored: " + nodes);

        List<Action> succesorStates = new List<Action>();

        succesorStates.AddRange(root.getSuccessors( 0 ));
        
        succesorStates.RemoveAll(item => 
        ((PacmanSearchState)item.Successor).Value < root.Value);

        dir = (PacmanMovement.Direction)succesorStates[
            new System.Random().Next(0, succesorStates.Count)].getAction();

        return dir;

    }
    
    private bool canPrune( List<AgentType> players )
    {
        //check if all players are either max or min, return true if they are
        return true;
    }

    public double calculateValue(PacmanSearchState state, int level,
        int depth, List<AgentType> players, bool alphabeta)
    {
        int playerIndex = (level - 1) % players.Count;

        double stateValue = 0.0;

        if (level == depth)
        {
            stateValue = state.evaluate();
        }
        else
        {

            if (players[playerIndex] == AgentType.MAXIMIZER)
            {
                stateValue = max(state, level, depth, players, alphabeta);
            }
            if (players[playerIndex] == AgentType.MINIMIZER)
            {
                stateValue = min(state, level, depth, players, alphabeta);
            }

            if (players[playerIndex] == AgentType.CHANCE)
            {
                stateValue = chance(state, level, depth, players, alphabeta);
            }
        }

        state.Value = stateValue;

        return stateValue ;
        
    }

    public double max( PacmanSearchState state, int level, 
        int depth, List<AgentType> players, bool alphabeta )
    {
        int playerIndex = (level - 1) % players.Count;

        List<double> allValues = new List<double>();

        double value = System.Double.NegativeInfinity;

        foreach(Action s in state.getSuccessors(  playerIndex  ))
        {
            double candidate = calculateValue((PacmanSearchState)s.Successor, 
                level + 1, depth, players, alphabeta);

            allValues.Add(candidate);

            nodes++;

            if (candidate > value)
            {
                value = candidate;
            }



            if( alphabeta)
            {
                //si se poda
                // bre

                // Check the value, if it can be pruned according to Alpha-beta then return here
            }

        }

        /*
        Debug.Log("Max values on level "+level+": ");
        foreach ( double v in allValues)
        {
            Debug.Log(v + "\t");
        }
        Debug.Log("Chosen value: " +value);
        */

        return value;
    }

    public double min(PacmanSearchState state, int level,
        int depth, List<AgentType> players, bool alphabeta )
    {

        int playerIndex = (level - 1) % players.Count;

        List<double> allValues = new List<double>();

        double value = System.Double.PositiveInfinity;

        foreach (Action s in state.getSuccessors( playerIndex))
        {
            double candidate = calculateValue((PacmanSearchState)s.Successor, level + 1,
                depth, players, alphabeta );

            nodes++;

            allValues.Add(candidate);

            if (candidate < value)
            {
                value = candidate;
            }

            if (alphabeta)
            {
                // Check the value, if it can be pruned according to Alpha-beta then return here
            }

        }

        /*
        Debug.Log("Min values on level " + level + ": ");
        foreach (double v in allValues)
        {
            Debug.Log(v + "\t");
        }
        Debug.Log("Chosen value: " + value);
        */

        return value;
    }

    public double chance(PacmanSearchState state, int level,
        int depth, List<AgentType> players, bool alphabeta)
    {
        int playerIndex = (level - 1) % players.Count;

        List<double> allValues = new List<double>();

        double totalValue = 0.0;

        foreach (Action s in state.getSuccessors(playerIndex))
        {

            double candidate = calculateValue((PacmanSearchState)s.Successor,
                level + 1, depth, players, alphabeta);

            allValues.Add(candidate);

            nodes++;

            totalValue += candidate;

        }

        double value = totalValue / allValues.Count;

        /*
        Debug.Log("Chance values on level " + level + ": ");
        foreach (double v in allValues)
        {
            Debug.Log(v + "\t");
        }
        Debug.Log("Chosen value: " + value);
        */

        return players.Count == 0 ? 0.0 : value ;
    }



}

