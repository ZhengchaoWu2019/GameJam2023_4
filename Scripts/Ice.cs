using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TriggerArea iceArea;
    [SerializeField] Animator animator;

    private void Start()
    {
        iceArea.OnFireGhostCollided += OnFireGhostCollided;
    }

    void OnFireGhostCollided()
    {
        animator.SetTrigger("toDisappear");

        GameManager.singleton.ChangeControlToPlayer();
    }
}
