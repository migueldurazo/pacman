using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RandomAgent : IAgent
{
    
    
    
    public RandomAgent()
    {
        
    }

    int[] probabilities = { 45, 5, 5, 45 };
    PacmanMovement.Direction[] directions = {PacmanMovement.Direction.Down,
        PacmanMovement.Direction.Left, PacmanMovement.Direction.Right,
        PacmanMovement.Direction.Up};
    List<PacmanMovement.Direction> memory = new List<PacmanMovement.Direction>();

    PacmanMovement.Direction lastDirection = PacmanMovement.Direction.Idle;

    public override PacmanMovement.Direction getDirection(Level level, Place entityPlace)
    {

        PacmanMovement.Direction dir = PacmanMovement.Direction.Idle;

        do {
            int value = new System.Random().Next(0,99);
            int max = 0;
            for(int i = 0; i < probabilities.Length; i++)
            {
                max += probabilities[i];
                if( value < max)
                {
                    dir = directions[i];
                    break;
                }
            }

        } while ( !entityPlace.getPlaceByMovement(dir).Valid   );

        bool changedDirection = true;

        if( lastDirection== dir )
        {
            changedDirection = false;
        }

        lastDirection = dir;

        setProbabilities(dir, changedDirection);

        return dir;
    }

    private void setProbabilities(PacmanMovement.Direction chosen, bool changedDirection)
    {

        for (int i = 0; i < directions.Length; i++)
        {

            PacmanMovement.Direction direction = directions[i];

            if( direction == chosen)
            {

                if (changedDirection)
                {

                    probabilities[i] = 85;

                }else
                {
                    if (probabilities[i] > 25) {
                        probabilities[i] -= 15;
                    }
                }

            }
            else
            {
                if (changedDirection)
                {
                    probabilities[i] = 5;
                }else
                {
                    if( probabilities[i] < 25)
                    {
                        probabilities[i] += 5;
                    }
                }

            }

        }

    }

    private void setMemory( PacmanMovement.Direction chosen)
    {
        memory.Add(chosen);
        if(memory.Count > 20)
        {
            memory.Remove(0);
        }
    }

    private void substractMemory(  )
    {

        for (int i = 0; i < directions.Length; i++)
        {

            PacmanMovement.Direction direction = directions[i];

            int directionCount = memory.Count(item => item == direction);

            probabilities[i] -= 2*directionCount;

        }

    }

    private void ensureProbabilities()
    {

        cutOverageProbabilities();

        int remaining = 100 - probabilities.Sum();

        if (remaining == 0) return;

        int value = remaining > 0 ? 1 : -1;

        while (remaining != 0)
        {

            for (int i = 0; i < probabilities.Length; i++)
            {

                if ((probabilities[i] + value) > 85|| (probabilities[i] + value) < 0)
                    continue;

                remaining -= value;

                probabilities[i] += value;

                value = remaining > 0 ? 1 : -1;

                if ( remaining == 0)
                {
                    break;
                }

            }

            value = remaining > 0 ? 1 : -1;

        }

    }

    private void cutOverageProbabilities()
    {
        for (int i = 0; i < probabilities.Length; i++)
        {

            if ( probabilities[i]<0)
            {
                probabilities[i] = 0;
            }
            if( probabilities[i]>85)
            {
                probabilities[i] = 85;
            }

        }
    }

    public override IAgent copy()
    {
        return new RandomAgent();
    }

}

