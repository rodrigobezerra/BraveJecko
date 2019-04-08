using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float forcaPulo;
    public float velocidadeMaxima;

    public int vidas;
    public int rings;

    public Text TextLives;
    public Text TextRings;

    public bool isGrounded;
    public bool inTheWater;
    public bool canFly;

    public GameObject LastCheckpoint;


    // Start is called before the first frame update
    void Start()
    {
        this.forcaPulo = 230;
        this.velocidadeMaxima = 2;

        TextLives.text = vidas.ToString();
        TextRings.text = rings.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        float movimento = Input.GetAxis("Horizontal");
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>(); // Física do Player
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Animator animator = GetComponent<Animator>();

        // x: movimento, y: velocidade baseada na atual (manter velocidade contante no eixo y)
        rigidbody.velocity = new Vector2(movimento * velocidadeMaxima, rigidbody.velocity.y);

        // Movimentação para ambos os lados
        if (movimento < 0) // direção contrária
        {
            spriteRenderer.flipX = true; // inverte o player
        }
        else if (movimento > 0)
        {
            spriteRenderer.flipX = false; // 
        }

        // Andar
        if ((movimento > 0 || movimento < 0) && !animator.GetBool("jumping"))
        {
            animator.SetBool("walking", true);
            //Debug.Log("Andando!");
        }
        else
        {
            animator.SetBool("walking", false);
        }

        if (!inTheWater) // Fora da água
        {
            animator.SetBool("swimming", false);

            // Pular
            if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space)))
            {

                //Debug.Log("Pulando!");
                if (isGrounded) // Player só consegue iniciar o pulo se estiver no châo (plataforma)
                {
                    rigidbody.AddForce(new Vector2(0, forcaPulo)); // Pular
                    GetComponent<AudioSource>().Play(); // som do pulo

                    canFly = false;
                }
                else
                {
                    canFly = true;
                }
            }

            // Voar
            if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Space)) && canFly)
            {
                animator.SetBool("flying", true);
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, -0.5f);
            }

            // No chão
            if (isGrounded)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("flying", false);
            }
            else
            {
                animator.SetBool("jumping", true);
            }

        } else
        {
            // Fisica do personagem na água
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rigidbody.AddForce(new Vector2(0, 6F * Time.deltaTime),ForceMode2D.Impulse);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                rigidbody.AddForce(new Vector2(0, -6F * Time.deltaTime), ForceMode2D.Impulse);
            }

            rigidbody.AddForce(new Vector2(0, 10F * Time.deltaTime), ForceMode2D.Impulse);
        }


        animator.SetBool("swimming", inTheWater);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            GetComponent<Animator>().SetTrigger("hammering");
            Debug.Log("Martelar");

            Collider2D[] colliders = new Collider2D[3];
            transform.Find("HammerArea").gameObject.GetComponent<Collider2D>() // collider relacionado a HammerArea
                .OverlapCollider(new ContactFilter2D(), colliders); // OverlapCollider: detecta colisão de todos os objetos que colidirem com a HammerArea
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                {
                    if (colliders[i].gameObject.CompareTag("Monstros"))
                    {
                        Destroy(colliders[i].gameObject);
                    }
                }
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Plataformas"))
        {
            isGrounded = true;
        }

        if (collision2D.gameObject.CompareTag("Trampolins"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 11f); // Força exercida no Transpolim
        }

        if (collision2D.gameObject.CompareTag("Monstros"))
        {
            vidas--;
            if (vidas == 0) // morrer e iniciar novamente a partir do ultimo checkpoint atingido
            {
                transform.position = LastCheckpoint.transform.position;
                vidas = 3; // nova chance
            }
            TextLives.text = vidas.ToString();
        }

        // Debug.Log("Colidiu com: " + collision2D.gameObject.tag);
    }

    private void OnCollisionExit2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Plataformas"))
        {
            isGrounded = false;
        }

        // Debug.Log("Parou de colidir com: " + collision2D.gameObject.tag);
    }

    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Liquidos"))
        {
            inTheWater = true;
        }

        if (collision2D.gameObject.CompareTag("Rings"))
        {
            Destroy(collision2D.gameObject);
            //GetComponents<AudioSource>()[1].Play();
            rings++;
            TextRings.text = rings.ToString();
            Debug.Log("Som da Moeda 3");
        }

        if (collision2D.gameObject.CompareTag("Checkpoints"))
        {
            Debug.Log("Colidiu com checkpoint " + collision2D.gameObject.name);
            LastCheckpoint = collision2D.gameObject; // Ultimo checkpoint registrado
        }

    }



    private void OnTriggerExit2D(Collider2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Liquidos"))
        {
            inTheWater = false;
        }
    }

}
