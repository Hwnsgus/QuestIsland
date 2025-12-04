using UnityEngine;


public class NPCInteraction : MonoBehaviour
{
    public string[] dialogueBeforeQuest;  // 대사 여러 줄 저장
    public string[] dialogueAfterQuest;



    public KillQuest quest;         // NPC가 주는 퀘스트
    public NPCQuestIcon questIcon;  // NPC 아이콘
    public bool questGiven = false;

    public DialogueManager dialogueManager;
    public bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            dialogueManager.SetCurrentNPC(this);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueManager.SetCurrentNPC(null);
        }
    }
}
 