using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MovementoXogador : MonoBehaviour
{
    [SerializeField] float velocidade = 5f;
    [SerializeField] float velocidadSalto = 5f;

    [Header("Doble Salto")]
    [SerializeField] int maxSaltos = 2;
    private int saltosRestantes;

    [Header("Vidas e Invulnerabilidad")]
    [SerializeField] int vidasActuales = 3;
    [SerializeField] float tiempoInvulnerabilidad = 1.5f;
    private bool esInvulnerable = false;

    [Header("Daño por caída")]
    [SerializeField] float distanciaCaidaDanio = 5f; 
    private float alturaMaxima;
    private bool enSuelo = true;

    InputAction accionMoverse;
    InputAction accionSaltar;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        accionMoverse = InputSystem.actions.FindAction("Movimiento");
        accionSaltar = InputSystem.actions.FindAction("Saltar");

        saltosRestantes = maxSaltos;
        alturaMaxima = transform.position.y;
    }

    void Update()
    {
        Vector2 movimiento = accionMoverse.ReadValue<Vector2>();
        rb.linearVelocityX = movimiento.x * velocidade;

        if (movimiento.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movimiento.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (accionSaltar.triggered)
        {
            if (saltosRestantes > 0)
            {
                if (saltosRestantes == 1)
                {
                    anim.SetTrigger("DobleSalto");
                }

                rb.linearVelocityY = velocidadSalto;
                saltosRestantes--;
            }
        }

        // Dañlo caída
        if (enSuelo)
        {
            alturaMaxima = transform.position.y;
        }
        else if (transform.position.y > alturaMaxima)
        {
            alturaMaxima = transform.position.y;
        }

        anim.SetFloat("Speed", movimiento.x < 0 ? -movimiento.x : movimiento.x);
        anim.SetBool("isGrounded", saltosRestantes == maxSaltos);
        anim.SetFloat("VerticalVelocity", rb.linearVelocityY);
    }

    private bool EsSuelo(Collision2D collision)
    {
        return collision.gameObject.CompareTag("Suelo") ||
               collision.gameObject.layer == LayerMask.NameToLayer("Ground");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!EsSuelo(collision)) return;

        // Solo cuenta como aterrizaje si tocamos el suelo por arriba (no una pared)
        if (collision.GetContact(0).normal.y < 0.5f) return;

        saltosRestantes = maxSaltos;
        enSuelo = true;

        float distanciaCaida = alturaMaxima - transform.position.y;
        if (distanciaCaida >= distanciaCaidaDanio)
        {
            PerderVida();
        }

        alturaMaxima = transform.position.y;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (EsSuelo(collision))
        {
            enSuelo = false;
            alturaMaxima = transform.position.y;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trampa") && !esInvulnerable)
        {
            vidasActuales--;

            anim.SetBool("esGolpeado", true);

            if (vidasActuales <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                StartCoroutine(InvulnerabilidadRoutine());
            }  
        }

        if (collision.CompareTag("Vidas"))
        {
            
            collision.tag = "Untagged";
            collision.enabled = false;

            SumarVida();
            Destroy(collision.gameObject);
        }
    }

    public void PerderVida()
    {
        if (esInvulnerable) return;

        vidasActuales--;
        anim.SetTrigger("esGolpeado");

        if (vidasActuales <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            StartCoroutine(InvulnerabilidadRoutine());
        }
    }

    public void SumarVida(int cantidad = 1)
    {
        vidasActuales += cantidad; 
    }

    private IEnumerator InvulnerabilidadRoutine()
    {
        esInvulnerable = true;

        yield return new WaitForSeconds(tiempoInvulnerabilidad);

        anim.SetBool("esGolpeado", false);

        esInvulnerable = false;
    }
}