using UnityEngine;

[CreateAssetMenu(fileName = "New Collection Quest", menuName = "Quest/Collection Quest")]
public class CollectionQuest : QuestBase
{
    [Header("물품 수집")]
    public string targetItemName; // 주워야 할 아이템 이름 (예: "SmallWood")
    public int requiredAmount;    // 목표 수량 (예: 2)
    public int currentAmount;     // 현재 모은 수량

    void OnEnable()
    {
        type = QuestType.Collect; // 타입 자동 지정
    }
}