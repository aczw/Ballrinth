using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] [Min(1)] private int beginningStage;

    [Header("Systems")]
    [SerializeField] private Labyrinth labyrinth;
    public RunTimer timer;
    public Inventory inventory;

    [Header("Canvas Objects")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject intermission;
    [SerializeField] private GameObject outcome;
    [SerializeField] private GameObject timerDisplay;
    [SerializeField] private GameObject inventoryDisplay;

    /// <summary>
    ///     Per-frame state.
    /// </summary>
    private RotationInputBundle rotationInput;

    /// <summary>
    ///     Current run state.
    /// </summary>
    private RunState run;

    /// <summary>
    ///     Overall game state (tracked across runs).
    /// </summary>
    private GameState state;

    public static GameManager I { get; private set; }

    private void Awake() {
        if (I == null) {
            I = this;
        }
        else {
            Destroy(gameObject);
        }

        rotationInput = new RotationInputBundle();

        state = new GameState {
            maxStagesEscaped = 0,
            status = GameState.Status.MainMenu
        };

        run = new RunState {
            won = false,
            stage = beginningStage
        };
    }

    private void Update() {
        if (state.status != GameState.Status.InGame) return;

        if (timer.IsFinished()) {
            EndGame();
            return;
        }

        rotationInput = new RotationInputBundle {
            rotateUp = Keyboard.current.upArrowKey.isPressed,
            rotateDown = Keyboard.current.downArrowKey.isPressed,
            rotateLeft = Keyboard.current.leftArrowKey.isPressed,
            rotateRight = Keyboard.current.rightArrowKey.isPressed
        };

        if (Keyboard.current.zKey.wasPressedThisFrame) {
            inventory.TryUse(Inventory.Slot.One);
        }
        else if (Keyboard.current.xKey.wasPressedThisFrame) {
            inventory.TryUse(Inventory.Slot.Two);
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame) {
            inventory.TryUse(Inventory.Slot.Three);
        }
    }

    private void FixedUpdate() => labyrinth.ProcessRotation(rotationInput);

    public void StartGame() {
        if (state.status != GameState.Status.MainMenu) {
            Debug.LogError($"Incorrect game state: should be on the main menu, instead we're on {state.status}");
            return;
        }

        state.status = GameState.Status.Transitioning;

        // Clear main menu UI
        mainMenu.SetActive(false);

        // Set up first labyrinth
        labyrinth.Generate(run.stage + 1, run.stage + 1);
        timer.Restart();
        timerDisplay.SetActive(true);
        inventoryDisplay.SetActive(true);

        state.status = GameState.Status.InGame;
    }

    public void EscapeStage() {
        if (state.status != GameState.Status.InGame) {
            Debug.LogError($"Incorrect game state: should be in-game, instead we're on {state.status}!");
            return;
        }

        state.status = GameState.Status.Transitioning;

        // End current labyrinth run
        timer.Pause();
        labyrinth.Clear();

        // Set up intermission
        intermission.SetActive(true);

        state.status = GameState.Status.Intermission;
    }

    public void EnterNextStage() {
        if (state.status != GameState.Status.Intermission) {
            Debug.LogError($"Incorrect game state: should be in the intermission, instead we're on {state.status}!");
            return;
        }

        state.status = GameState.Status.Transitioning;

        // Clear intermission UI
        intermission.SetActive(false);

        // Set up next stage
        run.stage++;
        labyrinth.Generate(run.stage + 1, run.stage + 1);
        timer.Resume();

        state.status = GameState.Status.InGame;
    }

    public void EndGame() {
        if (state.status != GameState.Status.InGame) {
            Debug.LogError($"Incorrect game state: should be in-game, instead we're on {state.status}!");
            return;
        }

        state.status = GameState.Status.Transitioning;

        // Game can end before we run out of time
        if (!timer.IsFinished()) timer.Pause();

        // End current labyrinth run and wrap up any final game state
        labyrinth.Clear();
        timerDisplay.SetActive(false);
        inventoryDisplay.SetActive(false);

        if (run.stage >= state.maxStagesEscaped) {
            run.won = true;
            state.maxStagesEscaped = run.stage;
        }

        state.status = GameState.Status.End;

        // Set up final outcome screen
        outcome.SetActive(true);
    }

    public void RestartGame() {
        if (state.status != GameState.Status.End) {
            Debug.LogError($"Incorrect game state: should be at the end, instead we're on {state.status}!");
            return;
        }

        state.status = GameState.Status.Transitioning;

        // Clear outcome UI and reset all state as needed
        outcome.SetActive(false);
        rotationInput = new RotationInputBundle();
        run.won = false;
        run.stage = beginningStage;

        // Launch the labyrinth again
        labyrinth.Generate(run.stage + 1, run.stage + 1);
        timer.Restart();
        inventory.Clear();
        timerDisplay.SetActive(true);
        inventoryDisplay.SetActive(true);

        state.status = GameState.Status.InGame;
    }

    public void ReturnToMainMenu() {
        if (state.status != GameState.Status.End) {
            Debug.LogError($"Incorrect game state: should be at the end, instead we're on {state.status}!");
            return;
        }

        state.status = GameState.Status.Transitioning;

        // Clear outcome UI and reset all state as needed
        outcome.SetActive(false);
        rotationInput = new RotationInputBundle();
        inventory.Clear();
        run.won = false;
        run.stage = beginningStage;

        // Prepare for main menu UI
        mainMenu.SetActive(true);

        state.status = GameState.Status.MainMenu;
    }

    public RunState GetRunState() => run;
    public GameState GetGameState() => state;
}