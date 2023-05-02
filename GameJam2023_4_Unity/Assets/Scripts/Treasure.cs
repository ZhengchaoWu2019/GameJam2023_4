using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireGhost"))
        {
            Destroy(gameObject);

            GameManager.singleton.ChangeControlToPlayer();

            GameManager.singleton.GetCurrentInteractNPC().GetComponent<NPCInteractiveArea>().FinishTask();
        }
    }
}
