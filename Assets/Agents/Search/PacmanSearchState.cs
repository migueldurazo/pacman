using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Agents.Search
{
    public class PacmanSearchState : SearchState
    {

        private Level levelAtSomePoint;
        private List<Action> successors = null;
        private double value = 0.0;
        

        public double Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        public PacmanSearchState( Level level )
        {
            levelAtSomePoint = level.clone();
            
        }
        protected override bool calculateHeuristic()
        {

            Place pacmanPos = levelAtSomePoint.PacmanPosition;

            int pacX = pacmanPos.X;
            int pacY = pacmanPos.Y;

            int x = 0;
            int y = 0;

            Place place = null;
            double totalDistance = 0.0;

            do
            {

                place = levelAtSomePoint.getPlace(x, y);

                if (place == null)
                {
                    y++;
                    x = 0;
                    place = levelAtSomePoint.getPlace(x, y);
                }
                else
                {

                    x++;

                }

                if( place != null && place.HasFood)
                {
                    totalDistance += (double)(Math.Abs(x - pacX) + Math.Abs(y - pacY));
                }

            } while (place != null );

            Heuristic = totalDistance;
            
            return true;
            
        }

        public override bool Equals(object comparingState)
        {
            PacmanSearchState pacmanState = (PacmanSearchState)comparingState;

            Level thisLevel = this.levelAtSomePoint;
            Level otherLevel = pacmanState.levelAtSomePoint;

            Place thisPacmanPosition = this.levelAtSomePoint.PacmanPosition;
            Place otherPacmanPosition = pacmanState.levelAtSomePoint.PacmanPosition;

            if( thisLevel.Equals( otherLevel ) && thisPacmanPosition.X == otherPacmanPosition.X
                && thisPacmanPosition.Y == otherPacmanPosition.Y  )
            {

                return true;

            }

            return false;

        }

        public override int GetHashCode()
        {

            int hash = 13;

            hash = (hash * 7) + this.levelAtSomePoint.GetHashCode();
            hash = (hash * 7) + this.levelAtSomePoint.PacmanPosition.GetHashCode();

            return hash;

        }


        public bool updatePacmanPosition(PacmanMovement.Direction direction, Place place)
        {

            Place clonedPlace = this.levelAtSomePoint.PacmanPosition.clone(this.levelAtSomePoint);

            Place newPlace = this.levelAtSomePoint.updatePacmanPosition(direction);

            return  newPlace!=null && !newPlace.Equals(clonedPlace);

        }

        public bool updateGhostPosition(PacmanMovement.Direction direction, Place place, int index)
        {

            Place clonedPlace = this.levelAtSomePoint.GhostPositions[index].clone(this.levelAtSomePoint);

            Place newPlace = this.levelAtSomePoint.updateGhostPosition(direction, index, place);

            return newPlace != null && !newPlace.Equals(clonedPlace);

        }

        public SearchState getSuccessorState(Action action, int index)
        {
            PacmanAction pacmanAction = (PacmanAction)action;
            PacmanMovement.Direction dir = (PacmanMovement.Direction)pacmanAction.getAction();

            PacmanSearchState newState = new PacmanSearchState(this.levelAtSomePoint);

            newState.UseHigherPriority = this.UseHigherPriority;

            if (index == 0)
            {

                if (newState.updatePacmanPosition(dir, levelAtSomePoint.PacmanPosition))
                {
                    return newState;
                }

            }else
            {
                if (newState.updateGhostPosition(dir, levelAtSomePoint.GhostPositions[index-1], index-1))
                {
                    return newState;
                }

            }

            return null;

        }

        public List<Action> getSuccessors( int index )
        {

            if (successors == null)
            {
                successors = new List<Action>();
            }else
            {
                return successors;
            }

            foreach (PacmanMovement.Direction direction in Enum.GetValues(typeof(PacmanMovement.Direction)))
            {
                if (direction == PacmanMovement.Direction.Idle) continue;

                PacmanAction action = new PacmanAction();
                action.Direction = direction;

                PacmanSearchState successorState =
                    (PacmanSearchState)this.getSuccessorState(action, index);
                action.Successor = successorState;
                if( successorState != null)
                {
                    successors.Add(action);
                }

            }

            return successors;

        }

        public override string ToString()
        {
            return "Priority: "+Priority+ ", Level: "+levelAtSomePoint.ToString()+
                ", Plan: "+planString()+", Pacman: "+levelAtSomePoint.PacmanPosition.ToString();
        }

        private String planString()
        {

            String planAsString = "";

            foreach( object obj in Plan)
            {

                planAsString+= obj.ToString()+",";

            }

            return planAsString;

            

        }

        public Level getLevel()
        {
            return levelAtSomePoint;
        }

        public double evaluate()
        {
            return levelAtSomePoint.getEvaluation();
        }

        public override SearchState getSuccessorState(Action action)
        {
            throw new NotImplementedException();
        }
    }
}
