using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] InteractableArea interactableArea;
    [SerializeField] Animator animator;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (interactableArea.CheckIsPlayerIn())
            {
                Player currentPlayer = interactableArea.GetPlayer();
                if (currentPlayer.FireCount >= 2)
                {
                    animator.SetTrigger("toDisappear");
                    currentPlayer.FireCount--;
                }
            }
        }
    }
}
