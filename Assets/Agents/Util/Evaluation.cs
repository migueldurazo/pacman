using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Agents.Util
{
    class Evaluation
    {
        public Evaluation(double e, PacmanMovement.Direction d)
        {
            evaluation = e;
            direction = d;
        }
        public double evaluation;
        public PacmanMovement.Direction direction;

        public override String ToString()
        {
            return "[" + direction + ": " + evaluation + "]";
        }

    }
}
