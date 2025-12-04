using System.Collections.Generic;
using UnityEngine;


public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public List<KillQuest> currentQuests = new List<KillQuest>();

    void Awake()
    {
        instance = this;
    }

    public void MonsterKilled(string tag)
    {
        foreach (var quest in currentQuests)
        {
            if (!quest.isCompleted && quest.targetTag == tag)
            {
                quest.currentKill++;

                if (quest.currentKill >= quest.killAmount)
                {
                    quest.isCompleted = true;

                    // NPC의 아이콘 업데이트 필요
                    quest.ownerNPC.questIcon.UpdateIcon(QuestIconState.Complete);
                }
            }
        }
    }
}
