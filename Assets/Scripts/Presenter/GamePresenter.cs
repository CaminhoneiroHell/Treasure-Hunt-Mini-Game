using UnityEngine;

namespace Presenter
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private GameConfigSO _gameConfig;
        
        private StateMachine<GameState> _stateMachine;
        private GameRoundModel _currentRound;
        private PlayerInventoryModel _inventory;
        private ChestOpeningService _chestService;
        
        private void Awake()
        {
            // InitializeServices();
            // InitializeStateMachine();
        }
        
        // private void InitializeStateMachine()
        // {
        //     _stateMachine = new StateMachine<GameState>();
        //     
        //     _stateMachine.AddStateCallback(GameState.MainMenu, OnEnterMainMenu);
        //     _stateMachine.AddStateCallback(GameState.RoundStarting, OnEnterRoundStarting);
        //     _stateMachine.AddStateCallback(GameState.RoundActive, OnEnterRoundActive);
        //     _stateMachine.AddStateCallback(GameState.ChestOpening, OnEnterChestOpening);
        //     _stateMachine.AddStateCallback(GameState.RoundComplete, OnEnterRoundComplete);
        //     
        //     _stateMachine.ChangeState(GameState.MainMenu);
        // }
        
        private async void OnChestSelected(ChestModel chest)
        {
            if (_stateMachine.CurrentState != GameState.RoundActive) return;
            
            _stateMachine.ChangeState(GameState.ChestOpening);
            
            var result = await _chestService.OpenChestAsync(
                chest, 
                _gameConfig.ChestOpeningDuration
            );
            
            // HandleChestOpenResult(result, chest);
        }
        
        // private void HandleChestOpenResult(OpenChestResult result, ChestModel chest)
        // {
        //     switch (result)
        //     {
        //         case OpenChestResult.Success:
        //             GrantReward();
        //             _stateMachine.ChangeState(GameState.RoundComplete);
        //             break;
        //             
        //         case OpenChestResult.Failure:
        //             _currentRound.ConsumeAttempt();
        //             if (_currentRound.AttemptsRemaining > 0)
        //                 _stateMachine.ChangeState(GameState.RoundActive);
        //             else
        //                 _stateMachine.ChangeState(GameState.RoundComplete);
        //             break;
        //             
        //         case OpenChestResult.Cancelled:
        //             _stateMachine.ChangeState(GameState.RoundActive);
        //             break;
        //     }
        // }
    }
}