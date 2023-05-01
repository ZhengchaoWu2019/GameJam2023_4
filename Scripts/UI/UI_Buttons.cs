using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Buttons : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] string firstLevelName;


    public void OnStartBtnClicked()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    public void OnPlayAgainBtnClicked()
    {
        SceneManager.LoadScene(0);
    }
}
