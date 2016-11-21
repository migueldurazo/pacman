using Assets.Agents.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class EvaulationAgent : IAgent
{
    public override IAgent copy()
    {
        return new EvaulationAgent();
    }

    public override PacmanMovement.Direction getDirection(Level level, Place place)
    {

        double initialEvaluation = level.getEvaluation();
        double maxScore = 0;
        PacmanMovement.Direction dir = PacmanMovement.Direction.Idle;
        List<Evaluation> validEvaluations = new List<Evaluation>();
        foreach (PacmanMovement.Direction direction in Enum.GetValues(typeof(PacmanMovement.Direction)))
        {

            if (direction == PacmanMovement.Direction.Idle) continue;

            Level tempLevel = level.clone();
            
            if (this.gameObject.name.StartsWith("Pacman"))
            {
                Place newPlace = tempLevel.updatePacmanPosition(direction);
                Place tempPlace = level.getPlace(newPlace.X, newPlace.Y);

                if (newPlace == null || !newPlace.Valid || newPlace.Equals( place )) continue;


                double newEval = tempLevel.getBetterEvaluation( level );
                validEvaluations.Add(new Evaluation( newEval, direction ));
            }

        }

        validEvaluations = validEvaluations.OrderByDescending(v => v.evaluation).ToList<Evaluation>();

        if( validEvaluations.Count > 0)
        {

            maxScore = validEvaluations[0].evaluation;

            validEvaluations.RemoveAll(item => item.evaluation < maxScore);

            dir = validEvaluations[new System.Random().Next(0, validEvaluations.Count)].direction;
            
        }

        

        return dir;

    }



}

