using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    // [변경] KillQuest -> QuestBase (모든 퀘스트 관리)
    public List<QuestBase> currentQuests = new List<QuestBase>();
    public TextMeshProUGUI questProgressText; // 퀘스트 진행도 UI 텍스트

    void Awake()
    {
        instance = this;
    }

    // [수정] 매개변수(QuestBase quest)를 없애고, 리스트 전체를 출력하도록 변경
    public void UpdateQuestUI()
    {
        if (questProgressText == null) return;

        // 1. 텍스트를 깨끗이 비웁니다.
        questProgressText.text = "";

        // 2. 현재 받은 퀘스트 리스트를 하나씩 돕니다.
        foreach (var quest in currentQuests)
        {
            // (선택) 이미 완료된 퀘스트는 목록에서 빼고 싶다면 아래 주석 해제
            // if (quest.isCompleted) continue;

            string content = "";

            // 3. 타입별로 내용 만들기
            if (quest.type == QuestType.Collect)
            {
                CollectionQuest cq = (CollectionQuest)quest;
                if (cq.currentAmount >= cq.requiredAmount)
                    content = $"{cq.questName} (완료!)";
                else
                    content = $"{cq.questName}: {cq.currentAmount} / {cq.requiredAmount}";
            }
            else if (quest.type == QuestType.Kill)
            {
                KillQuest kq = (KillQuest)quest;
                if (kq.currentKill >= kq.killAmount)
                    content = $"{kq.questName} (완료!)";
                else
                    content = $"{kq.questName}: {kq.currentKill} / {kq.killAmount}";
            }
            else if (quest.type == QuestType.Reach)
            {
                // 도착 퀘스트
                if (quest.isCompleted)
                    content = $"{quest.questName} (완료!)";
                else
                    content = $"{quest.questName}: 위치로 이동하세요";
            }

            // 4. 기존 텍스트 뒤에 내용을 이어 붙입니다 (+줄바꿈)
            questProgressText.text += content + "\n";
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
                    if (killQ.currentKill >= killQ.killAmount)
                    {
                        CompleteQuest(killQ);
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
                    UpdateQuestUI(); // UI 갱신
                    Debug.Log($"수집 진행도: {collectQ.currentAmount}/{collectQ.requiredAmount}");

                    // 목표 달성 시 완료 처리
                    if (collectQ.currentAmount >= collectQ.requiredAmount)
                    {
                        if (questProgressText != null)
                        {
                            questProgressText.text = $"{collectQ.questName}\n완료! {quest.ownerNPC.npcName}에게 돌아가세요.";
                            Debug.Log("수집 퀘스트 완료!");
                        }
                        else
                        {
                            CompleteQuest(collectQ);
                        }
                    }
                }
            }
        }
    }



    // [추가] 퀘스트 완료 시 텍스트 지우기
    public void ClearQuestUI()
    {
        if (questProgressText != null)
            questProgressText.text = "";
    }

    // 중복되는 완료 코드를 함수로 정리
    public void CompleteQuest(QuestBase quest)
    {
        quest.isCompleted = true;
        if (quest.ownerNPC != null)
        {
            quest.ownerNPC.questIcon.UpdateIcon(QuestIconState.Complete);
        }

        ClearQuestUI();
    }
}

