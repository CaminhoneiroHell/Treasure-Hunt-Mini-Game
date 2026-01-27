using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHuntMiniGame.States
{

    public class WonState : BaseGameState
    {
        private Button _startButton;
        private ChestFactory _chestFactory;
    
        public override GameState State => GameState.Won;
    
        public WonState(GameStateMachine stateMachine, Button startButton, ChestFactory chestFactory) : base(stateMachine)
        {
            _startButton = startButton;
            _chestFactory = chestFactory;
        }
    
        public override async UniTask Enter()
        {
            Debug.Log("Entering Won State");
        
            Debug.Log("YOU WON! Found all winning chests!");
        
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
            return UniTask.CompletedTask;
        }
    }
}