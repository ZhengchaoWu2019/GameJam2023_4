using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Dialogue : MonoBehaviour
{
    public System.Action OnDialogueFinished;

    [Header("Properties")]
    [SerializeField] List<DialogueContent> sceneStartDialogContents;
    [SerializeField] List<DialogueContent> sceneEndDialogContents;
    [SerializeField] float sayingSpeed;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] Button nextBtn;

    [Header("Read Only")]
    [SerializeField] bool isShowingFinished = true;
    [SerializeField] int toShowNextCount;
    [SerializeField] List<DialogueContent> currentDialogContents;
    [SerializeField] int sceneStartDialogIndex;
    [SerializeField] int sceneEndDialogIndex;
    [SerializeField] int currentDialogIndex;

    public void ShowNextDialogue()
    {
        toShowNextCount++;
    }

    public void SetDialogueContents(List<DialogueContent> newContents)
    {
        currentDialogIndex = 0;
        currentDialogContents = newContents;
    }

    public List<DialogueContent> GetSceneStartDialogContents()
    {
        return sceneStartDialogContents;
    }

    public List<DialogueContent> GetSceneEndDialogContents()
    {
        return sceneEndDialogContents;
    }

    #region Life Cycle
    private void Awake()
    {
        nextBtn.onClick.AddListener(() =>
        {
            ShowNextDialogue();
        });

        isShowingFinished = true;
    }

    private void Update()
    {
        if (!isShowingFinished)
        {
            return;
        }

        if (toShowNextCount > 0)
        {
            if (currentDialogIndex < currentDialogContents.Count)
            {
                DialogueContent currentDialog = currentDialogContents[currentDialogIndex];

                ShowDialogue(currentDialog.name, currentDialog.content);
                isShowingFinished = false;

                toShowNextCount--;

                currentDialogIndex++;

            }
            else
            {
                toShowNextCount = 0;
                OnDialogueFinished?.Invoke();
            }
        }

    }
    #endregion

    void ShowDialogue(string name, string dialogContent)
    {
        nameText.text = name;

        StartCoroutine(ShowContentPerChar(dialogContent));
    }

    IEnumerator ShowContentPerChar(string content)
    {
        dialogText.text = "";

        char[] chars = content.ToCharArray();

        foreach(char c in chars)
        {
            dialogText.text += c;

            yield return new WaitForSeconds(sayingSpeed * 0.01f);
        }

        isShowingFinished = true;
    }
}
