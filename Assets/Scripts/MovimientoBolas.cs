using UnityEngine;

public class BolaTrampa : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float radioBola = 0.5f; 
    
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float currentRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
       
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void FixedUpdate()
    {
       
        rb.linearVelocity = moveDirection * speed;

        float distanciaRecorrida = speed * Time.fixedDeltaTime;
        float incrementoRotacion = (distanciaRecorrida / radioBola) * Mathf.Rad2Deg;

        
        if (moveDirection.x < 0)
        {
            currentRotation += incrementoRotacion; 
        }
        else if (moveDirection.x > 0)
        {
            currentRotation -= incrementoRotacion; 
        }

       
        rb.SetRotation(currentRotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            MovementoXogador jugador = collision.gameObject.GetComponent<MovementoXogador>();
            if (jugador != null)
            {
                
                jugador.PerderVida(); 
            }
        }

        // Rebote automático contra muros, suelo o cualquier obstáculo sólido
        Vector2 normal = collision.contacts[0].normal;
        moveDirection = Vector2.Reflect(moveDirection, normal).normalized;
    }
}