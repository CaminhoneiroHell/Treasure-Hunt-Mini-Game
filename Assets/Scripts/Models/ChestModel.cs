using System;
using TreasureHuntMiniGame.Enums;

namespace TreasureHuntMiniGame.Model
{
    public class ChestModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ChestState State { get; private set; }
        public bool IsWinning { get; set; }
        
        public event Action<ChestState> OnStateChanged;
        
        public void ChangeState(ChestState newState)
        {
            if (State == newState) return;
            State = newState;
            OnStateChanged?.Invoke(newState);   
        }
    }
}
