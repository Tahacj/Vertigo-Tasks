using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public struct BackgroundStateSprite
{
    public RewardBackgroundState state;
    public Sprite backgroundSprite;
}

public class BattlepassLevelUI : MonoBehaviour
{
    [Header("State UI Assets")]
    public Sprite activeBarImage;
    public Sprite emptyBarImage;
    public List<BackgroundStateSprite> backgroundStateSprites;

    [Header("Progress Bar References")]
    public GameObject levelIndicatorContainer;
    public Image barImage;
    public Image iconImage;
    public TMP_Text levelText;
    public GameObject iconContainer; 
    public GameObject levelTextContainer; 

    [Header("Premium Level References")]
    public GameObject premiumLevelContainer;
    public Image premiumBackground;
    public Image premiumBackgroundHalo;
    public TMP_Text premiumTitleText;
    public GameObject premiumLockIcon;
    public GameObject premiumExclamationMark;
    public Image premiumMiddleIcon;
    public TMP_Text premiumPrizeText;
    public Image premiumPrizeIcon;

    [Header("Shine References")]
    public RectTransform premiumShine;
    public RectTransform normalShine;

    [Header("Normal Level References")]
    public GameObject normalLevelContainer;
    public Image normalBackground;
    public GameObject normalExclamationMark;
    public Image normalMiddleIcon;
    public TMP_Text normalTitleText;
    public TMP_Text normalPrizeText;
    public Image normalPrizeIcon;

    private Coroutine premiumShineCoroutine;
    private Coroutine normalShineCoroutine;

    private RewardBackgroundState currentPremiumState;
    private RewardBackgroundState currentNormalState;
    private bool isPremiumLocked;
    private Color defaultHaloColorCached;
    private BattlepassManager manager;
    private int levelIndex;

    public void Setup(BattlepassLevelSO data, bool isActiveLevel, int levelNumber, Color collectableColor, Color otherColor, Color collectableHaloColor, Color defaultHaloColor, bool isPremiumCollected, bool isNormalCollected, int index, BattlepassManager mgr)
    {
        levelIndex = index;
        manager = mgr;
        if (barImage != null)
        {
            barImage.sprite = isActiveLevel ? activeBarImage : emptyBarImage;
        }

        if (levelIndicatorContainer != null)
        {
            levelIndicatorContainer.SetActive(data.hasLevelIndicator);
        }

        if (data.levelIcon != null)
        {
            if (iconContainer != null) iconContainer.SetActive(true);
            if (levelTextContainer != null) levelTextContainer.SetActive(false);
            if (iconImage != null) iconImage.sprite = data.levelIcon;
        }
        else
        {
            if (iconContainer != null) iconContainer.SetActive(false);
            if (levelTextContainer != null) levelTextContainer.SetActive(true);
            if (levelText != null) levelText.text = levelNumber.ToString();
        }

        defaultHaloColorCached = defaultHaloColor;

        if (premiumLevelContainer != null)
        {
            premiumLevelContainer.SetActive(data.hasPremiumReward);
            if (data.hasPremiumReward)
            {
                currentPremiumState = isActiveLevel ? RewardBackgroundState.Collectable : (RewardBackgroundState)data.premiumReward.state;
                if (isPremiumCollected) currentPremiumState = RewardBackgroundState.Collected;
                isPremiumLocked = data.premiumReward.isLocked;

                if (premiumBackground != null)
                    premiumBackground.sprite = GetBackgroundSprite(currentPremiumState);
                
                if (premiumBackgroundHalo != null)
                {
                    premiumBackgroundHalo.color = currentPremiumState == RewardBackgroundState.Collectable ? collectableHaloColor : defaultHaloColor;
                }
                
                if (premiumTitleText != null) premiumTitleText.text = data.premiumReward.titleText;
                if (premiumLockIcon != null) premiumLockIcon.SetActive(data.premiumReward.isLocked);
                
                if (premiumExclamationMark != null) 
                    premiumExclamationMark.SetActive(currentPremiumState == RewardBackgroundState.Collectable);
                
                if (premiumMiddleIcon != null)
                {
                    premiumMiddleIcon.sprite = data.premiumReward.rewardIcon;
                    premiumMiddleIcon.gameObject.SetActive(data.premiumReward.rewardIcon != null);
                }
                
                if (premiumPrizeText != null) premiumPrizeText.text = data.premiumReward.prizeText;
                
                if (premiumPrizeIcon != null)
                {
                    premiumPrizeIcon.sprite = data.premiumReward.prizeIcon;
                    premiumPrizeIcon.gameObject.SetActive(data.premiumReward.prizeIcon != null);
                }

                if (premiumShineCoroutine != null) StopCoroutine(premiumShineCoroutine);
                if (premiumShine != null)
                {
                    if (currentPremiumState == RewardBackgroundState.Collected)
                    {
                        premiumShine.gameObject.SetActive(false);
                    }
                    else
                    {
                        premiumShine.gameObject.SetActive(true);
                        Image img = premiumShine.GetComponent<Image>();
                        if (img != null) img.color = currentPremiumState == RewardBackgroundState.Collectable ? collectableColor : otherColor;
                        if (gameObject.activeInHierarchy)
                            premiumShineCoroutine = StartCoroutine(AnimateShine(premiumShine, -165f, 165f));
                    }
                }
            }
        }

        if (normalLevelContainer != null)
        {
            normalLevelContainer.SetActive(data.hasNormalReward);
            if (data.hasNormalReward)
            {
                currentNormalState = isActiveLevel ? RewardBackgroundState.Collectable : (RewardBackgroundState)data.normalReward.backgroundState;
                if (isNormalCollected) currentNormalState = RewardBackgroundState.Collected;

                if (normalBackground != null)
                    normalBackground.sprite = GetBackgroundSprite(currentNormalState); 
                
                if (normalTitleText != null) normalTitleText.text = data.normalReward.titleText;
                
                if (normalExclamationMark != null) 
                    normalExclamationMark.SetActive(currentNormalState == RewardBackgroundState.Collectable); 
                
                if (normalMiddleIcon != null)
                {
                    normalMiddleIcon.sprite = data.normalReward.rewardIcon;
                    normalMiddleIcon.gameObject.SetActive(data.normalReward.rewardIcon != null);
                }
                
                if (normalPrizeText != null) normalPrizeText.text = data.normalReward.prizeText;
                
                if (normalPrizeIcon != null)
                {
                    normalPrizeIcon.sprite = data.normalReward.prizeIcon;
                    normalPrizeIcon.gameObject.SetActive(data.normalReward.prizeIcon != null);
                }

                if (normalShineCoroutine != null) StopCoroutine(normalShineCoroutine);
                if (normalShine != null)
                {
                    if (currentNormalState == RewardBackgroundState.Collected)
                    {
                        normalShine.gameObject.SetActive(false);
                    }
                    else
                    {
                        normalShine.gameObject.SetActive(true);
                        Image img = normalShine.GetComponent<Image>();
                        if (img != null) img.color = currentNormalState == RewardBackgroundState.Collectable ? collectableColor : otherColor;
                        if (gameObject.activeInHierarchy)
                            normalShineCoroutine = StartCoroutine(AnimateShine(normalShine, -105f, 105f));
                    }
                }
            }
        }
    }

    private System.Collections.IEnumerator AnimateShine(RectTransform shineRect, float startX, float endX)
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));

        while (true)
        {
            Vector2 pos = shineRect.anchoredPosition;
            pos.x = startX;
            shineRect.anchoredPosition = pos;

            float t = 0;
            float duration = 1.5f;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                pos.x = Mathf.Lerp(startX, endX, t);
                shineRect.anchoredPosition = pos;
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }

    public void OnRewardCollected(bool isPremium)
    {
        if (isPremium)
        {
            if (isPremiumLocked) return;

            if (currentPremiumState == RewardBackgroundState.Collectable)
            {
                if (manager != null) manager.MarkRewardCollected(levelIndex, true);
                currentPremiumState = RewardBackgroundState.Collected;
                
                if (premiumBackground != null)
                    premiumBackground.sprite = GetBackgroundSprite(currentPremiumState);
                    
                if (premiumBackgroundHalo != null)
                    premiumBackgroundHalo.color = defaultHaloColorCached;
                    
                if (premiumExclamationMark != null)
                    premiumExclamationMark.SetActive(false);
                    
                if (premiumShineCoroutine != null) StopCoroutine(premiumShineCoroutine);
                if (premiumShine != null)
                    premiumShine.gameObject.SetActive(false);
            }
        }
        else
        {
            if (currentNormalState == RewardBackgroundState.Collectable)
            {
                if (manager != null) manager.MarkRewardCollected(levelIndex, false);
                currentNormalState = RewardBackgroundState.Collected;
                
                if (normalBackground != null)
                    normalBackground.sprite = GetBackgroundSprite(currentNormalState);
                    
                if (normalExclamationMark != null)
                    normalExclamationMark.SetActive(false);
                    
                if (normalShineCoroutine != null) StopCoroutine(normalShineCoroutine);
                if (normalShine != null)
                    normalShine.gameObject.SetActive(false);
            }
        }
    }

    private Sprite GetBackgroundSprite(RewardBackgroundState state)
    {
        if (backgroundStateSprites == null) return null;
        foreach (var mapping in backgroundStateSprites)
        {
            if (mapping.state == state)
                return mapping.backgroundSprite;
        }
        return null;
    }
}
