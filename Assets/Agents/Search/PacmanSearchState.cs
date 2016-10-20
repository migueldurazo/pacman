using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Agents.Search
{
    class PacmanSearchState : SearchState
    {
        private Level levelAtSomePoint;

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


        public bool updatePacmanPosition(PacmanMovement.Direction direction)
        {

            return this.levelAtSomePoint.updatePacmanPosition(direction);

        }

        public override SearchState getSuccessorState(Action action)
        {
            PacmanAction pacmanAction = (PacmanAction)action;
            PacmanMovement.Direction dir = (PacmanMovement.Direction)pacmanAction.getAction();

            PacmanSearchState newState = new PacmanSearchState(this.levelAtSomePoint);

            newState.UseHigherPriority = this.UseHigherPriority;

            if( newState.updatePacmanPosition(dir))
            {
                return newState;
            }

            return null;

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
        
    }
}
