using UnityEngine;

namespace TreasureHuntMiniGame
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Treasure Hunt/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Game Settings")] [Tooltip("Number of chests per round ")] [SerializeField]
        private int _chestsPerRound = 3;

        [Tooltip("Chest opening duration in seconds")] [SerializeField]
        private float _chestOpeningDuration = 2f;

        [Tooltip("Maximum attempts per round")] [SerializeField]
        private int maxAttempts = 2;

        public int ChestsPerRound => _chestsPerRound;
        public float ChestOpeningDuration => _chestOpeningDuration;
        public int MaxAttempts => maxAttempts;
    }
}