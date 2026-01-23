using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using QFSW.QC;
using UnityEngine;

// ChestOpeningService.cs
public class ChestOpeningService : MonoBehaviour
{
    private CancellationTokenSource _currentOpeningCTS;
    private ChestModel _currentlyOpeningChest;

    
    public async UniTask<OpenChestResult> OpenChestAsync(
        ChestModel chest, 
        float duration, 
        CancellationToken externalToken = default)
    {
        // Cancel any currently opening chest before starting new one
        CancelCurrentOpening();
        
        _currentOpeningCTS = CancellationTokenSource.CreateLinkedTokenSource(externalToken);
        _currentlyOpeningChest = chest;
        
        try
        {
            chest.SetState(ChestState.Opening);
            Debug.Log($"Started opening chest: {chest.Id}");
            
            await UniTask.Delay(
                TimeSpan.FromSeconds(duration), 
                cancellationToken: _currentOpeningCTS.Token
            );
            
            // If we get here, opening completed
            var result = chest.IsWinning ? 
                OpenChestResult.Success : 
                OpenChestResult.Failure;
            
            chest.SetState(chest.IsWinning ? ChestState.Winning : ChestState.Opened);
            Debug.Log($"Finished opening chest: {chest.Id}, Result: {result}");
            
            return result;
        }
        catch (OperationCanceledException)
        {

            if (_currentlyOpeningChest == chest)
            {
                chest.SetState(ChestState.Closed);
                Debug.Log($"Cancelled opening chest: {chest.Id}");
            }
            
            return OpenChestResult.Cancelled;
        }
        finally
        {
            Cleanup();
        }
    }
    
    private void Cleanup()
    {
        _currentOpeningCTS?.Dispose();
        _currentOpeningCTS = null;
        _currentlyOpeningChest = null;
    }

    public void CancelCurrentOpening()
    {
        _currentOpeningCTS?.Cancel();
        Cleanup();
    }
    
}

public enum OpenChestResult
{
    Success,
    Failure,
    Cancelled
}