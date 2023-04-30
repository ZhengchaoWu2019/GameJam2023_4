using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] InteractableArea interactableArea;
    [SerializeField] TriggerArea woodArea;
    [SerializeField] Animator animator;

    [Header("Read Only")]
    [SerializeField] bool firing;

    public void Extinct()
    {
        animator.SetTrigger("toExtinct");
        firing = false;
    }

    public bool CheckIsFiring()
    {
        return firing;
    }
    private void Start()
    {
        woodArea.OnFireGhostCollided += OnFireGhostCollided;
    }

    private void Update()
    {
        if (firing)
        {
            return;
        }
    }

    void OnFireGhostCollided()
    {
        if (firing)
        {
            return;
        }

        animator.SetTrigger("toFire");
        firing = true;

        GameManager.singleton.ChangeControlToPlayer();
    }
}
