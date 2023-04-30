using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public System.Action OnFireGhostCollided;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireGhost"))
        {
            OnFireGhostCollided?.Invoke();
        }
    }
}
