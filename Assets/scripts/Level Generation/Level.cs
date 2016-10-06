using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level  {

    public Level()
    {

        board = new List<List<Place>>();

    }

    List<List<Place>> board;
    public Place PacmanPosition;
    private int foodCount;
    private int score = 0;
    private int FOOD_SCORE = 100;
    private int MOVE_PENALTY = 1;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }

    }

    public int FoodCount
    {
        get
        {
            return foodCount;
        }
        set
        {
            foodCount = value;
        }
       
    }

    public List<List<Place>> Board
    {
        get
        {
            return board;
        }

        set
        {
            board = value;
        }
    }

    public Place getPlace(int x, int y)
    {

        if( x >= Board.Count)
        {

            return null;

        }

        if( y >=  Board[x].Count)
        {

            return null;

        }

        return Board[x][y];

    }

    public Level clone() {

        Level newLevel = new Level();
        List<List<Place>> newBoard = newLevel.Board;
        foreach( List<Place> row in board)
        {
            List<Place> newRow = new List<Place>();
            
            foreach( Place p in row)
            {
                newRow.Add(p.clone());
            }

            newBoard.Add(newRow);

        }
        newLevel.Board = newBoard;

        newLevel.foodCount = this.foodCount;

        newLevel.PacmanPosition = this.PacmanPosition.clone();

        return newLevel;

    }

    public override bool Equals( object other )
    {

        if (this.FoodCount != ((Level)other).FoodCount) return false;

        List<List<Place>> thisBoard = this.board;
        List<List<Place>> otherBoard = ((Level)other).board;

        if (thisBoard.Count != otherBoard.Count)
        {
            return false;
        }

        for( int i = 0; i < thisBoard.Count; i++)
        {

            List<Place> thisRow = thisBoard[i];
            List<Place> otherRow = otherBoard[i];

            if (thisRow.Count != otherRow.Count) return false;

            for( int j = 0; j < thisRow.Count; j++)
            {

                Place thisPlace = thisRow[j];
                Place otherPlace = otherRow[j];

                if( !thisPlace.Equals(otherPlace))
                {
                    return false;
                }
                
            }

        }

        return true;

    }

    public override int GetHashCode()
    {

        int hash = 13;

        List<List<Place>> thisBoard = this.board;

        for (int i = 0; i < thisBoard.Count; i++)
        {

            List<Place> thisRow = thisBoard[i];

            for (int j = 0; j < thisRow.Count; j++)
            {
                Place thisPlace = thisRow[j];
                hash = (hash * 7) + thisPlace.GetHashCode();

                if (thisPlace.HasFood) {
                    hash = (hash * (i + 5)) + thisPlace.X * 6;
                }
                if (thisPlace.HasFood) {
                    hash = (hash * (j + 3)) + thisPlace.Y * 13;
                }
            }


        }

        hash = (hash * 7) + PacmanPosition.GetHashCode();

        hash = (hash * 7) + FoodCount;

        return hash;


    }

    public override string ToString()
    {
        return "{Food left: " + foodCount+"}";
    }

    public bool updatePacmanPosition( PacmanMovement.Direction dir) 
    {

        Place newPlace =    this.PacmanPosition.getPlaceByMovement(dir);

        newPlace = getPlace(newPlace.X, newPlace.Y);

        if (newPlace != null && newPlace.Valid)
        {
            this.PacmanPosition = newPlace;
            if (newPlace.HasFood)
            {
                foodCount--;
                score += FOOD_SCORE;
            }else
            {
                
                score -= MOVE_PENALTY;
                
            }
            newPlace.HasFood = false;
            
            
            return true;
        }

        return false;

    }



}
