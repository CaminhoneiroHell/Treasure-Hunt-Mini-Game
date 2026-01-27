using System;

namespace TreasureHuntMiniGame
{
    public class ChestModel
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        public ChestState State { get; private set; }
        public bool IsWinning { get; set; }
        
        public event Action<ChestState> OnStateChanged;
        
        public void SetState(ChestState state)
        {
            State = state;
        }
        
        public void ChangeState(ChestState newState)
        {
            State = newState;
            OnStateChanged?.Invoke(newState);   
        }
    }
}