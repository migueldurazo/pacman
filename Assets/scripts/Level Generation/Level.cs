using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level  {

    public Level()
    {

        board = new List<List<Place>>();

    }

    List<List<Place>> board;

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




}
