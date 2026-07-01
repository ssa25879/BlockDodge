using UnityEngine;
using UnityEngine.UI;

public enum DifficultyTier
{
    Easy,
    Normal,
    Hard,
    VeryHard
}

[System.Serializable]
public class DifficultyStage
{
    public DifficultyTier tier = DifficultyTier.Easy;
    public float startTime = 0f;
    public int activeSpawnerCount = 1;
    public float rateMin = 1.4f;
    public float rateMax = 2.4f;
    public float bulletSpeedInitial = 7f;
    public float bulletSpeedIncrement = 0.04f;
    public float bulletSpeedMax = 9f;
    public BulletPattern bulletPattern = BulletPattern.Single;
    public int patternBulletCount = 1;
    public float burstInterval = 0.12f;
    public float spreadAngle = 15f;
}

/// <summary>
/// 생존 시간에 따라 탄환 스포너의 난이도 단계를 조절합니다.
/// </summary>
[DefaultExecutionOrder(-100)]
public class DifficultyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Tooltip("비워두면 씬의 BulletSpawner를 자동으로 찾습니다.")]
    private BulletSpawner[] spawners;

    [SerializeField, Tooltip("현재 난이도 상태를 표시할 UI 텍스트입니다. 비워두면 자동 생성합니다.")]
    private Text difficultyText;

    [Header("Stages")]
    [SerializeField, Tooltip("생존 시간 기준으로 적용할 난이도 단계입니다.")]
    private DifficultyStage[] stages =
    {
        new DifficultyStage
        {
            tier = DifficultyTier.Easy,
            startTime = 0f,
            activeSpawnerCount = 1,
            rateMin = 1.4f,
            rateMax = 2.4f,
            bulletSpeedInitial = 7f,
            bulletSpeedIncrement = 0.04f,
            bulletSpeedMax = 9f,
            bulletPattern = BulletPattern.Single,
            patternBulletCount = 1
        },
        new DifficultyStage
        {
            tier = DifficultyTier.Normal,
            startTime = 20f,
            activeSpawnerCount = 2,
            rateMin = 1.0f,
            rateMax = 1.8f,
            bulletSpeedInitial = 8.5f,
            bulletSpeedIncrement = 0.05f,
            bulletSpeedMax = 11f,
            bulletPattern = BulletPattern.Single,
            patternBulletCount = 1
        },
        new DifficultyStage
        {
            tier = DifficultyTier.Hard,
            startTime = 45f,
            activeSpawnerCount = 3,
            rateMin = 0.75f,
            rateMax = 1.35f,
            bulletSpeedInitial = 10f,
            bulletSpeedIncrement = 0.06f,
            bulletSpeedMax = 13f,
            bulletPattern = BulletPattern.Burst,
            patternBulletCount = 2,
            burstInterval = 0.12f
        },
        new DifficultyStage
        {
            tier = DifficultyTier.VeryHard,
            startTime = 75f,
            activeSpawnerCount = 4,
            rateMin = 0.55f,
            rateMax = 1.0f,
            bulletSpeedInitial = 11.5f,
            bulletSpeedIncrement = 0.05f,
            bulletSpeedMax = 15f,
            bulletPattern = BulletPattern.Spread,
            patternBulletCount = 3,
            spreadAngle = 12f
        }
    };

    private GameManager gameManager;
    private int currentStageIndex = -1;
    private int currentActiveSpawnerCount;

    public DifficultyTier CurrentTier { get; private set; } = DifficultyTier.Easy;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsureDifficultyManager()
    {
        if (FindFirstObjectByType<DifficultyManager>() != null) { return; }
        if (FindFirstObjectByType<GameManager>() == null) { return; }
        if (FindObjectsByType<BulletSpawner>(FindObjectsSortMode.None).Length == 0) { return; }

        new GameObject("DifficultyManager").AddComponent<DifficultyManager>();
    }

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (spawners == null || spawners.Length == 0)
        {
            spawners = FindObjectsByType<BulletSpawner>(FindObjectsSortMode.InstanceID);
        }
    }

    private void Start()
    {
        EnsureDifficultyText();
        ApplyStage(GetStageIndex(0f));
    }

    private void Update()
    {
        if (gameManager == null || gameManager.IsGameover) { return; }

        int stageIndex = GetStageIndex(gameManager.SurviveTime);
        if (stageIndex != currentStageIndex)
        {
            ApplyStage(stageIndex);
        }

        UpdateDifficultyText(stages[currentStageIndex], currentActiveSpawnerCount);
    }

    private int GetStageIndex(float surviveTime)
    {
        int stageIndex = 0;

        for (int i = 0; i < stages.Length; i++)
        {
            if (surviveTime >= stages[i].startTime)
            {
                stageIndex = i;
            }
        }

        return stageIndex;
    }

    private void ApplyStage(int stageIndex)
    {
        if (stages == null || stages.Length == 0 || spawners == null) { return; }

        currentStageIndex = Mathf.Clamp(stageIndex, 0, stages.Length - 1);
        DifficultyStage stage = stages[currentStageIndex];
        CurrentTier = stage.tier;

        int activeCount = Mathf.Clamp(stage.activeSpawnerCount, 0, spawners.Length);
        currentActiveSpawnerCount = activeCount;

        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i] == null) { continue; }

            spawners[i].ApplyDifficulty(
                stage.rateMin,
                stage.rateMax,
                stage.bulletSpeedInitial,
                stage.bulletSpeedIncrement,
                stage.bulletSpeedMax,
                stage.bulletPattern,
                stage.patternBulletCount,
                stage.burstInterval,
                stage.spreadAngle);
            spawners[i].SetSpawnerActive(i < activeCount);
        }

        UpdateDifficultyText(stage, activeCount);
        Debug.Log($"[Difficulty] {stage.tier} | Active Spawners: {activeCount}/{spawners.Length} | Pattern: {stage.bulletPattern}");
    }

    private void EnsureDifficultyText()
    {
        if (difficultyText != null) { return; }

        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null) { return; }

        GameObject textObject = new GameObject("DifficultyText");
        textObject.transform.SetParent(canvas.transform, false);

        difficultyText = textObject.AddComponent<Text>();
        difficultyText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (difficultyText.font == null)
        {
            difficultyText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        difficultyText.fontSize = 28;
        difficultyText.color = Color.white;
        difficultyText.alignment = TextAnchor.UpperLeft;
        difficultyText.raycastTarget = false;

        RectTransform rectTransform = difficultyText.rectTransform;
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);
        rectTransform.anchoredPosition = new Vector2(20f, -70f);
        rectTransform.sizeDelta = new Vector2(520f, 90f);
    }

    private void UpdateDifficultyText(DifficultyStage stage, int activeCount)
    {
        if (difficultyText == null || stage == null) { return; }

        difficultyText.text =
            $"Difficulty: {stage.tier}\n" +
            $"Cannons: {activeCount}/{spawners.Length}  Pattern: {stage.bulletPattern}";
    }
}
