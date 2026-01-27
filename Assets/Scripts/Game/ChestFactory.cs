using System.Collections.Generic;
using UnityEngine;

namespace TreasureHuntMiniGame
{
    public class ChestFactory

    {
        private GameObject _chestPrefab;
        private Transform _chestContainer;
        private ChestOpeningTask _openingManager;
        
        private int _chestCounter = 0;

        public ChestFactory(GameObject chestPrefab, Transform chestContainer, ChestOpeningTask openingManager)
        {
            _chestPrefab = chestPrefab;
            _chestContainer = chestContainer;
            _openingManager = openingManager;
        }

        public ChestModel CreateChest()
        {
            var chestId = $"Chest_{_chestCounter++}";

            var chestModel = new ChestModel
            {
                Id = chestId,
                IsWinning = false
            };
            chestModel.ChangeState(ChestState.Closed);

            CreateChestUI(chestModel);

            Debug.Log($"Factory created chest: {chestId}");
            return chestModel;
        }

        public List<ChestModel> CreateChests(int chestsPerRound)
        {
            var chests = new List<ChestModel>(chestsPerRound);
            int winningIndex = Random.Range(0, chestsPerRound); 
    
            for (int i = 0; i < chestsPerRound; i++) 
            {
                var chest = CreateChest();
                chest.IsWinning = (i == winningIndex); 
                chests.Add(chest);
            }
    
            return chests;
        }

        public ChestModel CreateChest(bool isWinning)
        {
            var chest = CreateChest();
            chest.IsWinning = isWinning;
            return chest;
        }

        private void CreateChestUI(ChestModel model)
        {
            if (_chestPrefab == null || _chestContainer == null)
                return;

            var chestGO = UnityEngine.Object.Instantiate(_chestPrefab, _chestContainer);
            var chestButton = chestGO.GetComponent<ChestView>();

            if (chestButton != null)
            {
                chestButton.Initialize(model, _openingManager);
            }
        }

        public void ClearAllChests()
        {
            _chestCounter = 0;

            if (_chestContainer != null)
            {
                foreach (Transform child in _chestContainer)
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }

            Debug.Log("Factory cleared all chests");
        }
    }
}