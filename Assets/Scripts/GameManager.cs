using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] [Min(1)] private int beginningStage;

    [Header("Systems")]
    [SerializeField] private Labyrinth labyrinth;
    [SerializeField] private Timer timer;

    [Header("Canvas Objects")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject intermission;
    [SerializeField] private GameObject outcome;

    /// <summary>
    ///     Per-frame state.
    /// </summary>
    private FrameInputBundle input;

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

        input = new FrameInputBundle();

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

        Debug.Log($"Time left: {timer.GetCurrentTime()}");

        if (timer.IsFinished()) {
            EndGame();
            return;
        }

        input = new FrameInputBundle {
            rotateUp = Keyboard.current.upArrowKey.isPressed,
            rotateDown = Keyboard.current.downArrowKey.isPressed,
            rotateLeft = Keyboard.current.leftArrowKey.isPressed,
            rotateRight = Keyboard.current.rightArrowKey.isPressed
        };
    }

    private void FixedUpdate() => labyrinth.ProcessRotation(input);

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
        input = new FrameInputBundle();
        run.won = false;
        run.stage = beginningStage;

        // Launch the labyrinth again
        labyrinth.Generate(run.stage + 1, run.stage + 1);
        timer.Restart();

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
        input = new FrameInputBundle();
        run.won = false;
        run.stage = beginningStage;

        // Prepare for main menu UI
        mainMenu.SetActive(true);

        state.status = GameState.Status.MainMenu;
    }

    public RunState GetRunState() => run;
    public GameState GetGameState() => state;
}