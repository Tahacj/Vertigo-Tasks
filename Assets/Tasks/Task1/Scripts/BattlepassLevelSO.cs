using UnityEngine;

public enum RewardBackgroundState
{
    Collected = 0,
    Collectable = 1,
    Epic = 2,
    Legendary = 3,
    Mythic = 4,
    Rare = 5,
    Uncommon = 6
}

public enum RewardRarity
{
    Epic = 2,
    Legendary = 3,
    Mythic = 4,
    Rare = 5,
    Uncommon = 6
}

[System.Serializable]
public class PremiumRewardData
{
    public RewardRarity state;
    public string titleText;
    public bool isLocked;
    public Sprite rewardIcon;
    public string prizeText;
    public Sprite prizeIcon;
}

[System.Serializable]
public class NormalRewardData
{
    public RewardRarity backgroundState;
    public string titleText;
    public Sprite rewardIcon;
    public string prizeText;
    public Sprite prizeIcon;
}

[CreateAssetMenu(fileName = "New Battlepass Level", menuName = "Battlepass/Level Data")]
public class BattlepassLevelSO : ScriptableObject
{
    [Header("Level Availability")]
    public bool hasLevelIndicator;
    public bool hasPremiumReward;
    public bool hasNormalReward;

    [Header("Bar Settings")]
    public Sprite levelIcon;

    [Header("Premium Reward Settings")]
    public PremiumRewardData premiumReward;

    [Header("Normal Reward Settings")]
    public NormalRewardData normalReward;
}
