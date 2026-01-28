using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TreasureHuntMiniGame
{
    [System.Serializable]
    public class PlayerData
    {
        public List<CollectableEntries> CollectedItems = new List<CollectableEntries>();
    }
    
    public class PlayerCollectableData : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        
        [Header("Collectables References")] 
        [SerializeField] private Transform contentHUD;
        [SerializeField] private GameObject collectableEntryView;
        
        private string _savePath;
    
        private void Awake()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
            EraseSaveData();
        }
    
        public void AddCollectable(CollectableEntries collectableName)
        {
            playerData.CollectedItems.Add(collectableName);
            SaveData();
        }
    
        public List<CollectableEntries> GetCollectedItems()
        {
            return new List<CollectableEntries>(playerData.CollectedItems);
        }
    
        private void LoadData()
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                playerData = JsonUtility.FromJson<PlayerData>(json);
            }
            else
            {
                playerData = new PlayerData();
            }
        }
    
        private void SaveData()
        {
            string json = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(_savePath, json);
        }
        
        public void EraseSaveData()
        {
            if (File.Exists(_savePath))
            {
                File.Delete(_savePath);
            }
    
            // Reset to fresh PlayerData
            playerData = new PlayerData();
    
            // Optional: Immediately save the empty data to ensure clean state
            SaveData();
        }
    }
}