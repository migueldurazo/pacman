using UnityEngine;
using System.Collections;

public class Place
{

    public Place(bool valid)
    {

        this.valid = valid;

    }

    private Vector3 pacmanPosition;
    private bool valid;
    private int x;
    private int y;
    private Level level;

    public Vector3 PacmanPosition
    {
        get
        {
            return pacmanPosition;
        }

        set
        {
            pacmanPosition = value;
        }
    }

    public bool Valid
    {
        get
        {
            return valid;
        }

        set
        {
            valid = value;
        }
    }

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
        }
    }

    public Level Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public Place getPlaceByMovement( PacmanMovement.Direction direction)
    {

        if (Level != null)
        {

            switch (direction)
            {

                case PacmanMovement.Direction.Down:
                    return level.getPlace(X+1, Y);

                case PacmanMovement.Direction.Up:
                    return level.getPlace(X-1, Y );

                case PacmanMovement.Direction.Left:
                    return level.getPlace(X , Y-1);

                case PacmanMovement.Direction.Right:
                    return level.getPlace(X , Y+1);

                case PacmanMovement.Direction.Idle:
                    return this;

            }

        }

        return this; ;

    }

        


}
