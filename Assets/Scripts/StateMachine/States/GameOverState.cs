using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHuntMiniGame.States
{

    public class GameOverState : BaseGameState
    {
        private Button _startButton;
        private ChestFactory _chestFactory;
        private HUDDisplayView _hudDisplayView;
    
        public override GameState State => GameState.GameOver;
    
        public GameOverState(GameStateMachine stateMachine, Button startButton, ChestFactory chestFactory, HUDDisplayView hudDisplay) : base(stateMachine)
        {
            _startButton = startButton;
            _chestFactory = chestFactory;
            _hudDisplayView = hudDisplay;
        }
    
        public override async UniTask Enter()
        {
            Debug.Log("Entering Game Over State");
        
            _hudDisplayView.UpdateContextHUD(GameMessageType.GameOver);
        
            _startButton.gameObject.SetActive(true);
        
            var pressed = false;
            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(() => pressed = true);
        
            await UniTask.WaitUntil(() => pressed);
        
            await StateMachine.ChangeState(GameState.Playing);
        }
    
        public override UniTask Exit()
        {
            _chestFactory.ClearAllChests();
            _startButton.gameObject.SetActive(false);
            _hudDisplayView.UpdateContextHUD(GameMessageType.Clear);
            return UniTask.CompletedTask;
        }
    }
}