
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TreasureHuntMiniGame.States;
using UnityEngine;

namespace TreasureHuntMiniGame
{
    public enum OpenChestResult
    {
        Success,
        Failure,
        Cancelled
    }
    public enum ChestState
    {
        Closed,
        Opening,
        Opened,
        Winning
    }

public class ChestOpeningTask : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    
    // private CancellationTokenSource _currentOpeningCTS;
    private CancellationTokenSource _globalCancellationTokenSource;
    private ChestModel _currentlyOpeningChest;
    private GameStateMachine _stateMachine;
    
    public void SetGameStateMachine(GameStateMachine config) => _stateMachine = config;
    
    private void Awake()
    {
        _globalCancellationTokenSource = new CancellationTokenSource();
    }
    private void Start()
    {
        _stateMachine = FindObjectOfType<GameStateMachine>();
        
        
        if (_gameConfig == null)
        {
            Debug.LogError("GameConfig not assigned!");
            return;
        }
    }
    
    public async UniTask<OpenChestResult> OpenChestAsync(
        ChestModel chest, 
        CancellationToken externalToken = default)
    {
        if (_stateMachine?.GetCurrentState() != GameState.Playing)
        {
            Debug.Log("Can't open chest - not in Playing state");
            return OpenChestResult.Cancelled;
        }
        
        float duration = _gameConfig.ChestOpeningDuration;
        
        CancelCurrentOpening();
        
        // Create linked cancellation token
        using var linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(
            externalToken,
            _globalCancellationTokenSource.Token
        );
        
        var token = linkedCTS.Token;
        // _currentOpeningCTS = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
        // _currentlyOpeningChest = chest;
        
        try
        {
            chest.SetState(ChestState.Opening);
            Debug.Log($"Started opening chest: {chest.Id} for {duration}s");
            
            await UniTask.Delay(
                TimeSpan.FromSeconds(duration), 
                // cancellationToken: _currentOpeningCTS.Token
                cancellationToken: token
            );
            
            // Check if cancelled during delay
            if (token.IsCancellationRequested)
            {
                chest.ChangeState(ChestState.Closed);
                return OpenChestResult.Cancelled;
            }
            
            var result = chest.IsWinning ? 
                OpenChestResult.Success : 
                OpenChestResult.Failure;
            
            // chest.SetState(chest.IsWinning ? ChestState.Winning : ChestState.Opened);
            chest.ChangeState(chest.IsWinning ? ChestState.Winning : ChestState.Opened);
            Debug.Log($"Finished opening chest: {chest.Id}, Result: {result}");
            
            // Notify state machine
            _stateMachine.OnChestOpened(chest.IsWinning);
            
            return result;
        }
        catch (OperationCanceledException)
        {
            // if (_currentlyOpeningChest == chest)
            // {
            //     chest.SetState(ChestState.Closed);
            //     Debug.Log($"Cancelled opening chest: {chest.Id}");
            // }
            //
            // return OpenChestResult.Cancelled;
            
            chest.ChangeState(ChestState.Closed);
            Debug.Log($"Cancelled opening chest: {chest.Id}");
            return OpenChestResult.Cancelled;
        }
        // finally
        // {
        //     Cleanup();
        // }
    }
    
    // private void Cleanup()
    // {
    //     _currentOpeningCTS?.Dispose();
    //     _currentOpeningCTS = null;
    //     _currentlyOpeningChest = null;
    // }

    public void CancelCurrentOpening()
    {
        _globalCancellationTokenSource?.Cancel();
        // Create new CTS for future operations
        _globalCancellationTokenSource?.Dispose();
        _globalCancellationTokenSource = new CancellationTokenSource();
    }
    
    private void OnDestroy()
    {
        _globalCancellationTokenSource?.Cancel();
        _globalCancellationTokenSource?.Dispose();
    }
}

}