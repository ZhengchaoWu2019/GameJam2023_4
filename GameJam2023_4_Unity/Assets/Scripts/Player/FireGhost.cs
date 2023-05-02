using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FireGhost : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody2D rb2D;

    [Header("Properties")]
    [SerializeField] float speed;
    [SerializeField] float moveRadius;

    [Header("Read Only")]
    [SerializeField] bool moveRight;
    [SerializeField] bool moveLeft;
    [SerializeField] bool moveUp;
    [SerializeField] bool moveDown;
    [SerializeField] Vector3? currentCenter;

    private void Update()
    {
        currentCenter = GameManager.singleton.GetCurrentPlayer().transform.position;

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

        Vector2 deltaVec = transform.position - currentCenter.Value;
        if (deltaVec.sqrMagnitude >= moveRadius)
        {
            Vector2 dir = deltaVec.normalized;
            Vector2 dis = dir * moveRadius;

            if (moveRight)
            {
                float allowDistance = Mathf.Abs(Vector2.Dot(dis, Vector2.right));
                if(allowDistance == 0)
                {
                    if (deltaVec.magnitude <= moveRadius)
                    {
                        return;
                    }
                }
                if (deltaVec.x >= allowDistance)
                {
                    moveRight = false;
                }
            }

            if (moveLeft)
            {
                float allowDistance = Mathf.Abs(Vector2.Dot(dis, Vector2.right));
                if (allowDistance == 0)
                {
                    if (deltaVec.magnitude <= moveRadius)
                    {
                        return;
                    }
                }
                if (deltaVec.x <= -allowDistance)
                {
                    moveLeft = false;
                }
            }
            if (moveUp)
            {
                float allowDistance = Mathf.Abs(Vector2.Dot(dis, Vector2.up));
                if (allowDistance == 0)
                {
                    if (deltaVec.magnitude <= moveRadius)
                    {
                        return;
                    }
                }
                if (deltaVec.y >= allowDistance)
                {
                    moveUp = false;
                }
            }
            if (moveDown)
            {
                float allowDistance = Mathf.Abs(Vector2.Dot(dis, Vector2.up));
                if (allowDistance == 0)
                {
                    if (deltaVec.magnitude <= moveRadius)
                    {
                        return;
                    }
                }
                if (deltaVec.y <= -allowDistance)
                {
                    moveDown = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.singleton.GetCurrentPlayer().FireCount++;

            GameManager.singleton.ChangeControlToPlayer();
        }

        if (moveRight)
        {
            transform.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else if (moveLeft)
        {
            transform.GetComponentInChildren<SpriteRenderer>().flipX = true;
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

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Monster"))
        {
            GameManager.singleton.ChangeControlToPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            GameManager.singleton.ChangeControlToPlayer();
        }
    }
    private void OnDrawGizmos()
    {
        if (currentCenter != null)
        {
            Gizmos.DrawWireSphere(currentCenter.Value, moveRadius);

            Vector2 deltaVec = transform.position - currentCenter.Value;
            if (deltaVec.sqrMagnitude != 0)
            {
                Vector2 dir = deltaVec.normalized;
                Vector2 dis = dir * moveRadius;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(currentCenter.Value, (Vector2)currentCenter.Value + dis);
            }
        }
    }
}
