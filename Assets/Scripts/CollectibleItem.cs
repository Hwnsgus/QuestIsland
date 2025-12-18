using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Tooltip("퀘스트 파일의 targetItemName과 똑같이 적어야 합니다.")]
    public string itemName; // 예: "SmallWood"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 매니저에게 "나 주워졌어!"라고 알림
            QuestManager.instance.ItemCollected(itemName);

            // 아이템은 사라짐 (또는 인벤토리에 들어가는 로직)
            Destroy(gameObject);

            // 효과음이나 파티클을 여기서 재생해도 좋습니다.
            Debug.Log($"{itemName} 획득!");
        }
    }
}