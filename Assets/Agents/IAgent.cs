using UnityEngine;
using System.Collections;


public interface IAgent  {

    PacmanMovement.Direction getDirection(Transform pacman, PacmanMovement pacmanMovement );

    //The idea here is to call this method on Pacman's update, and do what it needs to do to move pacman, might need delays

}
