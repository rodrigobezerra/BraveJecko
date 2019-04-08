using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoHorizontal : MonoBehaviour
{

    public bool colided;

    public float move;


    // Start is called before the first frame update
    void Start()
    {
        colided = false;
        move = -2; // direcionamento no eixo Y para a esquerda por padrão no inicio
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(move, GetComponent<Rigidbody2D>().velocity.y);

        if (colided)
        {
            Flip();
        }
    }

    private void Flip()
    {
        move *= -1; //inversão da direção
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        colided = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plataformas"))
        {
            colided = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plataformas"))
        {
            colided = false;
        }
    }
}
