using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Agents.Util;
using System.Linq;

public abstract class IAgent
{

    public GameObject gameObject;
    public Place agentPlace;
    public int scaredTime = 0;

    public PacmanMovement.Direction getDirection( Level level, Place place, Place adversaryPlace)
    {
        if( scaredTime > 0)
        {
            scaredTime--;
            return getScaredDirection(level, place, adversaryPlace);
        }else
        {
            return getDirection(level, place);
        }
    }

    public abstract PacmanMovement.Direction getDirection(Level level, Place place);

    //The idea here is to call this method on Pacman's update, and do what it needs to do to move pacman, might need delays
    public abstract IAgent copy();

    public PacmanMovement.Direction getScaredDirection(Level level, Place place, Place dangerPlace)
    {

        PacmanMovement.Direction dir = PacmanMovement.Direction.Idle;

        List<Evaluation> validEvaluations = new List<Evaluation>();

        foreach (PacmanMovement.Direction direction in Enum.GetValues(typeof(PacmanMovement.Direction)))
        {

            if (direction == PacmanMovement.Direction.Idle) continue;


            Place newPlace = place.getPlaceByMovement(direction);

            if (newPlace == null || !newPlace.Valid || newPlace.Equals(place)) continue;

            double newEval = newPlace.distance(dangerPlace);

            validEvaluations.Add(new Evaluation(newEval, direction));


        }

        validEvaluations = validEvaluations.OrderBy(v => v.evaluation).ToList<Evaluation>();

        if (validEvaluations.Count > 0)
        {

            double minDistance = validEvaluations[0].evaluation;

            validEvaluations.RemoveAll(item => item.evaluation > minDistance);

            dir = validEvaluations[new System.Random().Next(0, validEvaluations.Count)].direction;

        }

        return dir;

    }

}
