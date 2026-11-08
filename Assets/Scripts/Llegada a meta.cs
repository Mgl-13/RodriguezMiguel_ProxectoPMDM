using UnityEngine;
using UnityEngine.SceneManagement;

public class Llegadaameta : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡El jugador ha llegado a la meta! Pasando a la siguiente escena...");

            int escenaActual = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(escenaActual + 1);

        }
    }
}
