using UnityEngine;

public class QuestLocation : MonoBehaviour
{
    [Tooltip("이 이름이 퀘스트 파일의 'Quest Name'과 똑같아야 합니다.")]
    public string targetQuestName;

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 닿으면
        if (other.CompareTag("Player"))
        {
            // 매니저에게 알림
            QuestManager.instance.LocationReached(targetQuestName);

            // 마커는 이제 필요 없으니 끄기
            gameObject.SetActive(false);
        }
    }
}