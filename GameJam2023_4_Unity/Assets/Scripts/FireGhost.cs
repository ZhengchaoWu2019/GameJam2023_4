using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGhost : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody2D rb2D;

    [Header("Properties")]
    [SerializeField] float speed;

    [Header("Read Only")]
    [SerializeField] bool moveRight;
    [SerializeField] bool moveLeft;
    [SerializeField] bool moveUp;
    [SerializeField] bool moveDown;

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            moveRight = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveLeft = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveUp = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDown = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.singleton.GetCurrentPlayer().FireCount++;

            GameManager.singleton.ChangeControlToPlayer();
        }
    }

    private void FixedUpdate()
    {
        rb2D.velocity = Vector2.zero;

        if (moveRight)
        {
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
            moveRight = false;
        }

        if (moveLeft)
        {
            rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
            moveLeft = false;
        }

        if (moveUp)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, speed);
            moveUp = false;
        }

        if (moveDown)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, -speed);
            moveDown = false;
        }
    }
}
