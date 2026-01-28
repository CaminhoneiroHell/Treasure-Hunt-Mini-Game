
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TreasureHuntMiniGame.Data;
using TreasureHuntMiniGame.Enums;
using TreasureHuntMiniGame.Model;
using TreasureHuntMiniGame.States;
using UnityEngine;

namespace TreasureHuntMiniGame.Core
{
    
public class ChestOpeningTask : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    
    private CancellationTokenSource _activeOperationCTS;
    private ChestModel _currentlyOpeningChest;
    private GameStateMachine _stateMachine;
    
    public void SetGameStateMachine(GameStateMachine config) => _stateMachine = config;
    
    private void Awake()
    {
        _activeOperationCTS = new CancellationTokenSource();
    }
    private void Start()
    {
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
        
        using var linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(
            externalToken,
            _activeOperationCTS.Token
        );
        
        var token = linkedCTS.Token;
        
        try
        {
            chest.ChangeState(ChestState.Opening);
            Debug.Log($"Started opening chest: {chest.Id} for {duration}s");
            
            await UniTask.Delay(
                TimeSpan.FromSeconds(duration), 
                cancellationToken: token
            );
            
            if (token.IsCancellationRequested)
            {
                chest.ChangeState(ChestState.Closed);
                return OpenChestResult.Cancelled;
            }
            
            var result = chest.IsWinning ? 
                OpenChestResult.Success : 
                OpenChestResult.Failure;
            
            chest.ChangeState(chest.IsWinning ? ChestState.Winning : ChestState.Opened);
            Debug.Log($"Finished opening chest: {chest.Id}, Result: {result}");
            
            _stateMachine.OnChestOpened(chest.IsWinning);
            
            return result;
        }
        catch (OperationCanceledException)
        {
            
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
        _activeOperationCTS?.Cancel();
        // Create new CTS for future operations
        _activeOperationCTS?.Dispose();
        _activeOperationCTS = new CancellationTokenSource();
    }
    
    private void OnDestroy()
    {
        _activeOperationCTS?.Cancel();
        _activeOperationCTS?.Dispose();
    }
}

}