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
    private bool hasPowerUp = false;

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

    public bool HasPowerUp
    {
        get
        {
            return hasPowerUp;
        }

        set
        {
            hasPowerUp = value;
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

    public Place clone(Level level)
    {

        Place newPlace = new Place();
      
        newPlace.HasFood = this.HasFood;
        newPlace.PacmanPosition = this.PacmanPosition;
        newPlace.Valid = this.Valid;
        newPlace.X = this.X;
        newPlace.Y = this.Y;
        newPlace.level = level;
        return newPlace;

    }

    public bool overlaps ( Place other, int squareSize)
    {
        int rows = squareSize / 2;
        int cols = squareSize / 2;

        for( int i = 0; i < rows; i++)
        {
            for( int j = 0; j < cols; j++)
            {
                for (int k = 0; k < rows; k++)
                {
                    for (int l = 0; l < cols; l++)
                    {
                        int thisX = this.X + i;
                        int thisY = this.Y + j;
                        //TODO compare with all squares from other
                        int otherX = other.X + k;
                        int otherY = other.Y + l;

                        if( thisX == otherX && thisY == otherY )
                        {
                            return true;
                        }

                    }
                }

            }
        }

        return false;

    }

    public override bool Equals(object other)
    {
        
        if( this.Valid == null || other == null)
        {
            int x = 2;
        }

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

    public double distance(Place other)
    {
        return Mathf.Abs(this.X - other.X) + Mathf.Abs(this.Y - other.Y);
    }


}
