using UnityEngine;
using UnityEngine.SceneManagement; 

public class Cambioescena : MonoBehaviour
{
    // Método para cargar una escena por su nombre exacto
    public void CambiarAEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // Método para pasar directamente al siguiente nivel
    public void SiguienteNivel()
    {
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual + 1);
    }

    // Método para salir del juego
    public void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}