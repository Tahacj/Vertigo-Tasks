using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class BattlepassManager : MonoBehaviour
{
    [Header("Data & References")]
    public List<BattlepassLevelSO> levelsData;
    public GameObject levelPrefab;
    public Transform levelsContainer;

    [Header("State")]
    [Tooltip("0 is the first level. -1 means no level is active.")]
    public int activeLevelIndex = 0;
    
    [Tooltip("Number of items to skip before numbering starts (e.g. 5 means index 5 is level 1)")]
    public int levelsToSkip = 0;

    [Header("Shine Colors")]
    public Color collectableShineColor = Color.white;
    public Color otherShineColor = new Color(1f, 1f, 1f, 0.5f);

    [Header("Halo Colors")]
    public Color collectableHaloColor = Color.yellow;
    public Color defaultHaloColor = Color.white;

    [Header("Follow Objects & UI")]
    public Transform divider;
    public Transform advanceButton;
    public TMP_Text currentLevelText;

    [Header("Scroll Buttons")]
    public ScrollRect scrollRect;
    public GameObject leftScrollButton;
    public GameObject rightScrollButton;

    private List<GameObject> spawnedLevels = new List<GameObject>();
    private Coroutine scrollCoroutine;
    private Transform targetIndicator;

    private HashSet<int> collectedPremiumRewards = new HashSet<int>();
    private HashSet<int> collectedNormalRewards = new HashSet<int>();

    public void MarkRewardCollected(int index, bool isPremium)
    {
        if (isPremium)
            collectedPremiumRewards.Add(index);
        else
            collectedNormalRewards.Add(index);
    }

    void Start()
    {
        GenerateLevels();
    }

    public void GenerateLevels()
    {
        if (levelsContainer == null || levelPrefab == null)
        {
            Debug.LogWarning("BattlepassManager: Missing prefab or container reference!");
            return;
        }

        foreach (Transform child in levelsContainer)
        {
            Destroy(child.gameObject);
        }
        spawnedLevels.Clear();

        for (int i = 0; i < levelsData.Count; i++)
        {
            GameObject levelObj = Instantiate(levelPrefab, levelsContainer);
            spawnedLevels.Add(levelObj);
        }

        UpdateActiveLevels();
    }

    public void UpdateActiveLevels()
    {
        targetIndicator = null;
        int actualActiveIndex = activeLevelIndex + levelsToSkip - 1;

        if (currentLevelText != null)
        {
            currentLevelText.text = (activeLevelIndex + 1).ToString();
        }

        if (leftScrollButton != null)
        {
            TMP_Text leftText = leftScrollButton.GetComponentInChildren<TMP_Text>();
            if (leftText != null) leftText.text = (activeLevelIndex + 1).ToString();
        }

        if (rightScrollButton != null)
        {
            TMP_Text rightText = rightScrollButton.GetComponentInChildren<TMP_Text>();
            if (rightText != null) rightText.text = (activeLevelIndex + 1).ToString();
        }

        for (int i = 0; i < spawnedLevels.Count; i++)
        {
            bool isLevelActive = i <= actualActiveIndex;
            GameObject levelObj = spawnedLevels[i];

            BattlepassLevelUI ui = levelObj.GetComponent<BattlepassLevelUI>();
            if (ui != null && i < levelsData.Count)
            {
                int levelNumber = i - levelsToSkip + 1;
                bool isPremiumCollected = collectedPremiumRewards.Contains(i);
                bool isNormalCollected = collectedNormalRewards.Contains(i);
                
                ui.Setup(levelsData[i], isLevelActive, levelNumber, collectableShineColor, otherShineColor, collectableHaloColor, defaultHaloColor, isPremiumCollected, isNormalCollected, i, this);
            }

            if (i == actualActiveIndex)
            {
                targetIndicator = levelObj.transform.Find("Current Level Indicator");
            }
        }
    }

    void Update()
    {
        if (targetIndicator != null)
        {
            if (divider != null) divider.position = targetIndicator.position;
            if (advanceButton != null) advanceButton.position = targetIndicator.position;
        }
        
        UpdateScrollButtons();
    }

    private void UpdateScrollButtons()
    {
        if (scrollRect == null || leftScrollButton == null || rightScrollButton == null) return;
        
        int actualActiveIndex = activeLevelIndex + levelsToSkip - 1;
        if (actualActiveIndex < 0 || actualActiveIndex >= spawnedLevels.Count) 
        {
            leftScrollButton.SetActive(false);
            rightScrollButton.SetActive(false);
            return;
        }

        RectTransform activeLevelRect = spawnedLevels[actualActiveIndex].GetComponent<RectTransform>();
        RectTransform viewport = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();

        if (activeLevelRect == null || viewport == null) return;

        Vector3[] levelCorners = new Vector3[4];
        activeLevelRect.GetWorldCorners(levelCorners);
        
        Vector3[] viewportCorners = new Vector3[4];
        viewport.GetWorldCorners(viewportCorners);

        bool isOffscreenLeft = levelCorners[2].x < viewportCorners[0].x;
        bool isOffscreenRight = levelCorners[0].x > viewportCorners[2].x;

        if (leftScrollButton.activeSelf != isOffscreenLeft) leftScrollButton.SetActive(isOffscreenLeft);
        if (rightScrollButton.activeSelf != isOffscreenRight) rightScrollButton.SetActive(isOffscreenRight);
    }

    public void IncreaseActiveLevel()
    {
        int maxIndex = levelsData.Count - 1 - levelsToSkip;
        if (activeLevelIndex < maxIndex)
        {
            activeLevelIndex++;
            UpdateActiveLevels();
        }
    }

    public void ScrollToActiveLevel()
    {
        if (scrollRect == null) return;

        int actualActiveIndex = activeLevelIndex + levelsToSkip - 1;
        if (actualActiveIndex < 0 || actualActiveIndex >= spawnedLevels.Count) return;

        RectTransform activeLevelRect = spawnedLevels[actualActiveIndex].GetComponent<RectTransform>();
        
        if (scrollCoroutine != null) StopCoroutine(scrollCoroutine);
        scrollCoroutine = StartCoroutine(SmoothScroll(activeLevelRect));
    }

    private System.Collections.IEnumerator SmoothScroll(RectTransform target)
    {
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();

        Vector3 startPosition = content.position;
        
        float offset = viewport.position.x - target.position.x;
        Vector3 targetPosition = startPosition + new Vector3(offset, 0, 0);

        float t = 0;
        float duration = 0.3f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float ease = Mathf.SmoothStep(0, 1, t);
            content.position = Vector3.Lerp(startPosition, targetPosition, ease);
            yield return null;
        }
        
        content.position = targetPosition;
        scrollRect.velocity = Vector2.zero;
    }

    private void OnValidate()
    {
        if (Application.isPlaying && spawnedLevels.Count > 0)
        {
            activeLevelIndex = Mathf.Clamp(activeLevelIndex, -1, levelsData.Count - 1 - levelsToSkip);
            UpdateActiveLevels();
        }
    }
}
