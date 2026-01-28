using System.Collections.Generic;
using TMPro;
using TreasureHuntMiniGame.Data;
using TreasureHuntMiniGame.Enums;
using UnityEngine;

namespace TreasureHuntMiniGame.View
{
    public class HUDDisplayView : MonoBehaviour
    {
        [Header("User Data Reference")] 
        
        [Header("HUD References")] 
        [SerializeField] private TMP_Text contextualMessages;
        [SerializeField] private TMP_Text attemptsEntry;
        private GameConfig config;
        
        
        [Header("Collectables References")] 
        [SerializeField] private Transform collectablesContentHUD;
        [SerializeField] private List<CollectableEntries> collectableEntriesList = new List<CollectableEntries>();

        
        private Dictionary<string, CollectableView> _collectableViewCache = new Dictionary<string, CollectableView>();

        public void SetConfig(GameConfig c) => config = c;
        
        public void UpdateContextHUD(GameMessageType messageType)
        {
            contextualMessages.text = messageType switch
            {
                GameMessageType.Victory => config.victoryMessage,
                GameMessageType.GameOver => config.gameOverMessage,
                GameMessageType.Clear => string.Empty,
                _ => string.Empty
            };
        }
        
        public void UpdateAttemptsDisplay(int current, int total)
        {
            attemptsEntry.text = $"Remaining Attempts: {current} / {total}";
        }

        public void UpdateUserCollectablesHUD()
        {
            // First clear existing entries
            ClearCollectableEntries();
    
            // Check if config exists and has collectables data
            if (config == null || config.collectableEntriesList == null)
            {
                Debug.LogWarning("Config or collectables list is null!");
                return;
            }
    
            foreach (CollectableEntries collectable in config.collectableEntriesList)
            {
                GameObject entryInstance = Instantiate(collectable.collectableEntryView, collectablesContentHUD);
                
                //TODO: Update parameters from persisted data later
                var collectableView = entryInstance.GetComponent<CollectableView>().InitializeCollectable(collectable.collectableEntryName, 
                    collectable.count);

                entryInstance.name = $"{collectable.collectableEntryName}";
                
                _collectableViewCache[collectable.collectableEntryName] = collectableView;
                
                // CollectableEntriesList.Add(collectable);
            }
        }
        
        public CollectableView GetCollectableView(string collectableName)
        {
            _collectableViewCache.TryGetValue(collectableName, out var collectableView);
            return collectableView;
        }


        private void ClearCollectableEntries()
        {
            foreach (Transform child in collectablesContentHUD)
            {
                Destroy(child.gameObject);
            }
        }
    }
}