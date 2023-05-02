using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractiveArea : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] List<DialogueContent> npcBeforeDialogContents;
    [SerializeField] List <DialogueContent> npcAfterDialogContents;

    [Header("Components")]
    [SerializeField] GameObject taskStartTriggerGO;
    [SerializeField] GameObject taskFinishTriggerGO;
    [SerializeField] GameObject taskFinishDestroyGO;

    [Header("Read Only")]
    [SerializeField] bool isInteracted = false;
    [SerializeField] bool tackFinished = false;

    public List<DialogueContent> GetBeforeDialogueContents()
    {
        return npcBeforeDialogContents;
    }

    public List<DialogueContent> GetAfterDialogueContents()
    {
        return npcAfterDialogContents;
    }

    public bool CheckIsFinishedTack()
    {
        return tackFinished;
    }

    public void FinishTask()
    {
        tackFinished = true;
    }

    public void FinishDialog()
    {
        if(tackFinished)
        {
            isInteracted = false;
            Destroy(taskFinishDestroyGO);
            taskFinishTriggerGO.SetActive(true);
            return;
        }

        taskStartTriggerGO.SetActive(true);
        isInteracted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInteracted)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            isInteracted = true;
            GameManager.singleton.StartInteractWithNPC(gameObject);
        }
    }
}
