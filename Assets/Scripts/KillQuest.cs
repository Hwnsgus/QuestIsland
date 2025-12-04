using UnityEngine;

[CreateAssetMenu(fileName = "New Kill Quest", menuName = "Quest/Kill Quest")]
public class KillQuest : ScriptableObject
{
    public string questName;      // 퀘스트 이름
    public string targetTag;      // 몬스터 태그
    public int killAmount;        // 목표 킬 수

    public int currentKill;       // 현재 잡은 수
    public bool isCompleted;      // 완료 여부

    public NPCInteraction ownerNPC;
}