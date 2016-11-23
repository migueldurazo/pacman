using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Level  {

    public Level()
    {

        board = new List<List<Place>>();

    }

    List<List<Place>> board;
    public Place PacmanPosition;
    private List<Place> ghostPositions = new List<Place>();
    private List<Place> foodPositions = new List<Place>();
    private List<Place> powerupPositions = new List<Place>();
    private List<Place> ghostOriginPosition = new List<Place>();
    private List<int> ghostScaredTimes = new List<int>();
    private int foodCount = 0;
    private int powerUpCount = 0;
    private int score = 0;
    public int FOOD_SCORE = 10;
    public int POWERUP_SCORE = 20;
    public int MOVE_PENALTY = 1;
    public int WIN_SCORE = 500;
    public int LOSE_SCORE = 300;
    public int GHOST_EATEN = 200;
    public int scareTime = 50;
    private bool gameOver = false;
    private bool win = false;
    private bool lose = false;
    private bool timeup = false;

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

    public List<Place> GhostPositions
    {
        get
        {
            return ghostPositions;
        }

        set
        {
            ghostPositions = value;
        }
    }

    
    public List<Place> FoodPositions
    {
        get
        {
            return foodPositions;
        }

        set
        {
            foodPositions = value;
        }
    }

    public bool GameOver
    {
        get
        {
            return gameOver;
        }

        set
        {
            gameOver = value;
        }
    }

    public bool Win
    {
        get
        {
            return win;
        }

        set
        {
            win = value;
        }
    }

    public List<Place> PowerupPositions
    {
        get
        {
            return powerupPositions;
        }

        set
        {
            powerupPositions = value;
        }
    }

    public List<Place> PowerupPositions1
    {
        get
        {
            return powerupPositions;
        }

        set
        {
            powerupPositions = value;
        }
    }

    public List<int> GhostScaredTimes
    {
        get
        {
            return ghostScaredTimes;
        }

        set
        {
            ghostScaredTimes = value;
        }
    }

    public int PowerUpCount
    {
        get
        {
            return powerUpCount;
        }

        set
        {
            powerUpCount = value;
        }
    }

    public List<Place> GhostOriginPosition
    {
        get
        {
            return ghostOriginPosition;
        }

        set
        {
            ghostOriginPosition = value;
        }
    }

    public bool Timeup
    {
        get
        {
            return timeup;
        }

        set
        {
            timeup = value;
        }
    }

    public void setScareTimesForAll(  int time)
    {
        for( int i = 0; i < GhostScaredTimes.Count; i++)
        {
            GhostScaredTimes[i] = time;
        }
    }

    public Place getPlace(int x, int y)
    {

        if (x >= Board.Count || x < 0)
        {

            return null;

        }

        if( y >=  Board[x].Count || y < 0)
        {

            return null;

        }

        return Board[x][y];

    }



    public Level clone() {

        Level newLevel = new Level();
        foreach( List<Place> row in board)
        {
            List<Place> newRow = new List<Place>();
            
            foreach( Place p in row)
            {
                Place clonedPlace = p.clone(newLevel);

                if( ghostPositions.Contains(p))
                {
                    newLevel.ghostPositions.Add(clonedPlace);
                }
                if( foodPositions.Contains(p))
                {
                    newLevel.foodPositions.Add(clonedPlace);
                }
                if (powerupPositions.Contains(p))
                {
                    newLevel.foodPositions.Add(clonedPlace);
                }

                newRow.Add(clonedPlace);
            }

            newLevel.Board.Add(newRow);

        }

        newLevel.foodCount = this.foodCount;

        newLevel.PacmanPosition = newLevel.getPlace(this.PacmanPosition.X, this.PacmanPosition.Y);

        foreach( int scareTime in ghostScaredTimes)
        {
            newLevel.ghostScaredTimes.Add(scareTime);
        }

        newLevel.score = this.score;

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

    

    public Place updatePacmanPosition( PacmanMovement.Direction dir ) 
    {

        Place newPlace = PacmanPosition.getPlaceByMovement(dir);

        if (newPlace != null && newPlace.Valid)
        {

            this.PacmanPosition = newPlace;

            checkGameOver();

            if (newPlace.HasFood)
            {
                foodCount--;
                score += FOOD_SCORE;
                FoodPositions.Remove(newPlace);
            }

            if( newPlace.HasPowerUp)
            {

                powerUpCount--;
                score += POWERUP_SCORE;
                PowerupPositions.Remove(newPlace);
            }

            score -= MOVE_PENALTY;

            newPlace.HasFood = false;
            newPlace.HasPowerUp = false;
            if (foodCount == 0)
            {
                score += WIN_SCORE;
                this.win = true;
                this.gameOver = true;
            }

        }

        return this.PacmanPosition;

    }

    public Place updateGhostPosition(PacmanMovement.Direction dir, int index, Place place)
    {

        Place newPlace = place.getPlaceByMovement(dir);

        this.ghostPositions[index] = newPlace;

        substractGhostScaredTime(index);

        checkGameOver();

        return this.ghostPositions[index];

    }

    public void substractGhostScaredTime( int index)
    {
        if (this.ghostScaredTimes[index] == 0) return;
        
        this.ghostScaredTimes[index] = this.ghostScaredTimes[index] - 1;
    }

    private void checkGameOver()
    {
        for( int i = 0; i < ghostPositions.Count; i++ )
        {

            try
            {

                if (PacmanPosition.Equals(ghostPositions[i]))
                {

                    int ghostScaredTime = ghostScaredTimes[i];

                    if (ghostScaredTime == 0)
                    {
                        this.score -= LOSE_SCORE;
                        this.gameOver = true;
                        this.lose = true;
                    }
                    else
                    {
                        this.score += GHOST_EATEN;
                        ghostPositions[i] = ghostOriginPosition[i].clone(this);
                        ghostScaredTimes[i] = 0;
                    }

                }

            }catch( NullReferenceException nrf)
            {
                int x = 1;
            }
        }
    }

    public void ghostPacmanCollision(int ghostIndex)
    {

        int ghostScaredTime = ghostScaredTimes[ghostIndex];

        if (ghostScaredTime == 0)
        {
            //this.score -= LOSE_SCORE;
            //this.gameOver = true;
        }
        else
        {
            //this.score += GHOST_EATEN;
        }

    }


    public double getEvaluation()
    {

        
        return this.Score;
         
    }


    

    public double getBetterEvaluation( Level previousLevel )
    {

        double scoreUtility = this.score * SCORE_MULTIPLIER;

        double winUtility = this.win ? 1.0 * WIN_MULTIPLIER : 0.0;

        double loseUtility = this.lose ? 1.0 * LOSE_MULTIPLIER : 0.0;

        double foodDistanceUtility = getPacmanDistanceToClosestFood() * CLOSEST_FOOD_MULTIPLIER;

        double foodLeftUtility = this.foodCount * FOOD_LEFT_MULTIPLIER;

        double foodEatenUtility = this.foodCount < previousLevel.foodCount ? 1.0 * FOOD_EATEN_MULTIPLIER : 0.0;

        double ghostDistanceUtility = getClosestGhostDistanceFromPacman() * GHOST_DISTANCE_MULTIPLIER;

        double powerUpDistanceUtility = getClosestPowerUPDistanceFromPacman() * POWERUP_DISTANCE_MULTIPLIER;

        double scaredGhostDistanceUtility = getTotalScaredTime() > 0 ? getClosestScaredGhost() * SCARED_GHOST_DISTANCE_MULTIPLIER : 0.0;

        double totalUtility = scoreUtility + winUtility + loseUtility +
            foodDistanceUtility + foodLeftUtility + foodEatenUtility +
            ghostDistanceUtility + powerUpDistanceUtility + scaredGhostDistanceUtility;;

        return totalUtility;

    }
















    double SCORE_MULTIPLIER = 1.0;
    double WIN_MULTIPLIER = 100000.0;
    double LOSE_MULTIPLIER = -100000.0;
    double CLOSEST_FOOD_MULTIPLIER = -3.0;
    double FOOD_LEFT_MULTIPLIER = 0.0;
    double FOOD_EATEN_MULTIPLIER = 100.0;
    double GHOST_DISTANCE_MULTIPLIER = 0.3;
    double POWERUP_DISTANCE_MULTIPLIER = -5.0;
    double SCARED_GHOST_DISTANCE_MULTIPLIER = -2.0;




    public double getPacmanDistanceToClosestFood()
    {

        int pacX = PacmanPosition.X;
        int pacY = PacmanPosition.Y;
        double distance = 0.0;
        bool first = true;

        foreach( Place foodPlace in FoodPositions)
        {
            double candidate =  (double)(Math.Abs(foodPlace.X - pacX) + Math.Abs(foodPlace.Y - pacY));
            if( first || candidate < distance)
            {
                distance = candidate;
                first = false;
            }
            
        }

        return distance;

    }
   
    public double getClosestGhostDistanceFromPacman()
    {
        int pacX = PacmanPosition.X;
        int pacY = PacmanPosition.Y;
        double distance = 0.0;
        bool first = true;

        for (int i = 0; i < GhostPositions.Count; i++)
        {
            Place ghostPlace = ghostPositions[i];
            double scaredTime = ghostScaredTimes[i];

            if (scaredTime == 0)
            {

                double candidate = (double)(Math.Abs(ghostPlace.X - pacX) + Math.Abs(ghostPlace.Y - pacY));

                if (first || candidate < distance)
                {
                    distance = candidate;
                    first = false;
                }

            }
        }

        return distance;
    }

    public double getClosestPowerUPDistanceFromPacman()
    {
        int pacX = PacmanPosition.X;
        int pacY = PacmanPosition.Y;
        double distance = 0.0;
        bool first = true;

        foreach (Place powerUpPlace in PowerupPositions)
        {
            double candidate = (double)(Math.Abs(powerUpPlace.X - pacX) + Math.Abs(powerUpPlace.Y - pacY));

            if (first || candidate < distance)
            {
                distance = candidate;
                first = false;
            }
        }

        return distance;
    }

    public double getFoodTotalDistanceFromPacman(int radius)
    {

        int pacX = PacmanPosition.X;
        int pacY = PacmanPosition.Y;

        int x = radius>0?pacX-radius:0;
        int y = radius > 0 ? pacY - radius : 0;

        Place place = null;
        double totalDistance = 0.0;

        do
        {

            place = this.getPlace(x, y);

            if (place == null || ( radius>0 && x >= pacX+radius))
            {
                y++;
                x = 0;
                place = this.getPlace(x, y);
            }
            else
            {

                x++;

            }

            if (place != null && place.HasFood)
            {
                totalDistance += (double)(Math.Abs(x - pacX) + Math.Abs(y - pacY));
            }

        } while (place != null && ( radius == 0 || ( x < pacX+radius && y < pacY+radius )));

        return totalDistance;

    }

    public int getTotalScaredTime()
    {
        int total = 0;

        foreach( int scaredTime in ghostScaredTimes)
        {
            total += scaredTime;
        }

        return total;

    }

    public double getClosestScaredGhost()
    {
        int pacX = PacmanPosition.X;
        int pacY = PacmanPosition.Y;
        double distance = 0.0;
        bool first = true;

        for(int i = 0 ; i < GhostPositions.Count; i++)
        {
            Place ghostPlace = ghostPositions[i];
            double scaredTime = ghostScaredTimes[i];

            if (scaredTime > 0)
            {

                double candidate = (double)(Math.Abs(ghostPlace.X - pacX) + Math.Abs(ghostPlace.Y - pacY));

                if (first || candidate < distance)
                {
                    distance = candidate;
                    first = false;
                }

            }
        }

        return distance;
    }




}
