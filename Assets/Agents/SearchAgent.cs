using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Agents.Search;


class SearchAgent : IAgent
{

    public enum SearchAlgorithm
    {
        DFS,
        BFS,
        UCS,
        ASTAR
    }

    private SearchAlgorithm algorithm = SearchAlgorithm.BFS;

    public SearchAgent( SearchAlgorithm algorithm)
    {

        this.algorithm = algorithm;

    }

    bool problemSolved = false;
    List<PacmanMovement.Direction> plan = new List<PacmanMovement.Direction>();
    public PacmanMovement.Direction getDirection(Transform pacman, PacmanMovement pacmanMovement)
    {
        if(!problemSolved)
        {

            PacmanSearchState startState = new PacmanSearchState(pacmanMovement.Level);

            PacmanSearchProblem problem = new PacmanSearchProblem(startState);

            List<object> actions = getPlan(problem);

            if (actions != null)
            {

                foreach (object o in actions)
                {
                    PacmanMovement.Direction dir = (PacmanMovement.Direction)o;
                    plan.Add(dir);
                }

            }

            problemSolved = true;

        }

        if( plan != null && plan.Count > 0)
        {
            //dequeue
            PacmanMovement.Direction direction = plan[0]; //primer elemento
            plan.RemoveAt(0);

            return direction;

        }

        return PacmanMovement.Direction.Idle;

    }

    public List<object> getPlan( SearchProblem problem )
    {

        switch( algorithm)
        {
            case SearchAlgorithm.ASTAR:
                return problem.solveAStar();

            case SearchAlgorithm.BFS:
                return problem.solveBFS();

            case SearchAlgorithm.DFS:
                return problem.solveDFS();

            case SearchAlgorithm.UCS:
                return problem.solveUCS();
                
        }

        return null;

    }

}
