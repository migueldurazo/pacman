using UnityEngine;
using System.Collections;
using System;

public class ReactionAgent : IAgent
{

    public override PacmanMovement.Direction getDirection(Level level, Place place)
    {

        Place originalPlace = place;

        foreach (PacmanMovement.Direction direction in Enum.GetValues(typeof(PacmanMovement.Direction)))
        {

            if (direction == PacmanMovement.Direction.Idle) continue;

            Place newPlace = originalPlace.getPlaceByMovement(direction);
            Place lastPlace = originalPlace;

            while ( newPlace!=null &&  newPlace != lastPlace && !newPlace.HasFood && newPlace.Valid) 
            {

                lastPlace = newPlace;
                newPlace = newPlace.getPlaceByMovement(direction);

            }

            if (newPlace != null && newPlace.HasFood)
            {

                return direction;

            }

        }

        return PacmanMovement.Direction.Idle;


    }

    public override IAgent copy()
    {
        return new ReactionAgent();
    }


}
