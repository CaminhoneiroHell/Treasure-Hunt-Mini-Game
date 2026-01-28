🏴‍☠️ <H1>Treasure Hunt Mini-Game</H1>

🎮 <H2>Game Overview</H2>

Treasure Hunt is a mini-game where players search for hidden treasure chests with limited attempts. Each round presents multiple chests, only one of which contains treasure.
    
Core Features

    🎲 Random Treasure: One winning chest per round

    ⏱️ Timed Opening: Real-time chest opening with cancellation

    🏆 Persistent Rewards: Collect coins, gems, and add custom rewards

    🔧 Configurable Gameplay: Adjust difficulty via ScriptableObjects

    🔄 State Management: State machine architecture

    💾 Auto-save(Not finished): Player progress saved automatically

🎮 Instalation

    Clone the repository
    bash

    git clone https://github.com/CaminhoneiroHell/Treasure-Hunt-Mini-Game.git
    cd treasure-hunt
    
⚙️ Configuration
Game Config ScriptableObject

Create and customize your game settings:

    Create Config Asset
    text

    Right-click in Project window → Create → Treasure Hunt → Game Config

    Configure Settings
    csharp

    // Example configuration
    Chests Per Round: 5      // Number of chests displayed
    Opening Duration: 2.5s   // Time to open a chest
    Max Attempts: 3         // Attempts per round
    Victory Message: "You found the treasure!"
    Game Over Message: "Out of attempts!"

    Add Collectables

        Click "+" in Collectable Entries list

        Assign UI prefab

        Set name (e.g., "Coins", "Gems")

        Set default count
    
Adding New Collectables

    Add entry to GameConfig.collectableEntriesList

    Create a UI prefab with the CollectableView component

    The system automatically picks it up for rewards

Adding New Game States

    Create a class inheriting from BaseGameState

    Add to GameStateMachine.InitializeStates()

    Implement state-specific logic


<h1>Core Architecture Pattern</h1>

<img width="1051" height="976" alt="image" src="https://github.com/user-attachments/assets/be40eb75-307b-4609-a661-df2d5340b95c" />



**Data Flow**

    Game Initialization → GameInitializer sets up GameStateMachine

    State Transitions → States control UI, chest creation, and game logic

    Chest Interaction → ChestOpeningTask manages async chest opening

    Reward System → Collectables awarded via PlayingState.AwardRandomReward()

    Persistence → PlayerCollectableData saves to JSON file


⚠️ **Known Limitations, Future Improvements & Trade-Offs**

    Current Limitations:

        No chest animation system (only color changes)

        Simple random reward system

        No sound effects implementation

        Basic UI with minimal polish

    Potential Improvements:

        Add particle effects for winning chests
        
        Add animations while opening chests

        Implement an audio system

        Add cloud save support

        Implement analytics tracking

🐛 **Troubleshooting**

Issue: Chests don't open

    Check that ChestOpeningTask has GameConfig assigned

    Verify GameStateMachine is in the Playing state

Issue: Collectables not saving

    Check Won State
    BUG FIX REQUIRED: Does not contain the amount of coins to persist here; it requires adding the amount in CollectableEntries.count, must create a temp copy from selectedCollectable, update the amount, and add as a parameter at SaveCollectableCollected, and delete it after using here to save
                
Issue: UI not updating

    Ensure HUDDisplayView has all required references

    Check that the CollectableView prefab has TMP_Text assigned

📊 Current Architecture Assessment:

MVC Implementation Status:

Component	| Current Role	MVC Classification	          | Issues
**ChestModel**	Data structure for chest	Model         |✅	Good
**PlayerData**	Player collectables data	Model         |✅	Good
**ChestView**	Visual chest representation	View          |✅	Good
**CollectableView**	Collectable UI element	View        |✅	Good
**HUDDisplayView**	Main UI controller	View/Controller |⚠️	Violates SRP
**GameStateMachine**	Game flow manager	Controller      |⚠️	Too many responsibilities
**PlayingState**	Gameplay logic	Controller            | ⚠️	Business logic mixed with UI

SOLID Principles Violations
🔴 1. Single Responsibility Principle (SRP) - Major Issues
Violation Examples:
A. HUDDisplayView.cs 

    public void UpdateUserCollectablesHUD() { /* Creates UI, manages cache */ }
    public void UpdateAttemptsDisplay() { /* Updates attempts UI */ }
    public void UpdateContextHUD() { /* Updates messages */ }
    public CollectableView GetCollectableView() { /* Cache management */ }
    
4+ responsibilities in one class

B. PlayingState - Game Logic + UI + Reward Logic

        // ❌ Mixed: Game rules, UI, rewards, state transitions
        AwardRandomReward() method Should be a separate service
        
C. ChestFactory - Creation + UI Instantiation

🔴 2. Open/Closed Principle (OCP) - Limited Extensibility
Violation Examples:
A. Hardcoded Game States

    private void InitializeStates()
    {
        // ❌ Closed for extension - adding new states requires modifying this class
        _states[GameState.Lobby] = new LobbyState(...);
        _states[GameState.Playing] = new PlayingState(...);
        _states[GameState.GameOver] = new GameOverState(...);
        _states[GameState.Won] = new WonState(...);
    }

🔴 3. Liskov Substitution Principle (LSP) - Mostly Good

✅ States can be substituted via IGameState interface
✅ Base classes provide default implementations

🔴 4. Interface Segregation Principle (ISP) - Issues
Violation Example:
A. IGameState Interface Too Broad

    public interface IGameState
    {
        GameState State { get; }
        UniTask Enter();
        UniTask Execute();
        UniTask Exit();
        UniTask OnChestOpened(bool wasWinning); // ❌ Not all states need this
    }

🔴 5. Dependency Inversion Principle (DIP) - Major Issues
Violation Examples:
A. High-Level Modules Depend on Low-Level Details

PlayingState has a direct dependency on concrete implementations 
    ❌ Too many concrete dependencies
    ❌ Violates Dependency Inversion


🛠️ Improvements Proposal
  1. Create Separate Services
  2. Extract UI Management in dedicated UI Controllers, ex:IUIManager and allow a Single responsibility while updating UI elements
  3. Apply Dependency Injection (DIP) Create Service Locator or a DI Container (Zenject)
  4. Complete MVC Architecture.

  The most significant missing piece is a pure Game Model that's completely separate from the state machine and UI. 
Currently, game state is scattered across multiple classes instead of being centralized in a single observable model that Views can subscribe to.

📝 License & Credits

This mini-game demonstrates common Unity patterns suitable for learning and extension. Feel free to modify for your own projects!
