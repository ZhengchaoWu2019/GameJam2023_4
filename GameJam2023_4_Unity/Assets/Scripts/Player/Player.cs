using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] Animator footAnimator;
    [SerializeField] PlayerAni playerAni;

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
            Animator animator = playerAni.GetComponent<Animator>();

            if (fireCount == 3)
            {
                if (value - fireCount < 0)
                {
                    animator.SetBool("L2M", true);
                }
            }

            if(fireCount == 2)
            {
                if(value - fireCount > 0)
                {
                    animator.SetBool("L2M", false);
                }
                else
                {
                    animator.SetBool("M2S", true);
                }
            }

            if(fireCount == 1)
            {
                if(value - fireCount > 0)
                {
                    animator.SetBool("M2S", false);
                }
            }

            fireCount = Mathf.Clamp(value, 0, 3);

            if(fireCount == 0)
            {
                animator.SetTrigger("toDie");
            }

            
        }
    }

    public void StopControl()
    {
        this.enabled = false;
        rb2D.velocity = Vector2.zero;
        playerAni.GetComponent<Animator>().SetBool("toWalk", false);
        footAnimator.SetBool("toWalk", false);
    }

    #region Life Cycle
    private void Awake()
    {
        playerAni.OnDieAniEnd += AfterPlayerDead;
    }
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

        if(canJump && rb2D.velocity.x != 0)
        {
            footAnimator.SetBool("toWalk", true);
        }
        else
        {
            footAnimator.SetBool("toWalk", false);
        }

        if(rb2D.velocity.x > 0)
        {
            playerAni.GetComponent<SpriteRenderer>().flipX = false;
            footAnimator.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(rb2D.velocity.x < 0)
        {
            playerAni.GetComponent<SpriteRenderer>().flipX = true;
            footAnimator.GetComponent<SpriteRenderer>().flipX = true;
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

    void AfterPlayerDead()
    {
        GameManager.singleton.PlayerDead();
    }
}
