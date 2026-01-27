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
        private int _attemptsUsed = 0;
        private int _winningChestsFound = 0;
        private int _totalWinningChests = 0;
        
        private ChestFactory _chestFactory;
        public override GameState State => GameState.Playing;
        
        public PlayingState(GameStateMachine stateMachine, GameConfig config, 
            Button startButton, ChestFactory chestFactory) : base(stateMachine)
        {
            _config = config;
            _startButton = startButton;
            _chestFactory = chestFactory;
        }
        
        public override async UniTask Enter()
        {
            Debug.Log("Entering Playing State");
            Debug.Log($"Starting game with {_config.ChestsPerRound} chests");
            
            // Reset game state
            _attemptsUsed = 0;
            _winningChestsFound = 0;
            _totalWinningChests = 0;
            
            CreateChests();
            
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
        
        public async UniTaskVoid OnChestOpened(bool wasWinning)
        {
            _attemptsUsed++;

            if (wasWinning)
            {
                _winningChestsFound++;
            }

            if (_winningChestsFound > 0)
            {
                Debug.Log($"WON THE GAME!");
                await StateMachine.ChangeState(GameState.Won);
            }
        
            if (_attemptsUsed >= _config.MaxAttempts)
            {
                Debug.Log($"LOST THE GAME!");
                await StateMachine.ChangeState(GameState.GameOver);
            }
    
            Debug.Log($"Attempts used: {_attemptsUsed}/{_config.MaxAttempts}");
        }
    }
}