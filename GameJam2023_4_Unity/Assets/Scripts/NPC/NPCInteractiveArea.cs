using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractiveArea : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] List<DialogueContent> npcDialogContents;

    [Header("Components")]
    [SerializeField] GameObject taskBindTreasureGO;

    [Header("Read Only")]
    [SerializeField] bool isInteracted = false;

    public List<DialogueContent> GetDialogueContents()
    {
        return npcDialogContents;
    }

    public void TriggerTaskFinishOperation()
    {
        taskBindTreasureGO.SetActive(true);
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
