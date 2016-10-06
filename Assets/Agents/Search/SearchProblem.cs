using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;

namespace Assets.Agents.Search
{
    abstract class SearchProblem : MonoBehaviour
    {
        PriorityQueue<SearchState> fringe = new PriorityQueue<SearchState>();
        HashSet<SearchState> closed = new HashSet<SearchState>();

        public abstract SearchState getStartState();
        public abstract bool isGoalState( SearchState state );
        public abstract List<Action> getSuccessors( SearchState state );


        public List<object> solveDFS()
        {
            int level = 1;
            int expandedNodes = 0;
            SearchState start = getStartState();
            start.Priority = level;
            start.UseHigherPriority = true;
            fringe.Enqueue(start);

            while (fringe.Count() > 0)
            {
                SearchState state = fringe.Dequeue();
                if( isGoalState(state))
                {
                    return state.Plan;
                }

                int hash = state.GetHashCode();

                if( !closed.Contains( state ))
                {
                    closed.Add(state);
                    List<Action> successors = getSuccessors(state);
                    expandedNodes++;
                    foreach( Action successorAction in successors)
                    {   
                        SearchState successorState = successorAction.Successor;
                        successorState.Plan = state.Plan;
                        successorState.Plan.Add(successorAction.getAction());
                        successorState.Priority = state.Priority + 1;
                        fringe.Enqueue(successorState);
                    }
                    if( expandedNodes%1000 == 0)
                    {
                        Debug.Log("Expanded " + expandedNodes+" Nodes");

                        if( expandedNodes== 20000)
                        {
                            Debug.Log("Too many nodes");
                            return state.Plan;
                        }

                    }

                }

            }

            return null;

        }

        public List<object> solveBFS()
        {

            //Implement Breadth-first search

            return null;

        }
    }
}
