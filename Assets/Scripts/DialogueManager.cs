using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;        // UI Panel
    public TextMeshProUGUI dialogueText;    // TMP 텍스트
    private string[] lines;                 // 지금 출력할 대사
    private int index = 0;
    private NPCInteraction currentNPC;

    void Update()
    {
        // f 누름
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 이미 대화 중이라면 다음 줄 출력
            if (dialoguePanel.activeSelf)
            {
                NextLine();
            }
            else if (currentNPC != null && currentNPC.playerInRange)
            {
                StartDialogue(currentNPC);
            }
        }
    }

    public void SetCurrentNPC(NPCInteraction npc)
    {
        currentNPC = npc;
    }

    void StartDialogue(NPCInteraction npc)
    {
        if (npc.quest != null && npc.quest.isCompleted)
        {
            lines = npc.dialogueAfterQuest;
        }
        // 2) 퀘스트 아직 시작 전
        else if (!npc.questGiven)
        {
            lines = npc.dialogueBeforeQuest;
        }
        // 3) 퀘스트 진행 중 (중요!)
        else
        {
            lines = npc.dialogueDuringQuest;
        }
        index = 0;
        dialoguePanel.SetActive(true);
        dialogueText.text = lines[index];
    }


    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            dialogueText.text = lines[index];
        }
        else
        {
            dialoguePanel.SetActive(false);

            if (currentNPC != null)
            {
                // 케이스 1: 퀘스트 수락 대사 끝난 순간
                if (!currentNPC.questGiven)
                {
                    AcceptQuest(currentNPC);
                }
                // 케이스 2: 완료 대사 끝난 순간 → 이때만 아이콘 삭제
                else if (currentNPC.quest.isCompleted)
                {
                    currentNPC.questIcon.HideIcon();
                }
            }
        }
    }



    public void AcceptQuest(NPCInteraction npc)
    {
        if (!npc.questGiven)
        {
            // [중요] ScriptableObject는 게임을 꺼도 데이터가 저장될 수 있으므로
            // 수락 시점에 초기화해주는 것이 안전합니다.
            npc.quest.currentKill = 0;
            npc.quest.isCompleted = false;

            // [중요] 퀘스트 매니저가 나중에 완료 아이콘을 띄울 때 누구 머리 위에 띄울지 알아야 합니다.
            npc.quest.ownerNPC = npc;

            QuestManager.instance.currentQuests.Add(npc.quest);
            npc.questGiven = true;

            npc.questIcon.UpdateIcon(QuestIconState.InProgress);   // 아이콘 변경!
        }
    }

}
