using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHuntMiniGame.States
{

    public class LobbyState : BaseGameState
    {
        private Button _startButton;
        private HUDDisplayView _hudDisplayView;
        private GameConfig _config;
    
        public override GameState State => GameState.Lobby;
    
        public LobbyState(GameStateMachine stateMachine, Button startButton, GameConfig config, 
            HUDDisplayView hudDisplay) : base(stateMachine)
        {
            _startButton = startButton;
            _hudDisplayView = hudDisplay;
            _config = config;
        }
    
        public override async UniTask Enter()
        {
            Debug.Log("Entering Lobby State");
            _hudDisplayView.UpdateContextHUD(GameMessageType.Clear);
        
            RenderUserCollectables();
            _startButton.gameObject.SetActive(true);
        
            var pressed = false;
            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(() => pressed = true);
            
            await UniTask.WaitUntil(() => pressed);
        
            await StateMachine.ChangeState(GameState.Playing);
        }
        
        public override UniTask Exit()
        {
            _startButton.gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
        
        private void RenderUserCollectables()
        {
            _hudDisplayView.UpdateUserCollectablesHUD();
        }
        
    }
}