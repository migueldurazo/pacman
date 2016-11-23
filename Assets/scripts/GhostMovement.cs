using UnityEngine;
using System.Collections;

public class GhostMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    Level level;
    int ghostIndex = 0;
    Vector3 origin = Vector3.zero;
    Place originPlace = null;
    MultiAgentMovement movement;

    public void setLevel(Level level)
    {
        this.level = level;
    }

    public void setGhostIndex( int index)
    {
        this.ghostIndex = index;
    } 

    public void setOrigin( Vector3 position)
    {
        origin = position;
    } 
    public void setOriginPlace(Place place, Level level)
    {
        this.originPlace = place.clone(level);
    }

    public void setMovement( MultiAgentMovement movement)
    {
        this.movement = movement;

    } 

    void OnTriggerEnter2D(Collider2D co)
    {
        /*
        if (co.name.StartsWith("Pacman"))
        {

            level.ghostPacmanCollision(ghostIndex);

            if (  level.GhostScaredTimes[ghostIndex] >0 )
            {
                level.GhostScaredTimes[ghostIndex] = 0;
                GetComponent<Animator>().SetBool("scared", false);
                transform.position = origin;
                level.GhostPositions[ghostIndex] = level.getPlace( originPlace.X, originPlace.Y);
                movement.destinations[ghostIndex + 1] = origin;

            }

            

        }*/
    }

}
