using System.Collections.Generic;
using TreasureHuntMiniGame.Data;

namespace TreasureHuntMiniGame.Model
{
    [System.Serializable]
    public class PlayerData
    {
        public List<CollectableEntries> CollectedItems = new List<CollectableEntries>();
    }

}