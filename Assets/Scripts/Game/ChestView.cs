using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHuntMiniGame
{

    public class ChestView : MonoBehaviour
    {
        [SerializeField] private Button chestButton;
        [SerializeField] private Image chestIcon;
        // [SerializeField] private Text chestIdText;
        // [SerializeField] private Animator buttonAnimator;
        
        [Header("Colors")]
        [SerializeField] private Color closedColor = Color.yellow;
        [SerializeField] private Color openingColor = Color.blue;
        [SerializeField] private Color openedColor = Color.gray;
        [SerializeField] private Color winningColor = Color.green;
        
        private ChestModel _chestModel;
        private ChestOpeningTask _openingManager;
        private bool _isOpening = false;
        
        public event Action OnOpenWinningChest;
        
        
        public void Initialize(ChestModel model, ChestOpeningTask manager)
        {
            _chestModel = model;
            _openingManager = manager;
            
            // Setup UI
            // chestIdText.text = model.Id;
            UpdateVisuals();
            
            chestButton.onClick.RemoveAllListeners();
            chestButton.onClick.AddListener(OnChestClicked);
            
            model.OnStateChanged += OnChestStateChanged;
        }
        
        private void OnChestClicked()
        {
            if (_isOpening) return;
            
            StartChestOpening().Forget();
        }
        
        private async UniTaskVoid StartChestOpening()
        {
            _isOpening = true;
            
            try
            {
                var result = await _openingManager.OpenChestAsync(
                    _chestModel
                );
                
                switch (result)
                {
                    case OpenChestResult.Success:
                        _chestModel.IsWinning = true;
                        Debug.Log($"{_chestModel.Id} WON!");
                        break;
                        
                    case OpenChestResult.Failure:
                        Debug.Log($"{_chestModel.Id} didn't win");
                        break;
                        
                    case OpenChestResult.Cancelled:
                        Debug.Log($"{_chestModel.Id} opening was cancelled");
                        break;
                }
            }
            finally
            {
                _isOpening = false;
            }
        }
        
        private void OnChestStateChanged(ChestState newState)
        {
            UpdateVisuals();
            
            switch (newState)
            {
                case ChestState.Opening:
                    // Play opening animation
                    break;
                    
                case ChestState.Winning:
                    // Optional: Play winning animation
                    break;
            }
        }
        
        private void UpdateVisuals()
        {
            switch (_chestModel.State)
            {
                case ChestState.Closed:
                    chestIcon.color = closedColor;
                    chestButton.interactable = true;
                    break;
                    
                case ChestState.Opening:
                    chestIcon.color = openingColor;
                    chestButton.interactable = false; 
                    break;
                    
                case ChestState.Opened:
                    chestIcon.color = openedColor;
                    chestButton.interactable = false;
                    break;
                    
                case ChestState.Winning:
                    chestIcon.color = winningColor;
                    chestButton.interactable = false; 
                    break;
            }
        }
        
        private void OnDestroy()
        {
            if (_chestModel != null)
            {
                _chestModel.OnStateChanged -= OnChestStateChanged;
            }
        }
    }
}