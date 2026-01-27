// using System;
// using System.Collections.Generic;
// using QFSW.QC;
// using UnityEngine;
//
// namespace TreasureHuntMiniGame
// {
//
//     public enum ChestOperationResult
//     {
//         Success,
//         Failure,
//         Cancelled
//     }
//     
//     // Manager to create and handle multiple chests
//     public class TestSpawner : MonoBehaviour
//     {
//         [SerializeField] private GameObject chestButtonPrefab;
//         [SerializeField] private Transform chestButtonsContainer;
//         // [SerializeField] private Button addChestButton;
//         // [SerializeField] private Button cancelAllButton;
//         
//         // private ChestOpeningManager _openingManager;
//         private List<ChestModel> _chests = new List<ChestModel>();
//         private int _chestCounter = 0;
//         
//         private void Awake()
//         {
//             // Create the opening manager
//             // _openingManager = gameObject.AddComponent<ChestOpeningManager>();
//             
//             // Setup UI buttons
//             // addChestButton.onClick.AddListener(CreateNewChest);
//             // cancelAllButton.onClick.AddListener(CancelAllOpenings);
//         }
//         
//         [Command]
//         private void CreateNewChest()
//         {
//             var chestId = $"Chest_{_chestCounter++}";
//             
//             // Create model
//             var chestModel = new ChestModel
//             {
//                 Id = chestId,
//                 IsWinning = false
//             };
//             chestModel.ChangeState(ChestState.Closed);
//             
//             _chests.Add(chestModel);
//             
//             // Create UI button
//             var chestGO = Instantiate(chestButtonPrefab, chestButtonsContainer);
//             var chestButton = chestGO.GetComponent<ChestView>();
//             
//             if (chestButton != null)
//             {
//                 // chestButton.Initialize(chestModel, _openingManager);
//             }
//             
//             Debug.Log($"Created new chest: {chestId}");
//         }
//         
//         [Command]
//         private void CancelAllOpenings()
//         {
//             // _openingManager.CancelCurrentOpening();
//             Debug.Log("Cancelled all chest openings");
//         }
//     }
// }