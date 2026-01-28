using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TreasureHuntMiniGame.Core;
using TreasureHuntMiniGame.Data;
using TreasureHuntMiniGame.View;
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
            
            await UniTask.Yield();
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
        
        public override async UniTask OnChestOpened(bool wasWinning)
        {
            if (wasWinning)
            {
                _winningChestsFound++;
                Debug.Log($"WON THE GAME!");
                await StateMachine.ChangeState(GameState.Won);
                return;
            }
            
            _attempts--; 
            RenderAttemptsText();
    
            if (_attempts <= 0)
            {
                Debug.Log($"LOST THE GAME!");
                await StateMachine.ChangeState(GameState.GameOver);
            }
        }
        
        

    }
}