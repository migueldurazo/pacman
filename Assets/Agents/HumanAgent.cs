using UnityEngine;
using System.Collections;
using System;

public class HumanAgent : IAgent {


    public Vector2 setDestination(Transform pacman, Vector2 dest)
    {
     //   if (Vector2.Distance(dest, (Vector2)pacman.position) <= 0.1f)
       // {

            if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up, pacman))
                dest = (Vector2)pacman.position + Vector2.up;
            if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right , pacman))
                dest = (Vector2)pacman.position + Vector2.right;
            if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up, pacman))
                dest = (Vector2)pacman.position - Vector2.up;
            if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right, pacman))
                dest = (Vector2)pacman.position - Vector2.right;
        //}

        return dest;

    }

    bool valid(Vector2 dir, Transform pacman)
    {

        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 pos = pacman.position;

        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);

        Debug.DrawLine(pos + dir * 1.1f, pos);
        Debug.DrawRay(pos + dir, pos);
        
        bool a = (hit.collider == pacman.gameObject.GetComponent<Collider2D>()) 
            || ((hit.collider != pacman.gameObject.GetComponent<Collider2D>()) && hit.collider.isTrigger  );

        return a;
    }


}
