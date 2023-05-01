using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [Header("Children Elements")]
    [SerializeField] UI_Dialogue dialogue;
    [SerializeField] UI_SceneEndAni sceneEndAni;

    public void ShowStartSceneUI()
    {
        dialogue.gameObject.SetActive(true);
        dialogue.SetDialogueContents(dialogue.GetSceneStartDialogContents());
        dialogue.ShowNextDialogue();
    }
    public void ShowEndSceneUI()
    {
        dialogue.gameObject.SetActive(true);
        dialogue.SetDialogueContents(dialogue.GetSceneEndDialogContents());
        dialogue.ShowNextDialogue();
    }

    public void ShowNPCUI()
    {
        dialogue.gameObject.SetActive(true);
        dialogue.SetDialogueContents(
            GameManager.singleton.GetCurrentInteractNPC()
            .GetComponent<NPCInteractiveArea>().GetDialogueContents());
        dialogue.ShowNextDialogue();
    }

    public void HideUI()
    {
        dialogue.gameObject.SetActive(false);
        sceneEndAni.gameObject.SetActive(false);
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
        GameManager.singleton.FinishDialog();
    }
}
