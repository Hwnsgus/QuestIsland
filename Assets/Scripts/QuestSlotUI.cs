using UnityEngine;
using TMPro;

public class QuestSlotUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI progressText;

    public void Setup(QuestBase quest)
    {
        titleText.text = quest.questName;

        // 타입별 진행도 표시 로직 (QuestManager에 있던 걸 여기로 가져옴)
        if (quest.type == QuestType.Collect)
        {
            CollectionQuest cq = (CollectionQuest)quest;
            if (cq.currentAmount >= cq.requiredAmount)
                progressText.text = "(완료) 보상을 받으세요";
            else
                progressText.text = $"진행도: {cq.currentAmount} / {cq.requiredAmount}";
        }
        else if (quest.type == QuestType.Kill)
        {
            KillQuest kq = (KillQuest)quest;
            if (kq.currentKill >= kq.killAmount)
                progressText.text = "(완료) 보상을 받으세요";
            else
                progressText.text = $"처치: {kq.currentKill} / {kq.killAmount}";
        }
        else if (quest.type == QuestType.Reach)
        {
            if (quest.isCompleted) progressText.text = "(완료)";
            else progressText.text = "해당 위치로 이동하세요";
        }
    }
}