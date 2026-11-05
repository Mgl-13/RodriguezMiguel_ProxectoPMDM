using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MovementoXogador : MonoBehaviour
{
    [SerializeField] float velocidade = 5f;
    [SerializeField] float velocidadSalto = 5f;

    [SerializeField] int maxSaltos = 2;

    [SerializeField] int vidasActuales = 0;
    private int saltosRestantes;
    
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
    }

    void Update()
    {
        // Movimiento horizontal
        Vector2 movimiento = accionMoverse.ReadValue<Vector2>();
        rb.linearVelocityX = movimiento.x * velocidade;


        //Dirección movimientpo
        if (movimiento.x > 0)
        {
            spriteRenderer.flipX = false; 
        }
        else if (movimiento.x < 0)
        {
            spriteRenderer.flipX = true;
        }


        //Accion salto
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

        anim.SetFloat("Speed", movimiento.x < 0 ? -movimiento.x : movimiento.x);
        anim.SetBool("isGrounded", saltosRestantes == maxSaltos);
        anim.SetFloat("VerticalVelocity", rb.linearVelocityY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.CompareTag("Suelo") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            saltosRestantes = maxSaltos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Trampa"))
        {
            if(vidasActuales > 0){ 
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
                vidasActuales--;
            } else if(vidasActuales == 0){

                Debug.Log("Has perdido");
                //SceneManager.GetSceneByName(string Inicio);            
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);           
        }

        if (collision.CompareTag("Vidas"))
        {
             vidasActuales++;
             Destroy(collision.gameObject);          
        }
    }
}