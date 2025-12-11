using System.Collections.Generic;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    // [변경] KillQuest -> QuestBase (모든 퀘스트 관리)
    public List<QuestBase> currentQuests = new List<QuestBase>();

    void Awake()
    {
        instance = this;
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

    // 중복되는 완료 코드를 함수로 정리
    void CompleteQuest(QuestBase quest)
    {
        quest.isCompleted = true;
        if (quest.ownerNPC != null)
        {
            quest.ownerNPC.questIcon.UpdateIcon(QuestIconState.Complete);
        }
    }
}

