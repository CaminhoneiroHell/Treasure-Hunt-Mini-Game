// namespace Model
// {

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameRoundModel
    {
        private CancellationTokenSource _roundCTS;
    
        public async UniTask StartRoundAsync()
        {
            _roundCTS = new CancellationTokenSource();
        
        //     try
        //     {
        //         await InitializeChestsAsync(_roundCTS.Token);
        //         await StartRoundCountdownAsync(_roundCTS.Token);
        //     }
        //     catch (OperationCanceledException)
        //     {
        //         Debug.Log("Round was cancelled");
        //         Cleanup();
        //     }
        }
    
        public void CancelRound()
        {
            _roundCTS?.Cancel();
            _roundCTS?.Dispose();
            _roundCTS = null;
        }
    }
// }