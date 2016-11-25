using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

    public GameObject audio;

    private MultiAgentMovement multiAgent;

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.name.StartsWith("Pacman"))
        {
            PlaySound();
            multiAgent.scareGhosts(level.scareTime);
            Destroy(gameObject);
            
        }
    }

    Level level;

    public void setLevel(Level level)
    {
        this.level = level;
    }

    public void setMultiAgentMovement( MultiAgentMovement movement)
    {
        this.multiAgent = movement;
    } 

    void Update()
    {

        if( multiAgent.Level.getTotalScaredTime() == 0)
        {
            if (audio.activeSelf)
            {
                audio.SetActive(false);
            }
        }

    }

    private void PlaySound()
    {

        audio.SetActive(true);
        
    }

}
