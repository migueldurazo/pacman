using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour {

    public enum Agent { Human, OneDirection };

    public Agent agentType = Agent.Human;

    public IAgent agent;

	public float speed = 0.4f;
	Vector2 dest = Vector2.zero;   

	// Use this for initialization
	void Start () {

		dest = transform.position;

/*
        switch (agentType)
        {

            case Agent.Human:
                agent = new HumanAgent();
                break;
            case Agent.OneDirection:
                agent = new MoveIn1DirectionAgent();
                break;

        }

        Debug.Log(dest);
        */
	
	}
	
	
	
	int counter = 1;
    public void setAgent(IAgent age)
    {

        this.agent = age;
    }

    void FixedUpdate() {

        if(agent == null)
        {
            return;
        }
		
        // Move closer to Destination
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        if (Vector2.Distance(dest, (Vector2)transform.position) <= 0.1f)
        {
            dest = agent.setDestination(transform, dest);

           // Debug.Log("destination: "+dest);

           // Debug.Log("current position: "+transform.position);
        }

        //	Debug.Log(dest+" - "+(Vector2)transform.position);

        //Debug.Log(Vector2.Distance(dest,(Vector2)transform.position)<=0.1);

        // Check for Input if not moving

        /*
        if (Vector2.Distance(dest,(Vector2)transform.position)<=0.1f) {
			
			if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up))
				dest = (Vector2)transform.position + Vector2.up;
			if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right*1.1f))
				dest = (Vector2)transform.position + Vector2.right;
			if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up))
				dest = (Vector2)transform.position - Vector2.up;
			if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right))
				dest = (Vector2)transform.position - Vector2.right;
		}
		*/


		// Animation Parameters
		Vector2 dir = dest - (Vector2)transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);
		
    }

	
}
