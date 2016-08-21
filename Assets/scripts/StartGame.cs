using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    
    // Use this for initialization
    public void generateLevel()
    {

        // LevelGenerator.Maze maze = getMaze();

        IAgent agent = getAgent();

        LevelGenerator lg = Camera.main.GetComponent<LevelGenerator>();

        lg.levelData = getLevelData();

        lg.enabled = true;

        lg.agent = agent;

        GameObject.Find("MainMenu").GetComponent<Canvas>().gameObject.SetActive(false); 

    }

    string getLevelData()
    {

        string levelName = GameObject.Find("MazeDropdown").GetComponent<Dropdown>().captionText.text;

        TextAsset mytxtData = (TextAsset)Resources.Load("levels/" + levelName);

        return mytxtData.text;



    }

    LevelGenerator.Maze getMaze()
    {
        LevelGenerator.Maze maze = LevelGenerator.Maze.Test;

        int selectedMaze = GameObject.Find("MazeDropdown").GetComponent<Dropdown>().value;

        switch (selectedMaze)
        {
            case 0:
                maze = LevelGenerator.Maze.Sample1;
                break;
            case 1:
                maze = LevelGenerator.Maze.Test;
                break;

        }

        return maze;


    }

    IAgent getAgent()
    {

        int selectedAgent = GameObject.Find("AgentDropdown").GetComponent<Dropdown>().value;

        int options = 0;

        if( GameObject.Find("OptionsDropdown"))
        {

            options = GameObject.Find("OptionsDropdown").GetComponent<Dropdown>().value;

        }

        switch (selectedAgent)
        {
            case 0:

                return new HumanAgent();

                
            case 1:

                MoveIn1DirectionAgent agent = new MoveIn1DirectionAgent(getDirection(options));
                
                return agent;
                
                
        }

        return new HumanAgent();

    }

    PacmanMovement.Direction getDirection(int options)
    {
        
        switch (options)
        {
            case 0:
                return PacmanMovement.Direction.Right;                
            case 1:
                return PacmanMovement.Direction.Left;
            case 2:
                return PacmanMovement.Direction.Down;
            case 3:
                return PacmanMovement.Direction.Up;

        }

        return PacmanMovement.Direction.Left;
    }



}
