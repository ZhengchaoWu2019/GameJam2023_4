using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchArea : MonoBehaviour
{
    public System.Action<Transform> OnTargetChanged;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnTargetChanged?.Invoke(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnTargetChanged?.Invoke(null);
        }
    }
}
