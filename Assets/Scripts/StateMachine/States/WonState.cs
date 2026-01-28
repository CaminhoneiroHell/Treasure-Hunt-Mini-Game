using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHuntMiniGame.States
{

    public class WonState : BaseGameState
    {
        private Button _startButton;
        private ChestFactory _chestFactory;
        private HUDDisplayView _hudDisplayView;
        private GameConfig _gameConfig;
        private PlayerCollectableData _playerCollectableData;
    
        public override GameState State => GameState.Won;
    
        public WonState(GameStateMachine stateMachine, Button startButton, ChestFactory chestFactory,
            HUDDisplayView hudDisplay, GameConfig config,PlayerCollectableData collectableData) : base(stateMachine)
        {
            _startButton = startButton;
            _chestFactory = chestFactory;
            _hudDisplayView = hudDisplay;
            _gameConfig = config;
            _playerCollectableData = collectableData;
        }
    
        public override async UniTask Enter()
        {
            Debug.Log("Entering Won State");

            _hudDisplayView.UpdateContextHUD(GameMessageType.Victory);
        
            _startButton.gameObject.SetActive(true);
        
            var pressed = false;
            _startButton.onClick.RemoveAllListeners();
            _startButton.onClick.AddListener(() => pressed = true);

            OnCollectableCollected(_gameConfig.collectableEntriesList[0]);
        
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
        
        public void OnCollectableCollected(CollectableEntries collectedItem)
        {
            if (collectedItem != null)
            {
                _playerCollectableData.AddCollectable(collectedItem);
            }
        }
    }
}