using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D fisica;
    public SpriteRenderer sprite;
    public Animator anim;

    //Movimiento
    public int velocidad;
    public int fuerzaSalto;
    float entradaX = 0f;

    //Stats
    public int estrellas;
    public int vidas;
    public bool vulnerable;
    public bool muerto;

    // Star configuration
    [Tooltip("Number of stars required to advance")]
    public int starsRequired = 3;
    [Tooltip("Scene to load when enough stars are collected")]
    public string targetSceneName = "Level2";

    //HUD
    public Canvas canvas;

    //Sonido
    public AudioSource sonidoSalto;
    public AudioSource sonidoMuerteEnemigo;
    public AudioSource sonidoDolor;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        fisica = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        //Movimiento
        velocidad = 8;
        fuerzaSalto = 8;

        //Vidas
        vidas = 3;
        estrellas = 0;
        vulnerable = true;
        muerto = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Obtención de input de mover hacia los lados
        entradaX = Input.GetAxis("Horizontal");

        //Control de animación
        anim.SetFloat("velocidadX", Mathf.Abs(fisica.velocity.x));
        anim.SetFloat("velocidadY", fisica.velocity.y);

        //Mecánica de salto
        Salto();
        //Giro de dibujo del personaje
        Flip();

        //Teclas de nivel
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Level1");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Level2");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Level3");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void FixedUpdate()
    {
        //Movimiento
        fisica.velocity = new Vector2(entradaX * velocidad, fisica.velocity.y);
    }

    public void Salto()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (TocandoSuelo())
            {
                sonidoSalto.Play();
                fisica.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            }
            else
            {
                if ((Mathf.Abs(fisica.velocity.y) < 0.5f))
                {
                    sonidoSalto.Play();
                    fisica.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
                }
            }
        }
    }

    public void Flip()
    {
        if (fisica.velocity.x > 0f)
        {
            sprite.flipX = false;
        }
        else if (fisica.velocity.x < 0f)
        {
            sprite.flipX = true;
        }
    }

    private bool TocandoSuelo()
    {
        RaycastHit2D toca = Physics2D.Raycast(
            transform.position + new Vector3(0, -2f, 0), Vector2.down, 0.2f);
        return toca.collider != null;
    }

    public void FinJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void setVidas()
    {
        canvas.GetComponent<HUDController>().CambioVida(vidas);
    }

    public void SetEstrellas()
    {
        canvas.GetComponent<HUDController>().SetEstrellas(estrellas);
        CheckStarsAndLoadScene();
    }

    public void CheckStarsAndLoadScene()
    {
        if (estrellas >= starsRequired && !string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}