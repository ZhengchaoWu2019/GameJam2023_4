using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] InteractableArea interactableArea;
    [SerializeField] WoodArea woodArea;
    [SerializeField] Animator animator;

    [Header("Read Only")]
    [SerializeField] bool firing;

    public void Extinct()
    {
        animator.SetTrigger("toExtinct");
        firing = false;
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (interactableArea.CheckIsPlayerIn())
            {
                if(interactableArea.GetPlayer().FireCount >= 2)
                {
                    interactableArea.GetPlayer().FireCount--;

                    animator.SetTrigger("toFire");
                    firing = true;

                    GameManager.singleton.SetFiringWood(this);
                }
            }
        }
    }

    void OnFireGhostCollided()
    {
        if (firing)
        {
            return;
        }

        if (GameManager.singleton.CheckIsCurrentFiringWood(gameObject))
        {
            return ;
        }

        animator.SetTrigger("toFire");
        firing = true;

        GameManager.singleton.ChangeControlToPlayer();
    }
}
