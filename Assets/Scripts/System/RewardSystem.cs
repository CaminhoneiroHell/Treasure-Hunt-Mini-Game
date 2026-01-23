using UnityEngine;

namespace System
{
// RewardModel.cs
    public abstract class Reward
    {
        public abstract RewardType Type { get; }
        public int Amount { get; protected set; }
    
        public abstract void Apply(PlayerInventoryModel inventory);
        // public abstract Sprite GetIcon();
    }

    public class CoinReward : Reward
    {
        public override RewardType Type => RewardType.Coins;
    
        public CoinReward(int amount) => Amount = amount;
    
        public override void Apply(PlayerInventoryModel inventory)
        {
            inventory.AddCoins(Amount);
        }
    
        // public override Sprite GetIcon() => Resources.Load<Sprite>("Icons/Coin");
    }

    public class GemReward : Reward
    {
        public override RewardType Type => RewardType.Gems;
    
        public GemReward(int amount) => Amount = amount;
    
        public override void Apply(PlayerInventoryModel inventory)
        {
            inventory.AddGems(Amount);
        }
    
        // public override Sprite GetIcon() => Resources.Load<Sprite>("Icons/Gem");
    }

// Factory Pattern for easy extension
    public class RewardFactory
    {
        public static Reward CreateReward(RewardType type, int amount)
        {
            return type switch
            {
                RewardType.Coins => new CoinReward(amount),
                RewardType.Gems => new GemReward(amount),
                _ => throw new System.ArgumentException($"Unknown reward type: {type}")
            };
        }
    }
}