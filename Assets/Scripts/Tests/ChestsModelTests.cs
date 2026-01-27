// using System.Collections.Generic;
// using System.Threading;
// using QFSW.QC;
// using UnityEngine;
//
// namespace TreasureHuntMiniGame.Tests
// {
//     public class ChestsModelTests: MonoBehaviour
// {
//     public ChestOpeningTask task;
//     public List<ChestModel> chests = new List<ChestModel>();
//     
//     private void Start()
//     {
//         // Initialize chests
//         for (int i = 0; i < 5; i++)
//         {
//             var chestModel = new ChestModel
//             {
//                 Id = $"Chest_{i++}",
//                 IsWinning = false
//             };
//             
//             chests.Add(chestModel);
//         }
//         
//         Debug.Log("it was created: " + chests.Co+unt + " Chest models");
//     }
//
//     [ContextMenu("Test Chest Opening Sequence")]
//     [Command]
//     private async void ChestStartTest()
//     {
//         Debug.Log("=== Starting Chest Opening Tests ===");
//         
//         // Test 1: Open first chest
//         Debug.Log("Test 1: Opening first chest...");
//         var result1 = await task.OpenChestAsync(chests[0], 5f);
//         Debug.Log($"Result: {result1}");
//         
//         // Test 2: Open winning chest
//         Debug.Log("Test 2: Opening winning chest...");
//         var result2 = await task.OpenChestAsync(chests[2], 3f);
//         Debug.Log($"Result: {result2}");
//         
//         // Test 3: Cancel after delay
//         Debug.Log("Test 3: Opening with cancellation after 0.5s...");
//         var cts = new CancellationTokenSource();
//         cts.CancelAfter(500);
//         var result3 = await task.OpenChestAsync(chests[3], 3f, cts.Token);
//         Debug.Log($"Result: {result3}");
//     }
//
//     // Simulate UI button click for chest selection
//     [Command]
//     public async void OnChestSelected(int chestIndex)
//     {
//         if (chestIndex < 0 || chestIndex >= chests.Count)
//         {
//             Debug.LogError($"Invalid chest index: {chestIndex}");
//             return;
//         }
//         
//         Debug.Log($"Selected chest {chestIndex}");
//         
//         // This will automatically cancel any currently opening chest
//         var result = await task.OpenChestAsync(chests[chestIndex], 2f);
//         
//         if (result == OpenChestResult.Success)
//         {
//             Debug.Log($"Chest {chestIndex} contained the treasure!");
//         }
//         else if (result == OpenChestResult.Failure)
//         {
//             Debug.Log($"Chest {chestIndex} was empty.");
//         }
//         else if (result == OpenChestResult.Cancelled)
//         {
//             Debug.Log($"Opening chest {chestIndex} was cancelled.");
//         }
//     }
// }
// }