using Cysharp.Threading.Tasks;

namespace TreasureHuntMiniGame.States
{
    public enum GameState
    {
        Lobby,
        Playing,
        GameOver,
        Won
    }

    public interface IGameState
    {
        GameState State { get; }
        UniTask Enter();
        UniTask Execute();
        UniTask Exit();
        UniTask OnChestOpened(bool wasWinning);
    }

    public abstract class BaseGameState : IGameState
    {
        public abstract GameState State { get; }
        protected GameStateMachine StateMachine { get; }

        protected BaseGameState(GameStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual UniTask Enter() => UniTask.CompletedTask;
        public virtual UniTask Execute() => UniTask.CompletedTask;
        public virtual UniTask Exit() => UniTask.CompletedTask;
        public virtual UniTask OnChestOpened(bool wasWinning) => UniTask.CompletedTask;
    }
}