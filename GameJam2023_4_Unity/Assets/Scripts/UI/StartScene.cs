using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            animator.SetTrigger("toPlay");
        }
    }

    void Animation_OnPlayEnd()
    {
        SceneManager.LoadScene(1);
    }
}
