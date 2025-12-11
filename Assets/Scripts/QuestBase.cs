using UnityEngine;


public enum QuestType
{
    Kill,
    Reach
}

// 부모 클래스: ScriptableObject 상속
public class QuestBase : ScriptableObject
{
    public string questName;      // 퀘스트 이름
    public QuestType type;        // 퀘스트 타입
    public bool isCompleted;      // 완료 여부
    public NPCInteraction ownerNPC; // 퀘스트 주인
}