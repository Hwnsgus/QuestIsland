using UnityEngine;


public class NPCInteraction : MonoBehaviour
{
    public string[] dialogueBeforeQuest;  // 대사 여러 줄 저장
    public string[] dialogueAfterQuest;
    public string[] dialogueDuringQuest;



    // [변경] KillQuest -> QuestBase (모든 퀘스트 허용)
    public QuestBase quest;

    public NPCQuestIcon questIcon;  // NPC 아이콘
    public bool questGiven = false;

    public DialogueManager dialogueManager;
    public bool playerInRange = false;

    public string[] currentDialogue
    {
        get
        {
            if (quest != null && quest.isCompleted)
            {
                return dialogueAfterQuest; // 3. 완료 후 대사
            }
            else if (questGiven)
            {
                return dialogueDuringQuest; // 2. 진행 중 대사
            }
            else
            {
                return dialogueBeforeQuest; // 1. 시작 전 대사
            }
        }
    }

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
 