using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody2D rb2D;

    [Header("Properties")]
    [SerializeField] int fireCount;
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;

    [Header("Read Only")]
    [SerializeField] bool canJump;
    [SerializeField] bool moveRight;
    [SerializeField] bool moveLeft;
    [SerializeField] bool jump;

    public int FireCount
    {
        get { return fireCount; }
        set 
        { 
            fireCount = Mathf.Clamp(value, 0, 3);

            if(fireCount == 0)
            {
                GameManager.singleton.PlayerDead();
            }
        }
    }

    #region Life Cycle
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump)
            {
                jump = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (fireCount >= 2)
            {
                FireCount--;
                GameManager.singleton.ChangeControlToFireGhost();
            }
        }
    }

    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(0, rb2D.velocity.y);

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

        if (jump)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
            canJump = false;
            jump = false;
        }
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
}
