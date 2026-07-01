using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>모든 퀘스트 완료 시 런타임에 구성되는 엔딩 화면.</summary>
public class EndingUIController : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public static void Show(int completedQuestCount)
    {
        if (FindFirstObjectByType<EndingUIController>() != null) return;
        GameObject root = new GameObject("Ending UI", typeof(RectTransform));
        root.AddComponent<EndingUIController>().Build(completedQuestCount);
    }

    private void Build(int completedQuestCount)
    {
        EnsureEventSystem();
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        gameObject.AddComponent<GraphicRaycaster>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        Stretch(GetComponent<RectTransform>());

        Image backdrop = CreateImage("Night Backdrop", transform, new Color(0.025f, 0.055f, 0.09f, 0.97f));
        Stretch(backdrop.rectTransform);
        Image glow = CreateImage("Island Glow", transform, new Color(0.08f, 0.48f, 0.42f, 0.22f));
        SetRect(glow.rectTransform, new Vector2(0.5f, 0.55f), new Vector2(980f, 620f), Vector2.zero);
        Image line = CreateImage("Gold Line", transform, new Color(1f, 0.78f, 0.3f, 0.95f));
        SetRect(line.rectTransform, new Vector2(0.5f, 0.79f), new Vector2(88f, 5f), Vector2.zero);

        TMP_FontAsset font = FindSceneFont();
        TextMeshProUGUI eyebrow = CreateText("Eyebrow", transform, "QUEST ISLAND  ·  JOURNEY COMPLETE", 24, font);
        eyebrow.color = new Color(1f, 0.78f, 0.3f);
        eyebrow.fontStyle = FontStyles.Bold;
        eyebrow.characterSpacing = 4f;
        SetRect(eyebrow.rectTransform, new Vector2(0.5f, 0.72f), new Vector2(900f, 60f), Vector2.zero);

        TextMeshProUGUI title = CreateText("Title", transform, "섬의 모든 이야기를\n완성했습니다", 74, font);
        title.fontStyle = FontStyles.Bold;
        title.color = new Color(0.96f, 0.98f, 0.93f);
        title.lineSpacing = -8f;
        SetRect(title.rectTransform, new Vector2(0.5f, 0.56f), new Vector2(1100f, 210f), Vector2.zero);

        TextMeshProUGUI body = CreateText("Message", transform,
            "당신의 발걸음 덕분에 퀘스트 아일랜드에 다시 평화가 찾아왔습니다.\n모험의 끝까지 함께해 주셔서 감사합니다.", 28, font);
        body.color = new Color(0.73f, 0.8f, 0.78f);
        body.lineSpacing = 8f;
        SetRect(body.rectTransform, new Vector2(0.5f, 0.39f), new Vector2(1100f, 100f), Vector2.zero);

        TextMeshProUGUI record = CreateText("Record", transform,
            $"COMPLETE  {completedQuestCount} / {completedQuestCount}", 23, font);
        record.fontStyle = FontStyles.Bold;
        record.color = new Color(0.45f, 0.92f, 0.75f);
        SetRect(record.rectTransform, new Vector2(0.5f, 0.29f), new Vector2(520f, 56f), Vector2.zero);

        Button replay = CreateButton("다시 모험하기", transform, font, new Vector2(-130f, -315f), true);
        replay.onClick.AddListener(RestartGame);
        Button quit = CreateButton("게임 종료", transform, font, new Vector2(130f, -315f), false);
        quit.onClick.AddListener(QuitGame);

        TextMeshProUGUI hint = CreateText("Hint", transform, "ENTER  다시 시작     ·     ESC  게임 종료", 18, font);
        hint.color = new Color(0.45f, 0.53f, 0.55f);
        SetRect(hint.rectTransform, new Vector2(0.5f, 0.08f), new Vector2(650f, 40f), Vector2.zero);

        Time.timeScale = 0f;
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) RestartGame();
        else if (Input.GetKeyDown(KeyCode.Escape)) QuitGame();
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < 1.2f)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.SmoothStep(0f, 1f, elapsed / 1.2f);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private static void RestartGame()
    {
        // ScriptableObject의 런타임 값은 씬 재로딩 후에도 남을 수 있어 직접 초기화합니다.
        foreach (NPCInteraction npc in FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None))
        {
            npc.questGiven = false;
            if (npc.quest == null) continue;
            npc.quest.isCompleted = false;
            npc.quest.ownerNPC = null;

            if (npc.quest is KillQuest killQuest) killQuest.currentKill = 0;
            else if (npc.quest is CollectionQuest collectQuest) collectQuest.currentAmount = 0;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private static void QuitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private static TMP_FontAsset FindSceneFont()
    {
        TextMeshProUGUI existing = FindFirstObjectByType<TextMeshProUGUI>();
        return existing != null ? existing.font : TMP_Settings.defaultFontAsset;
    }

    private static Image CreateImage(string name, Transform parent, Color color)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent, false);
        Image image = go.GetComponent<Image>();
        image.color = color;
        return image;
    }

    private static TextMeshProUGUI CreateText(string name, Transform parent, string value, float size, TMP_FontAsset font)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);
        TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = size;
        text.font = font;
        text.alignment = TextAlignmentOptions.Center;
        text.enableWordWrapping = false;
        return text;
    }

    private static Button CreateButton(string label, Transform parent, TMP_FontAsset font, Vector2 position, bool primary)
    {
        Image image = CreateImage(label + " Button", parent,
            primary ? new Color(0.94f, 0.68f, 0.24f) : new Color(0.12f, 0.2f, 0.23f));
        SetRect(image.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(230f, 64f), position);
        Button button = image.gameObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.highlightedColor = primary ? new Color(1f, 0.8f, 0.4f) : new Color(0.18f, 0.3f, 0.32f);
        colors.pressedColor = new Color(0.35f, 0.58f, 0.5f);
        button.colors = colors;
        TextMeshProUGUI text = CreateText("Label", image.transform, label, 23, font);
        text.fontStyle = FontStyles.Bold;
        text.color = primary ? new Color(0.08f, 0.12f, 0.12f) : new Color(0.9f, 0.94f, 0.91f);
        Stretch(text.rectTransform);
        return button;
    }

    private static void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null) return;
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void SetRect(RectTransform rect, Vector2 anchor, Vector2 size, Vector2 position)
    {
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }
}
