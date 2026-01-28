using TreasureHuntMiniGame.Data;
using TreasureHuntMiniGame.States;
using TreasureHuntMiniGame.View;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHuntMiniGame.Core
{

    public class GameStarter : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private PlayerCollectableData playerCollectableCollectableData;

    
        [Header("UI References")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Transform _chestContainer;
        [SerializeField] private GameObject _chestPrefab;
        [SerializeField] private ChestOpeningTask _chestOpenTask;
        [SerializeField] private HUDDisplayView _hudDisplayView;
    
        private GameStateMachine _stateMachine;
        private void Start()
        {
            if (_gameConfig == null)
            {
                Debug.LogError("GameConfig not assigned!");
                return;
            }
        
            Debug.Log("=== Game Starting ===");
            Debug.Log($"Chests per round: {_gameConfig.ChestsPerRound}");
            Debug.Log($"Opening duration: {_gameConfig.ChestOpeningDuration}s");
            Debug.Log($"Max attempts: {_gameConfig.MaxAttempts}");
            Debug.Log("=====================");
        
            InitializeStateMachine();
        }
    
        private void InitializeStateMachine()
        {
            _stateMachine = gameObject.AddComponent<GameStateMachine>();
        
            GameStateMachine stateMachineComponent = _stateMachine;
            stateMachineComponent.SetPlayerCollectablesData(playerCollectableCollectableData);
            stateMachineComponent.SetConfig(_gameConfig);
            stateMachineComponent.SetStartButton(_startButton);
            stateMachineComponent.SetHUDDisplayMessages(_hudDisplayView);
            stateMachineComponent.SetChestFactory(new ChestFactory(_chestPrefab, _chestContainer,_chestOpenTask));
            _chestOpenTask.SetGameStateMachine(stateMachineComponent);
        }
    }
}