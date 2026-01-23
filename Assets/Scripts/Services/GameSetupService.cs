using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Services
{
    public class GameSetupService
    {
        private GameConfigSO _config;
    
        public List<ChestModel> CreateChestsForRound()
        {
            var chests = new List<ChestModel>();
            int winningIndex = Random.Range(0, _config.ChestsPerRound);
        
            for (int i = 0; i < _config.ChestsPerRound; i++)
            {
                chests.Add(new ChestModel($"Chest_{i}", i == winningIndex));
            }
        
            return chests;
        }
    
        public Reward CreateRandomReward()
        {
            var rewardConfig = _config.PossibleRewards[
                Random.Range(0, _config.PossibleRewards.Length)];
        
            int amount = Random.Range(
                rewardConfig.AmountRange.x, 
                rewardConfig.AmountRange.y + 1);
        
            return RewardFactory.CreateReward(rewardConfig.Type, amount);
        }
    }
}