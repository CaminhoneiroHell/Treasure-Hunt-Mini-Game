using System.Collections.Generic;
using UnityEngine;

namespace TreasureHuntMiniGame
{
    [System.Serializable]
    public class CollectableEntries
    {
        public GameObject collectableEntryView;
        public string collectableEntryName;
        public int count;
    }

    [CreateAssetMenu(fileName = "GameConfig", menuName = "Treasure Hunt/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Game Settings")] [Tooltip("Number of chests per round ")] [SerializeField]
        private int _chestsPerRound = 3;

        [Tooltip("Chest opening duration in seconds")] [SerializeField]
        private float _chestOpeningDuration = 2f;

        [Tooltip("Maximum attempts per round")] [SerializeField]
        private int maxAttempts = 2;

        [Header("Message Display References")] 
        public string victoryMessage = "You Found the Treasure! You Win!!";
        public string gameOverMessage = "GameOver! Out of Attempts";

        [Header("Collectables References")] 
        public List<CollectableEntries> collectableEntriesList = new List<CollectableEntries>();

        public int ChestsPerRound => _chestsPerRound;
        public float ChestOpeningDuration => _chestOpeningDuration;
        public int MaxAttempts => maxAttempts;
    }
}