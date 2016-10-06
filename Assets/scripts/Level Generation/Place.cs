using UnityEngine;
using System.Collections;

public class Place
{

    private Vector3 pacmanPosition;
    private bool valid = false;
    private int x;
    private int y;
    private Level level;
    private bool hasFood = false;

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

    public bool HasFood
    {
        get
        {
            return hasFood;
        }

        set
        {
            hasFood = value;
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

        return null; 

    }

    public Place clone()
    {

        Place newPlace = new Place();
      
        newPlace.HasFood = this.HasFood;
        newPlace.PacmanPosition = this.PacmanPosition;
        newPlace.Valid = this.Valid;
        newPlace.X = this.X;
        newPlace.Y = this.Y;
        newPlace.level = this.level;

        return newPlace;

    }

    public override bool Equals(object other)
    {
        
        if (this.Valid != ((Place)other).Valid) return false;
        if (this.HasFood != ((Place)other).HasFood) return false;
        if (this.X != ((Place)other).X) return false;
        if (this.Y != ((Place)other).Y) return false;

        return true;

    }

    public override int GetHashCode()
    {
        int hash = 13;
        hash = (hash * 7) + (Valid ? 99 : 135);
        hash = (hash * 7) + (HasFood ? 12 : 883);
        hash = (hash * 7) + X * 3;
        hash = (hash * 7) + Y * 9;

        return hash;
    }

    public override string ToString()
    {
        return "{X=" + X + ",Y=" + Y + ",Valid=" + Valid + ",HasFood=" + hasFood + "}";
    }


}
