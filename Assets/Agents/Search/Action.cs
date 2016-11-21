using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Agents.Search
{
    public abstract class Action
    {
        private double cost;
        public double Cost
        {
            get
            {
                return cost;
            }

            set
            {
                cost = value;


            }
        }

        private SearchState successor;
        public SearchState Successor
        {
            get
            {
                return successor;
            }

            set
            {
                successor = value;


            }
        }

        public abstract object getAction();

    }
}