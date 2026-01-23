// GameConfigSO.cs

using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Treasure Hunt/Game Config")]
public class GameConfigSO : ScriptableObject
{
    [Header("Gameplay Settings")]
    [Range(3, 10)] public int ChestsPerRound = 5;
    [Range(1, 10)] public int MaxAttemptsPerRound = 3;
    [Range(1, 5)] public float ChestOpeningDuration = 2f;
    
    [Header("Reward Settings")]
    public RewardConfig[] PossibleRewards;
    
    [Header("Chest Settings")]
    public Sprite ClosedChestSprite;
    public Sprite OpeningChestSprite;
    public Sprite EmptyChestSprite;
    public Sprite WinningChestSprite;
    
    [System.Serializable]
    public class RewardConfig
    {
        public RewardType Type;
        public Vector2Int AmountRange;
        public Color DisplayColor;
    }
}

public enum RewardType
{
    Coins,
    Gems
    // Easily extensible
}