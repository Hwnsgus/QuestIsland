using UnityEngine;
using UnityEngine.UI;

public enum QuestIconState
{
    None,
    Available,      // 수락 가능 (!)
    InProgress,     // 진행 중 (?)
    Complete        // 완료 가능 (!)
}

public class NPCQuestIcon : MonoBehaviour
{
    public Image iconImage;

    public Sprite iconAvailable;   // 수락 가능 !
    public Sprite iconInProgress;  // 진행 중 ?
    public Sprite iconComplete;    // 완료 가능 !

    private QuestIconState currentState = QuestIconState.None;

    void Start()
    {
        UpdateIcon(QuestIconState.Available);
    }

    public void UpdateIcon(QuestIconState state)
    {
        currentState = state;

        switch (state)
        {
            case QuestIconState.Available:
                iconImage.sprite = iconAvailable;
                iconImage.gameObject.SetActive(true);
                break;

            case QuestIconState.InProgress:
                iconImage.sprite = iconInProgress;
                iconImage.gameObject.SetActive(true);
                break;

            case QuestIconState.Complete:
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = iconComplete;

                iconImage.gameObject.SetActive(false);
                break;

            case QuestIconState.None:
            default:
                iconImage.gameObject.SetActive(false);
                break;
        }
    }

    public void HideIcon()
    {
        if (iconImage != null)
            iconImage.gameObject.SetActive(false);
    }

}
