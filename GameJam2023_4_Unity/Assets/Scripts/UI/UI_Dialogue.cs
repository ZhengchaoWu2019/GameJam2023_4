using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Dialogue : MonoBehaviour
{
    public System.Action OnDialogueFinished;

    [Header("Properties")]
    [SerializeField] List<DialogueContent> dialogueContents;
    [SerializeField] float sayingSpeed;

    [Header("Components")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] Button nextBtn;

    [Header("Read Only")]
    [SerializeField] bool isShowingFinished = true;
    [SerializeField] int toShowNextCount;
    [SerializeField] int dialogIndex;

    public void ShowNextDialogue()
    {
        toShowNextCount++;
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
            if (dialogIndex < dialogueContents.Count)
            {
                DialogueContent currentDialog = dialogueContents[dialogIndex];

                ShowDialogue(currentDialog.name, currentDialog.content);
                isShowingFinished = false;

                toShowNextCount--;

                dialogIndex++;
            }
            else
            {
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
