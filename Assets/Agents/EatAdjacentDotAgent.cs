using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EatAdjacentDotAgent : IAgent
{

    List<PacmanMovement.Direction> directions = new List<PacmanMovement.Direction>();
    List<PacmanMovement.Direction> plan = new List<PacmanMovement.Direction>();

    public EatAdjacentDotAgent()
    {
        directions.Add(PacmanMovement.Direction.Down); //0
        directions.Add(PacmanMovement.Direction.Right); //1
        directions.Add(PacmanMovement.Direction.Up); //2
        directions.Add(PacmanMovement.Direction.Left); //3

    }

    public PacmanMovement.Direction getDirection(
        Transform pacman, PacmanMovement pacmanMovement)
    {

        if (plan.Count == 0)
        {

            Place original = pacmanMovement.CurrentPlace;

            Place current = pacmanMovement.CurrentPlace;
            
            foreach (PacmanMovement.Direction direction in directions)
            {
                Place newPlace;
                int counter = 0;
                do
                {
                    newPlace = current.getPlaceByMovement(direction);

                    current = newPlace;

                    counter++;

                } while (newPlace.Valid && !newPlace.HasFood);

                if (current.Valid && current.HasFood)
                {
                    //N veces a la derecha
                    for( int i = 0; i < counter; i++)
                    {
                        plan.Add(direction);

                    }

                    //romper el for
                    break;
                }
                else
                {
                    //reset
                    current = original;
                }

            }

        }else
        {

            //dequeue
            PacmanMovement.Direction direction = plan[0]; //primer elemento
            plan.RemoveAt(0);

            return direction;

        }

        return PacmanMovement.Direction.Idle;
    }
}
