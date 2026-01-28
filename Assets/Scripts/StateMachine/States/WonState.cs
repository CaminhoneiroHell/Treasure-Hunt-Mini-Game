
using Cysharp.Threading.Tasks;
using TreasureHuntMiniGame.Core;
using TreasureHuntMiniGame.Data;
using TreasureHuntMiniGame.Enums;
using TreasureHuntMiniGame.View;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHuntMiniGame.States
{

    public class WonState : BaseGameState
    {
        private Button _startButton;
        private ChestFactory _chestFactory;
        private HUDDisplayView _hudDisplayView;
        private GameConfig _gameConfig;
        private PlayerCollectableData _playerCollectableData;
    
        public override GameState State => GameState.Won;
    
        public WonState(GameStateMachine stateMachine, Button startButton, ChestFactory chestFactory,
            HUDDisplayView hudDisplay, GameConfig config,PlayerCollectableData collectableData) : base(stateMachine)
        {
            _startButton = startButton;
            _chestFactory = chestFactory;
            _hudDisplayView = hudDisplay;
            _gameConfig = config;
            _playerCollectableData = collectableData;
        }
    
        public override async UniTask Enter()
        {
            Debug.Log("Entering Won State");

            _hudDisplayView.UpdateContextHUD(GameMessageType.Victory);
        
            AwardRandomReward();
            
            _startButton.gameObject.SetActive(true);
        
            var pressed = false;
            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(() => pressed = true);

        
            await UniTask.WaitUntil(() => pressed);
        
            await StateMachine.ChangeState(GameState.Playing);
        }
    
        public override UniTask Exit()
        {
            _chestFactory.ClearAllChests();
            _startButton.gameObject.SetActive(false);
            _hudDisplayView.UpdateContextHUD(GameMessageType.Clear);
            return UniTask.CompletedTask;
        }
        
        
        
        public void AwardRandomReward()
        {
            if (_gameConfig == null || _gameConfig.collectableEntriesList.Count == 0)
            {
                Debug.LogError("GameConfig not set or no collectables defined");
                return;
            }
        
            // Randomly select a collectable
            int randomIndex = Random.Range(0, _gameConfig.collectableEntriesList.Count);
            var selectedCollectable = _gameConfig.collectableEntriesList[randomIndex];
        
            // Generate random quantity
            int randomQuantity = Random.Range(1, 100);
            
            var collectableView = _hudDisplayView.GetCollectableView(selectedCollectable.collectableEntryName);
        
            if (collectableView != null)
            {
                // Use IncrementAmount if you have it (more efficient)
                collectableView.IncrementAmount(randomQuantity);
            }
            else
            {
                Debug.LogWarning($"CollectableView not found for: {selectedCollectable.collectableEntryName}");
            }
        
            //BUG: Does not contain the amount of coins to persist here; it requires adding the amount in CollectableEntries.count, must create a temp copy from selectedCollectable, update the amount, and add as a parameter at SaveCollectableCollected, and delete it after using here to save
            OnCollectableCollected(selectedCollectable);
            
            Debug.Log($"Awarded: {randomQuantity}x {selectedCollectable.collectableEntryName}");
        }
        
        public void OnCollectableCollected(CollectableEntries collectedItem)
        {
            if (collectedItem != null)
            {
                _playerCollectableData.AddCollectable(collectedItem);
            }
        }
    }
}