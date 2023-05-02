using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().FireCount -= 3;
            return;
        }

        if (collision.CompareTag("Monster"))
        {
            collision.GetComponent<Monster>().MonsterDead();
        }
    }
}
