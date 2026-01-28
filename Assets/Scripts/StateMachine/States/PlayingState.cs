using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHuntMiniGame.States
{

    public class PlayingState : BaseGameState
    {
        private GameConfig _config;
        private Button _startButton;
        private int _attempts = 0;
        private int _winningChestsFound = 0;
        private HUDDisplayView _hudDisplayView;
        
        private ChestFactory _chestFactory;
        public override GameState State => GameState.Playing;
        
        public PlayingState(GameStateMachine stateMachine, GameConfig config, 
            Button startButton, ChestFactory chestFactory, HUDDisplayView hudDisplay) : base(stateMachine)
        {
            _config = config;
            _startButton = startButton;
            _chestFactory = chestFactory;
            _hudDisplayView = hudDisplay;
        }
        
        public override async UniTask Enter()
        {
            Debug.Log("Entering Playing State");
            Debug.Log($"Starting game with {_config.ChestsPerRound} chests");
            
            // Reset game state
            _attempts = _config.MaxAttempts;
            _winningChestsFound = 0;
            
            CreateChests();
            RenderAttemptsText();
            
            // Start game loop
            await Execute();
        }


        public override async UniTask Execute()
        {
            // Game loop runs until win or game over
            while (true)
            {
                await UniTask.Yield();
                
            }
        }
        
        public override UniTask Exit()
        {
            Debug.Log("Exiting Playing State");
            return UniTask.CompletedTask;
        }
        
        private void CreateChests()
        {
            _chestFactory.CreateChests(_config.ChestsPerRound);
            Debug.Log($"Created {_config.ChestsPerRound} chests");
        }
        
        private void RenderAttemptsText()
        {
            _hudDisplayView.UpdateAttemptsDisplay(_attempts, _config.MaxAttempts);
        }
        
        public async UniTaskVoid OnChestOpened(bool wasWinning)
        {
            _attempts--;
            RenderAttemptsText();

            if (wasWinning)
            {
                _winningChestsFound++;
                
                AwardRandomReward();
            }


            if (_winningChestsFound > 0)
            {
                Debug.Log($"WON THE GAME!");
                await StateMachine.ChangeState(GameState.Won);
            }
        
            if (_attempts <= 0)
            {
                Debug.Log($"LOST THE GAME!");
                await StateMachine.ChangeState(GameState.GameOver);
            }
    
            Debug.Log($"Attempts used: {_attempts}/{_config.MaxAttempts}");
        }
        
        
        
        public void AwardRandomReward()
        {
            if (_config == null || _config.collectableEntriesList.Count == 0)
            {
                Debug.LogError("GameConfig not set or no collectables defined");
                return;
            }
        
            // Randomly select a collectable
            int randomIndex = Random.Range(0, _config.collectableEntriesList.Count);
            var selectedCollectable = _config.collectableEntriesList[randomIndex];
        
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
        
            Debug.Log($"Awarded: {randomQuantity}x {selectedCollectable.collectableEntryName}");
        }

    }
}