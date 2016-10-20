using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Agents.Search
{
    abstract class SearchState : IComparable<SearchState>
    {

        private List<object> plan = new List<object>();
        private bool useHigherPriority = true;

        public bool UseHigherPriority
        {
            get
            {
                return useHigherPriority;
            }
            set
            {
                useHigherPriority = value;
            }
        }

        public List<object> Plan
        {
            get { return plan; }
            set
            {
                plan = new List<object>( value ); //make a copy of it 
            }

        }

        private double priority = 0.0;
        public double Priority
        {
            get
            {
                return priority;
            }

            set
            {
                priority = value;
            }
        }

        private bool heuristicCalculated;

        private double heuristic = 0.0;
        public double Heuristic
        {
            get {
                if (!heuristicCalculated)
                    prepareHeuristic();
                return heuristic;
            }
            set { heuristic = value;  }
        }

        protected abstract bool calculateHeuristic();

        private void prepareHeuristic()
        {

            if (calculateHeuristic())
            {
                heuristicCalculated = true;
            }
            
        }
        
        public abstract SearchState getSuccessorState(Action action);

        public int CompareTo(SearchState other)
        {
            if (this.Priority < other.Priority) return useHigherPriority ? 1 : -1;
            else if (this.Priority > other.Priority) return useHigherPriority ? -1 : 1;
            else return 0;
        }

    }
}
