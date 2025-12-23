using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;        // UI Panel
    public TextMeshProUGUI dialogueText;    // TMP 텍스트
    public TextMeshProUGUI nameText;        // NPC 이름 텍스트
    private string[] lines;                 // 지금 출력할 대사
    private int index = 0;
    private NPCInteraction currentNPC;
    private bool isReadyToFinish = false;

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
        isReadyToFinish = false; // 대화 시작할 때 초기화

        if (nameText != null)
        {
            nameText.text = npc.npcName;
        }

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
        else if (npc.questGiven && npc.quest != null)
        {
            // 혹시 수집 퀘스트이고, 물건을 다 모아왔는지 체크
            if (npc.quest.type == QuestType.Collect)
            {
                CollectionQuest cQuest = (CollectionQuest)npc.quest;
                if (cQuest.currentAmount >= cQuest.requiredAmount)
                {
                    // 다 모아왔으면 '완료 후 대사'를 미리 보여줌
                    lines = npc.dialogueAfterQuest;
                    isReadyToFinish = true; // 대화 끝나면 퀘스트 완료 처리
                }
                else
                {
                    lines = npc.dialogueDuringQuest; // 아직 덜 모음
                }
            }
            else if (npc.quest.type == QuestType.Kill)
            {
                KillQuest kQuest = (KillQuest)npc.quest;

                // 현재 잡은 수(currentKill)가 목표(killAmount)보다 많거나 같으면?
                if (kQuest.currentKill >= kQuest.killAmount)
                {
                    lines = npc.dialogueAfterQuest; // (완료 대사)
                    isReadyToFinish = true;         // 대화 끝나면 퀘스트 완료시키기
                }
                else
                {
                    lines = npc.dialogueDuringQuest; // "더 잡고 와라"
                }
            }

            else
            {
                lines = npc.dialogueDuringQuest;
            }
        }

        index = 0;
        dialoguePanel.SetActive(true);
        if (lines != null && lines.Length > 0)
            dialogueText.text = lines[index];
    }


    void NextLine()
    {
        index++;

        if (lines != null && index < lines.Length)
        {
            dialogueText.text = lines[index];
        }
        else
        {
            dialoguePanel.SetActive(false);

            if (currentNPC != null)
            {
                // 퀘스트가 아예 없는 NPC(단순 대화 NPC)라면 여기서 중단
                if (currentNPC.quest == null)
                {
                    // 퀘스트가 없으니 그냥 대화만 닫고 끝냄
                    return;
                }

                // 케이스 1: 퀘스트 수락 대사 끝난 순간
                if (!currentNPC.questGiven)
                {
                    AcceptQuest(currentNPC);
                }
                // 케이스 2: 완료 대사 끝난 순간
                // currentNPC.quest가 null인지 먼저 확인
                else if (currentNPC.quest != null && currentNPC.quest.isCompleted)
                {
                    // [안전장치 2] 아이콘이 연결 안 되어 있을 수도 있으니 확인
                    if (currentNPC.questIcon != null)
                        currentNPC.questIcon.HideIcon();
                }
                // 케이스 3: 수집 퀘스트 완료 보고
                else if (isReadyToFinish)
                {
                    // 안전장치
                    if (QuestManager.instance == null)
                    {
                        Debug.LogError("오류: 씬에 QuestManager가 없습니다!");
                        return;
                    }

                    if (currentNPC.quest == null)
                    {
                        Debug.LogError($"오류: {currentNPC.npcName}에게 퀘스트 파일이 연결되지 않았습니다!");
                        return;
                    }

                    QuestManager.instance.CompleteQuest(currentNPC.quest);
                    isReadyToFinish = false;
                }
            }
        }
    }



    public void AcceptQuest(NPCInteraction npc)
    {
        if (!npc.questGiven)
        {
            // [공통 초기화] 모든 퀘스트가 가지고 있는 변수 초기화
            npc.quest.isCompleted = false;
            npc.quest.ownerNPC = npc;

            // [타입별 초기화] 퀘스트 종류에 따라 다르게 처리
            if (npc.quest.type == QuestType.Kill)
            {
                // npc.quest를 'KillQuest' 모양으로 강제 변환(Casting)해야 currentKill이 보입니다.
                KillQuest killQ = (KillQuest)npc.quest;
                killQ.currentKill = 0;
            }
            // 만약 ReachQuest에 초기화할 변수가 있다면 여기서 else if로 처리하면 됩니다.
            else if (npc.quest.type == QuestType.Collect)
            {
                CollectionQuest collectQ = (CollectionQuest)npc.quest;
                collectQ.currentAmount = 0;
            }

            // 매니저에 등록
            QuestManager.instance.currentQuests.Add(npc.quest);
            npc.questGiven = true;

            npc.questIcon.UpdateIcon(QuestIconState.InProgress);   // 아이콘 변경!
            QuestManager.instance.UpdateQuestUI(); // 퀘스트 진행상황 UI 갱신
        }
    }

}
