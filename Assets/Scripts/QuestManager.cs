using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    // KillQuest -> QuestBase (모든 퀘스트 관리)
    public List<QuestBase> currentQuests = new List<QuestBase>();

    [Header("UI 설정")]
    public Transform questListContent;  // 아까 만든 QuestContainer 연결
    public GameObject questSlotPrefab;  // 아까 만든 QuestSlot 프리팹 연결

    void Awake()
    {
        instance = this;
    }

    // 매개변수(QuestBase quest)를 없애고, 리스트 전체를 출력하도록 변경
    public void UpdateQuestUI()
    {
        // 1. 기존에 떠 있던 슬롯들을 삭제
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        // 2. 현재 퀘스트 리스트를 돌면서 슬롯을 하나씩 생성
        foreach (var quest in currentQuests)
        {
            // (선택) 완료된 퀘스트 숨기기
            if (quest.isCompleted) continue; 

            // 프리팹 생성 
            GameObject newSlot = Instantiate(questSlotPrefab, questListContent);

            // 생성된 슬롯의 스크립트를 가져와서 내용 채우기
            QuestSlotUI slotScript = newSlot.GetComponent<QuestSlotUI>();

            if (slotScript != null)
            {
                slotScript.Setup(quest);
            }
        }
    }

    public void MonsterKilled(string tag)
    {
        foreach (var quest in currentQuests)
        {
            // 타입이 Kill이고 아직 완료 안 됐으면
            if (quest.type == QuestType.Kill && !quest.isCompleted)
            {
                // KillQuest로 변환해서 사용
                KillQuest killQ = (KillQuest)quest;

                if (killQ.targetTag == tag)
                {
                    killQ.currentKill++;
                    UpdateQuestUI(); // 카운트 올라가게 갱신

                    // 목표 달성 확인
                    if (killQ.currentKill >= killQ.killAmount)
                    {
                        //  목표 달성 시, NPC 아이콘을 즉시 '완료(파란색)'로 변경
                        if (quest.ownerNPC != null)
                        {
                            quest.ownerNPC.questIcon.UpdateIcon(QuestIconState.Complete);
                        }

                        // UI 갱신 (완료 메시지 띄우기 위해)
                        UpdateQuestUI();
                    }
                }
            }
        }
    }

    public void LocationReached(string reachedLocationName)
    {
        foreach (var quest in currentQuests)
        {
            // 타입이 Reach이고 아직 완료 안 됐으면
            if (quest.type == QuestType.Reach && !quest.isCompleted)
            {
                // 퀘스트 이름과 장소 이름이 같으면 완료!
                if (quest.questName == reachedLocationName)
                {
                    CompleteQuest(quest);
                    Debug.Log($"'{reachedLocationName}' 퀘스트 장소 도착 완료!");
                }
            }
        }
    }

    //아이템을 주웠을 때 호출
    public void ItemCollected(string itemName)
    {
        foreach (var quest in currentQuests)
        {
            // 수집 퀘스트이고, 아직 완료 안 됐으면
            if (quest.type == QuestType.Collect && !quest.isCompleted)
            {
                // CollectionQuest로 형변환
                CollectionQuest collectQ = (CollectionQuest)quest;

                // 이름이 같은지 확인
                if (collectQ.targetItemName == itemName)
                {
                    collectQ.currentAmount++;
                    Debug.Log($"수집 진행도: {collectQ.currentAmount}/{collectQ.requiredAmount}");
                    UpdateQuestUI();

                    if (collectQ.currentAmount >= collectQ.requiredAmount)
                    {
                        // [수정 2] 목표 달성 시, NPC 아이콘을 즉시 '완료(파란색)'로 변경
                        if (quest.ownerNPC != null)
                        {
                            quest.ownerNPC.questIcon.UpdateIcon(QuestIconState.Complete);
                        }

                        UpdateQuestUI();
                    }
                }
            }
        }
    }



    // [추가] 퀘스트 완료 시 텍스트 지우기
    // 중복되는 완료 코드를 함수로 정리
    public void CompleteQuest(QuestBase quest)
    {
        quest.isCompleted = true;

        if (quest.ownerNPC != null)
        {
            quest.ownerNPC.questIcon.UpdateIcon(QuestIconState.Complete);
        }

        UpdateQuestUI();
    }
}

