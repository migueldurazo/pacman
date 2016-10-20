using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Agents.Search
{
    class PacmanSearchProblem : SearchProblem
    {

        private PacmanSearchState startSearchState;
        public PacmanSearchProblem(PacmanSearchState start )
        {
            this.startSearchState = start;

        }

        public override SearchState getStartState()
        {
            return startSearchState;
        }

        public override List<Action> getSuccessors(SearchState state)
        {
            List<Action> successors = new List<Action>();

            SearchState pacmanSearchState = (PacmanSearchState)state;

            int lastStateFood = ((PacmanSearchState)pacmanSearchState).getLevel().FoodCount;

            foreach (PacmanMovement.Direction direction in Enum.GetValues(typeof(PacmanMovement.Direction)))
            {
                if (direction == PacmanMovement.Direction.Idle) continue;

                PacmanAction action = new PacmanAction();
                action.Cost = 1;
                action.Direction = direction;

                PacmanSearchState successorState = (PacmanSearchState)pacmanSearchState.getSuccessorState(action);

                action.Successor = successorState;

                int currentStateFood = successorState==null?lastStateFood:successorState.getLevel().FoodCount;

                if( lastStateFood == currentStateFood)  //no comio
                {

                    action.Cost = 2;

                }

                if (action.Successor != null)
                {

                    successors.Add(action);

                }
            }

            return successors;

        }

        public override bool isGoalState(SearchState state)
        {

            PacmanSearchState pacmanState = (PacmanSearchState)state;
            if( pacmanState.getLevel().FoodCount == 0)
            {
                return true;
            }

            return false;

        }
    }
}
