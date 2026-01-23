
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using QFSW.QC;

namespace Tests
{
    public class ChestTester : MonoBehaviour
    {
        public ChestOpeningService service;
        public ChestModel testChest = new ChestModel("01", false);
    
        [Command]
        private async void ChestStartTest()
        {
            Debug.Log("Opening chest...");
            var result = await service.OpenChestAsync(testChest, 5f);
            Debug.Log($"Result: {result}");
        
            Debug.Log("Opening winning chest...");
            testChest.SetWinning(true);
            result = await service.OpenChestAsync(testChest, 3f);
            Debug.Log($"Result: {result}");
        
            Debug.Log("Opening and cancelling chest...");
            var cts = new CancellationTokenSource();
            cts.CancelAfter(500);
            result = await service.OpenChestAsync(testChest, 3f, cts.Token);
            Debug.Log($"Result: {result}");
            
        }
        
    }
}