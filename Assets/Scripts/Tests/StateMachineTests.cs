// using System;
// using System.Threading;
// // using CoreDomain.GameDomain.Services.Levels;
// using Cysharp.Threading.Tasks;
// using UnityEngine;
//
// namespace Tests
// {
//     public class StateMachineTests: MonoBehaviour
//     {
//         // private readonly ILevelsDataService _levelsDataService;
//         private void Start()
//         {
//             _ = InitEntryPoint(CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken));
//         }
//         
//         private async Awaitable InitEntryPoint(CancellationTokenSource cancellationTokenSource)
//         {
//             try
//             {
//                 UpdateApplicationSettings();
//                 // _loadingScreenController.Show();
//                 // InitializeServices();
//                 // _audioService.AddAudioClips(_coreAudioClipsScriptableObject);
//                 await LoadGameScene(cancellationTokenSource);
//                 // await _loadingScreenController.SetLoadingSlider(1, cancellationTokenSource);
//             }
//             catch (OperationCanceledException)
//             {
//                 Debug.Log("Operation init core was cancelled");
//             }
//             catch (Exception e)
//             {
//                 Debug.LogError(e.Message);
//                 throw;
//             }
//             
//             // _loadingScreenController.Hide();
//         }
//         
//         private void UpdateApplicationSettings()
//         {
//             Screen.sleepTimeout = SleepTimeout.NeverSleep;
//             Application.targetFrameRate = 60;
//         }
//         //
//         // private async UniTask LoadGameScene(CancellationTokenSource cancellationTokenSource)
//         // {
//         //     // await //_sceneLoaderService.TryLoadScene(SceneType.GameScene, new GameInitiatorEnterData(), cancellationTokenSource);
//         // }
//         
//         // private void InitializeServices()
//         // {
//         //     _gameInputActions.Enable();
//         //     _audioService.InitEntryPoint();
//         //     _sceneLoaderService.InitEntryPoint();
//         // }
//
//     }
//
//     internal interface ISceneLoaderService
//     {
//     }
// }