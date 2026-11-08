using UnityEngine;

public class ColisionColeccionables : MonoBehaviour
{
    //[SerializeField] int vidasAportadas = 1;    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MovementoXogador jugador = collision.GetComponent<MovementoXogador>();
            if (jugador != null)
            {
                //jugador.SumarVida(vidasAportadas);
                Debug.Log("¡Vida sumada!");
            }

            Animator anim = GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("isRecogida");
            }

            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }

            Destroy(gameObject, 0.5f);
        }
    }
}