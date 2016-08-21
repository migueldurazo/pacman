using UnityEngine;
using System.Collections;

public class PacmanMovement : MonoBehaviour {

    private IAgent agent;
    private Level level;
    private Place currentPlace;

    public enum Direction { Right, Left, Up, Down, Idle };

    public float speed = 0.4f;
	Vector2 dest = Vector2.zero;

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

    public Place CurrentPlace
    {
        get
        {
            return currentPlace;
        }

        set
        {
            currentPlace = value;
        }
    }



    // Use this for initialization
    void Start () {

		dest = transform.position;
	
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
            PacmanMovement.Direction direction = Agent.getDirection(transform);

            if( direction != Direction.Idle)
            {

                int x = 2;

            }

            //new destination, up, down, top or bottom?
            Vector2 directionVector = getDirectionVector(direction);

            if( valid(directionVector, transform ))
            {

                dest = (Vector2)transform.position + directionVector;

                currentPlace = currentPlace.getPlaceByMovement(direction);

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

    bool valid(Vector2 dir, Transform pacman)
    {

        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 pos = pacman.position;

        if( pos == dir)
        {

     //       return false;

        }

        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);

        Debug.DrawLine(pos + dir * 1.1f, pos);
        Debug.DrawRay(pos + dir, pos);

        bool a = (hit.collider == pacman.gameObject.GetComponent<Collider2D>())
            || ((hit.collider != pacman.gameObject.GetComponent<Collider2D>()) && hit.collider.isTrigger);

        return a;
    }

}
