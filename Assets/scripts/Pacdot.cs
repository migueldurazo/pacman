using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pacdot : MonoBehaviour
{

    public AudioClip WAKA;
    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.name.StartsWith("Pacman"))
        {
            ReproducirSonido(WAKA);
            Destroy(gameObject);
        }
    }

    private void ReproducirSonido(AudioClip ClipOriginal)
    {
        AudioSource.PlayClipAtPoint(ClipOriginal, transform.position);
    }
}