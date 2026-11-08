using UnityEngine;

public class ColisionEnemigos : MonoBehaviour
{
    private MovementoXogador movementoXogador;

    void Start()
    {
        movementoXogador = GetComponent<MovementoXogador>();
    }

    private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trampa"))
        {
            if (movementoXogador != null)
            {
                movementoXogador.PerderVida();
            }
        }
    }
}