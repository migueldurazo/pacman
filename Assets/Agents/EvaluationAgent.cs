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

    List<PacmanMovement.Direction> history = new List<PacmanMovement.Direction>();

    int unblocking = 0;

    PacmanMovement.Direction unblockDirection = PacmanMovement.Direction.Idle;

    public override PacmanMovement.Direction getDirection(Level level, Place place)
    {
        if( unblocking > 0)
        {
            unblocking--;
            return unblockDirection;
        }
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

        if( checkHistoryForBlockers())
        {
            dir = unblockDirection = history[history.Count - 1];
            unblocking = 2;
        }

        history.Add(dir);

        return dir;

    }

    private bool checkHistoryForBlockers()
    {
        if (history.Count < 2)
        {
            return false;
        }

        PacmanMovement.Direction[] repetition = new PacmanMovement.Direction[2];
        repetition[0] = history[history.Count - 1];
        repetition[1] = history[history.Count - 2];

        if( repetition[0] == repetition[1])
        {
            return false;
        }
        
        int index = history.Count - 3;
        for (int i = 0; i < 6 && index>=0; i++, index--)
        {
            if( repetition[i%2] != history[index])
            {
                return false;
            }
        }

        return true;


    }



}

