using System.Collections.Generic;
using System.IO;
using QFSW.QC;
using TreasureHuntMiniGame.Model;
using UnityEngine;

namespace TreasureHuntMiniGame.Data
{

    public class PlayerCollectableData : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData = new PlayerData();
        
        [Header("Collectables References")] 
        [SerializeField] private Transform contentHUD;
        [SerializeField] private GameObject collectableEntryView;
        
        private string _savePath;
    
        private void Awake()
        {
            _savePath = Path.Combine(Application.persistentDataPath, "playerData.json");
            // LoadData();
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
        
        [Command]
        public void EraseSaveData()
        {
            if (File.Exists(_savePath))
            {
                File.Delete(_savePath);
            }
    
            playerData = new PlayerData();
    
            SaveData();
        }
    }
}