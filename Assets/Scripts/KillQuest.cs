using UnityEngine;

[CreateAssetMenu(fileName = "New Kill Quest", menuName = "Quest/Kill Quest")]
public class KillQuest : QuestBase
{
    // questName, isCompleted, ownerNPC (부모 클래스 변수 사용)

    public string targetTag;      // 몬스터 태그
    public int killAmount;        // 목표 킬 수
    public int currentKill;       // 현재 잡은 수

    void OnEnable()
    {
        type = QuestType.Kill;    // 타입 자동 지정
    }
}