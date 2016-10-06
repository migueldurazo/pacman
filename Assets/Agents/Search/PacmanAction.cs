using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Agents.Search 
{
    class PacmanAction : Action
    {

        private PacmanMovement.Direction direction;

        public PacmanMovement.Direction Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }

        }

        public override object getAction()
        {
            return Direction;
        }
    }
}
