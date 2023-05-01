using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject taskFinishedBindGO;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireGhost"))
        {
            Destroy(gameObject);
            Destroy(taskFinishedBindGO);
            GameManager.singleton.ChangeControlToPlayer();
        }
    }
}
