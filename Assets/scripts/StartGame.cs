using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    void Update()
    {
        if (started)
        {

            if (Input.GetKeyDown("escape"))
            {
                started = false;
                SceneManager.LoadScene("main");
            }


            time -= Time.deltaTime;

            GameObject.Find("Canvas-time").GetComponent<Text>().text = "Time: " + time.ToString("f0");

            if (time <= 0.0f)
            {
                lg.generatedLevel.Timeup = true;
                lg.generatedLevel.GameOver = true;
                GameObject.Find("Canvas-time").GetComponent<Text>().text = "Time: 0";
            }

        }

    }

    public float time;
    private bool started = false;
    LevelGenerator lg;

    // Use this for initialization
    public void generateLevel()
    {

        // LevelGenerator.Maze maze = getMaze();

        IAgent agent = getAgent();

        lg = Camera.main.GetComponent<LevelGenerator>();

        lg.levelData = getLevelData();

        lg.enabled = true;

        lg.agent = agent;

        GameObject multiAgentGO = GameObject.Find("Multiagent");

        multiAgentGO.SetActive(false);

        lg.multiAgentGO = multiAgentGO;

        GameObject.Find("MainMenu").GetComponent<Canvas>().gameObject.SetActive(false);
        GameObject.Find("InGame").GetComponent<Text>().text = ("Score: " + 0 + "\n");

        time = 60;

        started = true;

    }

    string getLevelData()
    {

        string levelName = GameObject.Find("MazeDropdown").GetComponent<Dropdown>().captionText.text;

        TextAsset mytxtData = (TextAsset)Resources.Load("levels/" + levelName);

        return mytxtData.text;

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

            case 2:

                return new ReactionAgent();

            case 3:

               EatAdjacentDotAgent agenteNuevo = new EatAdjacentDotAgent();

               return agenteNuevo;

            case 4:

                SearchAgent.SearchAlgorithm algorithm = getSearchAlgorithm(options);

                return new SearchAgent(algorithm);

            case 5:

                return new MultiAgent(getPacmanAgent(options), new RandomAgent()) ;

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

    SearchAgent.SearchAlgorithm getSearchAlgorithm(int options)
    {

        switch (options)
        {
            case 0:
                return SearchAgent.SearchAlgorithm.DFS;
            case 1:
                return SearchAgent.SearchAlgorithm.BFS;
            case 2:
                return SearchAgent.SearchAlgorithm.UCS;
            case 3:
                return SearchAgent.SearchAlgorithm.ASTAR;

        }

        return SearchAgent.SearchAlgorithm.BFS;
    }

    IAgent getPacmanAgent(int options)
    {

        switch (options)
        {
            case 0:
                return new EvaulationAgent();
            case 1:
                return new MiniMaxAgent(0, 2, false);
            case 2:
                return new MiniMaxAgent(0, 2, true);
            case 3:
                return new MiniMaxAgent(0, 3, false);
            case 4:
                return new MiniMaxAgent(0, 3, true);
            case 5:
                return new MiniMaxAgent(0, 4, false);
            case 6:
                return new MiniMaxAgent(0, 4, true);
            case 7:
                return new ExpectiMaxAgent(0, 2);
            case 8:
                return new ExpectiMaxAgent(0, 3);
            case 9:
                return new ExpectiMaxAgent(0, 4);
            case 10:
                return new HumanAgent();

        }

        return new EvaulationAgent();
    }



}
