using UnityEngine;
using System.Collections;


public interface IAgent  {

    Vector2 setDestination(Transform pacman, Vector2 dest);

    //The idea here is to call this method on Pacman's update, and do what it needs to do to move pacman, might need delays

}
