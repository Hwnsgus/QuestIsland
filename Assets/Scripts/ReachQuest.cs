using UnityEngine;

[CreateAssetMenu(fileName = "New Reach Quest", menuName = "Quest/Reach Quest")]
public class ReachQuest : QuestBase
{
    // 특별한 변수가 없어도 됩니다. 
    // questName만 일치하면 완료 처리할 것이기 때문입니다.

    void OnEnable()
    {
        type = QuestType.Reach;   // 타입 자동 지정
    }
}