using UnityEngine;

public class BolaTrampa : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float radioBola = 0.5f; // Ajusta este valor si tu sprite es más grande o pequeño
    
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float currentRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Asignar una dirección inicial aleatoria en diagonal
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void FixedUpdate()
    {
        // 1. Mover la bola con la velocidad física
        rb.linearVelocity = moveDirection * speed;

        // 2. Calcular la rotación en función del desplazamiento (efecto rodar con un solo sprite)
        float distanciaRecorrida = speed * Time.fixedDeltaTime;
        float incrementoRotacion = (distanciaRecorrida / radioBola) * Mathf.Rad2Deg;

        // Si se mueve hacia la izquierda gira en un sentido, si va a la derecha en el otro
        if (moveDirection.x < 0)
        {
            currentRotation += incrementoRotacion; // Rota hacia la izquierda
        }
        else if (moveDirection.x > 0)
        {
            currentRotation -= incrementoRotacion; // Rota hacia la derecha
        }

        // Aplicar la rotación física al transform de la bola
        rb.SetRotation(currentRotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca contra el jugador, le quitamos vida
        if (collision.gameObject.CompareTag("Player"))
        {
            MovementoXogador jugador = collision.gameObject.GetComponent<MovementoXogador>();
            if (jugador != null)
            {
                // Asegúrate de que el método en tu script del jugador se llame así (o ajústalo a tu función de daño)
                jugador.PerderVida(); 
            }
        }

        // Rebote automático contra muros, suelo o cualquier obstáculo sólido
        Vector2 normal = collision.contacts[0].normal;
        moveDirection = Vector2.Reflect(moveDirection, normal).normalized;
    }
}