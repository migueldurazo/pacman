using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultiAgentMovement : MonoBehaviour {

    List<IAgent> agents;
    public List<Vector2> destinations;
    Level level;

    public float speed = 0.4f;

    public MultiAgentMovement(  )
    {
        
    }

    public Level Level
    {
        get { return level; }
        set { level = value; }
    }

    public void scareGhosts(int time)
    {
        for (int i = 0; i < agents.Count; i++)
        {
            IAgent agent = agents[i];

            if (!agent.gameObject.name.StartsWith("Pacman"))
            {

                agent.scaredTime = time;
                level.GhostScaredTimes[i-1] = time;
                agent.gameObject.GetComponent<Animator>().SetBool("scared", true);
                
            }

        }
    }

    public void setAgents(List<IAgent> agents)
    {
        this.agents = agents;

    }

    public long maxTime = 30000L;

	// Use this for initialization
	void Start () {

        destinations = new List<Vector2>();

        if ( agents!= null && agents.Count > 0)
        {
            foreach(IAgent a in agents)
            {
                this.destinations.Add(a.gameObject.transform.position);
            }
        }

	}

    private bool gameOver = false;
	
	// Update is called once per frame
	void Update () {

        if (gameOver) return;

        if (agents == null || agents.Count == 0) return;

        updatePositions(agents, destinations);

        if (level.GameOver)
        {

            if (level.GameOver)
            {

                if (level.Win)
                {
                    GameObject.Find("Texto").GetComponent<Text>().text = "Winner!";
                    //GetComponent<AudioSource>().Play();
                }
                else if( level.Timeup ) 
                {
                    GameObject.Find("Texto").GetComponent<Text>().text = "Time is up";
                }
                else
                {
                    GameObject.Find("Texto").GetComponent<Text>().text = "Game Over";
                }

            }

            gameOver = true;

        }

        if (gameOver) return;

        if (noMovement(agents, destinations))
        {
            //get new destinations
            for (int i = 0; i < agents.Count; i++)
            {
                IAgent agent = agents[i];
                PacmanMovement.Direction dir = agent.getDirection(level, agent.agentPlace);
                
                if (dir == PacmanMovement.Direction.Idle) return;

                //Place p = agent.agentPlace.getPlaceByMovement(dir);
                //Vector2 directionVector = p.EntityPosition - agent.agentPlace.EntityPosition;
                //   if (valid(dir, directionVector, agent.gameObject.transform)) {

                if (agent.gameObject.name.StartsWith("Pacman"))
                {
                    //TODO
                    //agent.agentPlace = level.updatePacmanPosition( dir );
                    level.updatePacmanPosition(dir);
                    GameObject.Find("InGame").GetComponent<Text>().text =
                    ("Score: " + level.Score + "\n");
                }
                else
                {
                    //agent.agentPlace = level.updateGhostPosition(dir, i-1, agent.agentPlace);
                    level.updateGhostPosition(dir, i - 1, agent.agentPlace);
                    int scaredTimeLeft = level.GhostScaredTimes[i - 1];
                    if (scaredTimeLeft == 0)
                    {
                        agent.gameObject.GetComponent<Animator>().SetBool("scared", false);
                    }
                }


                for (int j = 0; j < agents.Count; j++)
                {

                    IAgent agentObject = agents[j];

                    if (agentObject.gameObject.name.StartsWith("Pacman"))
                    {
                        agentObject.agentPlace = level.PacmanPosition;
                    }
                    else
                    {
                        agentObject.agentPlace = level.GhostPositions[j - 1];
                    }

                }

                destinations[i] = agent.agentPlace.EntityPosition;



                //    }

            }

        }

	}

    private void updatePositions(List<IAgent> agents, List<Vector2> destinations)
    {

        for (int i = 0; i < agents.Count; i++)
        {
            IAgent agent = agents[i];
            Vector2 p = Vector2.MoveTowards(agent.gameObject.transform.position, 
                destinations[i], speed);
            agent.gameObject.GetComponent<Rigidbody2D>().MovePosition(p);

            Vector2 direction = destinations[i] - (Vector2)agent.gameObject.transform.position;
            agent.gameObject.GetComponent<Animator>().SetFloat("DirX", direction.x);
            agent.gameObject.GetComponent<Animator>().SetFloat("DirY", direction.y);

        }

        
    }

    private bool noMovement( List<IAgent> agents, List<Vector2> destinations )
    {
        for( int i = 0; i < agents.Count; i++)
        {
            if( Vector2.Distance(destinations[i], (Vector2)agents[i].gameObject.transform.position) >= 0.1f)
            {
                return false;
            }
        }

        return true;
        

    }

    Vector2 getDirectionVector(PacmanMovement.Direction direction)
    {

        switch (direction)
        {

            case PacmanMovement.Direction.Down:
                return Vector2.down;

            case PacmanMovement.Direction.Up:
                return Vector2.up;

            case PacmanMovement.Direction.Left:
                return Vector2.left;

            case PacmanMovement.Direction.Right:
                return Vector2.right;

            case PacmanMovement.Direction.Idle:
                return Vector2.zero; //transform.position;
        }

        return Vector2.zero;//transform.position;

    }

    bool valid(PacmanMovement.Direction direction, Vector2 dir, Transform entity)
    {
        float halfSize = entity.GetComponent<Renderer>().bounds.size.x / 2.0f;
        halfSize -= 0.2f; //remove a small part so it wont wrongly hit a collider

        if (direction == PacmanMovement.Direction.Idle) return false;

        List<Vector2[]> differentPacmanPositions = new List<Vector2[]>();
        //Add original position and where it is going
        Vector2[] originalPair = { entity.position, dir };
        differentPacmanPositions.Add(originalPair);

        if (direction == PacmanMovement.Direction.Down || direction == PacmanMovement.Direction.Up)
        {

            //Add position for pacman prefab start bound
            Vector2[] pairxStart = { (Vector2)entity.position - new Vector2(halfSize, 0), dir };
            differentPacmanPositions.Add(pairxStart);
            //Add position for pacman prefab start bound
            Vector2[] pairxEnd = { (Vector2)entity.position + new Vector2(halfSize, 0), dir };
            differentPacmanPositions.Add(pairxEnd);

        }
        if (direction == PacmanMovement.Direction.Right || direction == PacmanMovement.Direction.Left)
        {

            //Add position for pacman prefab start bound
            Vector2[] pairyStart = { (Vector2)entity.position - new Vector2(0, halfSize), dir };
            differentPacmanPositions.Add(pairyStart);
            //Add position for pacman prefab start bound
            Vector2[] pairyEnd = { (Vector2)entity.position + new Vector2(0, halfSize), dir };
            differentPacmanPositions.Add(pairyEnd);

        }


        foreach (Vector2[] pair in differentPacmanPositions)
        {

            // Cast Line from 'next to Pac-Man' to 'Pac-Man'
            Vector2 pos = pair[0];
            dir = pair[1];

            RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);

            Debug.DrawLine(pos + dir, pos);
            Debug.DrawRay(pos + dir, pos);

            if (hit.collider == null)
            {
                continue;
            }

            bool a = (hit.collider == entity.gameObject.GetComponent<Collider2D>())
                || ((hit.collider != entity.gameObject.GetComponent<Collider2D>()) && hit.collider.isTrigger);

            if (!a)
            {
                return false;
            }


        }

        return true;

    }

}
