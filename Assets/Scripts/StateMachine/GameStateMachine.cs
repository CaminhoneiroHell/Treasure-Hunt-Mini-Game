using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace TreasureHuntMiniGame.States
{


    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private PlayerCollectableData playerCollectableData;
        [SerializeField] private Button _startButton;
        [SerializeField] private ChestFactory _chestFactory;
        [SerializeField] private HUDDisplayView _hudDisplayView;
    
        private Dictionary<GameState, IGameState> _states = new Dictionary<GameState, IGameState>();
        private IGameState _currentState;
    
        private PlayingState _playingState; 

        public void SetPlayerCollectablesData(PlayerCollectableData collectableData) => playerCollectableData = collectableData;
        public void SetConfig(GameConfig config) => _gameConfig = config;
        public void SetStartButton(Button button) => _startButton = button;
        public void SetHUDDisplayMessages(HUDDisplayView hudDisplayView) => _hudDisplayView = hudDisplayView;
        public void SetChestFactory(ChestFactory chestFactory) => _chestFactory = chestFactory;
    
        private void Start()
        {
            InitializeStates();
            StartStateMachine().Forget();
        }

        private void InitializeStates()
        {
            _states[GameState.Lobby] = new LobbyState(this, _startButton , _gameConfig, _hudDisplayView);
        
            _playingState = new PlayingState(this, _gameConfig, _startButton, _chestFactory,_hudDisplayView);
            _states[GameState.Playing] = _playingState;
        
            _states[GameState.GameOver] = new GameOverState(this, _startButton, _chestFactory,_hudDisplayView);
            _states[GameState.Won] = new WonState(this, _startButton, _chestFactory, _hudDisplayView, _gameConfig, playerCollectableData);
        }
    
        private async UniTaskVoid StartStateMachine()
        {
            await ChangeState(GameState.Lobby);
            
        }
    
        public async UniTask ChangeState(GameState newState)
        {
            if (_states.TryGetValue(newState, out var nextState))
            {
                if (_currentState != null)
                {
                    await _currentState.Exit();
                }
            
                _currentState = nextState;
            
                await _currentState.Enter();
            }
            else
            {
                Debug.LogError($"State {newState} not found!");
            }
        }
    
        public void OnChestOpened(bool wasWinning)
        {
            if (_currentState?.State == GameState.Playing)
            {
                _ = _playingState.OnChestOpened(wasWinning);
            }
        }
    
        public GameState GetCurrentState() => _currentState?.State ?? GameState.Lobby;
    }
}