using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableArea : MonoBehaviour
{
    [Header("Read Only")]
    [SerializeField] bool isPlayerIn;
    [SerializeField] Player currentPlayer;

    public bool CheckIsPlayerIn()
    {
        return isPlayerIn;
    }

    public Player GetPlayer()
    {
        return currentPlayer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
            currentPlayer = collision.gameObject.GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
            currentPlayer = null;
        }
    }
}
