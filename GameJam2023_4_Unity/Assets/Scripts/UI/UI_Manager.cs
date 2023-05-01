using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [Header("Children Elements")]
    [SerializeField] UI_Dialogue dialogue;
    [SerializeField] UI_SceneEndAni sceneEndAni;

    public void ShowUI()
    {
        dialogue.gameObject.SetActive(true);

        dialogue.ShowNextDialogue();
    }

    public void HideUI()
    {
        dialogue.gameObject.SetActive(false);
    }

    public void PlaySceneEndAni()
    {
        sceneEndAni.gameObject.SetActive(true);
    }

    public bool CheckIsSceneEndAniFinished()
    {
        return sceneEndAni.CheckIsAniEnd();
    }

    #region Life Cycle
    private void Awake()
    {
        dialogue.OnDialogueFinished += OnDialogueFinished;
    }
    #endregion

    void OnDialogueFinished()
    {
        HideUI();

        GameManager.singleton.PlayerStartControl();
    }
}
