using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PacmanMovement : MonoBehaviour {

    public IAgent agent = new HumanAgent();
    private Level level;

    private string ScoreData = "";
    public enum Direction { Right, Up, Left, Down, Idle };

    public float speed = 0.4f;
	Vector2 dest = Vector2.zero;

    public bool greenLight = true;

    public IAgent Agent
    {
        get
        {
            return agent;
        }

        set
        {
            agent = value;
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

    // Use this for initialization
    void Start () {

		dest = transform.position;
	
	}

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.name.StartsWith("Pacman"))
        {
            //GetComponent<AudioSource>().Play();
           // GameObject.Find("Texto").GetComponent<Text>().text = "Game Over";
            // Application.LoadLevel("main");
        }

    }
    void FixedUpdate() {

        if(Agent == null)
        {
            return;
        }

        // Move closer to Destination
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        if (Vector2.Distance(dest, (Vector2)transform.position) <= 0.1f)
        {
            
            PacmanMovement.Direction direction = Agent.getDirection( level, agent.agentPlace );

            //new destination, up, down, top or bottom?
            Vector2 directionVector = getDirectionVector(direction);

            if( valid( direction, directionVector, transform ))
            {

                dest = (Vector2)transform.position + directionVector;

                if (transform.tag != "ghost") {

                    level.updatePacmanPosition(direction);

                    Debug.Log("Score: " + level.Score);

                    GameObject.Find("InGame").GetComponent<Text>().text =
                        ("Score: " + level.Score + "\n");

                    // Usar las ghost positions y pacman position para el game over

                }

            }

        }

		// Animation Parameters
		Vector2 dir = dest - (Vector2)transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);
		
    }

    Vector2 getDirectionVector( PacmanMovement.Direction direction)
    {

        switch( direction)
        {

            case Direction.Down:
                return Vector2.down;
                
            case Direction.Up:
                return Vector2.up;
                
            case Direction.Left:
                return Vector2.left;

            case Direction.Right:
                return Vector2.right;

            case Direction.Idle:
                return Vector2.zero; //transform.position;
        }

        return Vector2.zero;//transform.position;

    } 

    bool valid( Direction direction, Vector2 dir, Transform entity)
    {
        float halfSize = entity.GetComponent<Renderer>().bounds.size.x / 2.0f;
        halfSize -= 0.2f; //remove a small part so it wont wrongly hit a collider

        if (direction == Direction.Idle) return false;

        List<Vector2[]> differentPacmanPositions = new List<Vector2[]>();
        //Add original position and where it is going
        Vector2[] originalPair = { entity.position, dir };
        differentPacmanPositions.Add(originalPair);

        if( direction == Direction.Down || direction == Direction.Up)
        {

            //Add position for pacman prefab start bound
            Vector2[] pairxStart = { (Vector2)entity.position - new Vector2(halfSize, 0), dir };
            differentPacmanPositions.Add(pairxStart);
            //Add position for pacman prefab start bound
            Vector2[] pairxEnd = { (Vector2)entity.position + new Vector2(halfSize, 0), dir };
            differentPacmanPositions.Add(pairxEnd);

        }
        if (direction == Direction.Right || direction == Direction.Left)
        {

            //Add position for pacman prefab start bound
            Vector2[] pairyStart = { (Vector2)entity.position - new Vector2(0, halfSize), dir };
            differentPacmanPositions.Add(pairyStart);
            //Add position for pacman prefab start bound
            Vector2[] pairyEnd = { (Vector2)entity.position + new Vector2(0, halfSize), dir };
            differentPacmanPositions.Add(pairyEnd);

        }


        foreach( Vector2[] pair in differentPacmanPositions)
        {

            // Cast Line from 'next to Pac-Man' to 'Pac-Man'
            Vector2 pos = pair[0];
            dir = pair[1];

            RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);

            Debug.DrawLine(pos + dir , pos);
            Debug.DrawRay(pos + dir, pos);

            if( hit.collider == null)
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
