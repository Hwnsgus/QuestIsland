using UnityEngine;

public class MonsterScripts : MonoBehaviour
{
    public int hp = 2;                 // 간단히 1회 공격 → 사망
    public string monsterTag = "Cow";  // 몬스터 종류 (KillQuest.targetTag 와 동일해야 함)

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 퀘스트 카운트 증가
        QuestManager.instance.MonsterKilled(monsterTag);

        Destroy(gameObject); // 몬스터 삭제
    }
}
