using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        MainMenu,
        InGame,
        Intermission,
        End,
        Transitioning
    }

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
    ///     Overall game state (tracked across runs).
    /// </summary>
    private int maxStagesEscaped;

    /// <summary>
    ///     Current run state.
    /// </summary>
    private RunState run;

    public static GameManager I { get; private set; }

    private void Awake() {
        if (I == null) {
            I = this;
        }
        else {
            Destroy(gameObject);
        }

        input = new FrameInputBundle();

        run = new RunState {
            won = false,
            stage = beginningStage,
            state = State.MainMenu
        };

        maxStagesEscaped = 0;
    }

    private void Update() {
        if (run.state != State.InGame) return;

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
        if (run.state != State.MainMenu) {
            Debug.LogError($"Incorrect game state: should be on the main menu, instead we're on {run.state}");
            return;
        }

        run.state = State.Transitioning;

        // Clear main menu UI
        mainMenu.SetActive(false);

        // Set up first labyrinth
        labyrinth.Generate(run.stage + 1, run.stage + 1);
        timer.Restart();

        run.state = State.InGame;
    }

    public void EscapeStage() {
        if (run.state != State.InGame) {
            Debug.LogError($"Incorrect game state: should be in-game, instead we're on {run.state}!");
            return;
        }

        run.state = State.Transitioning;

        // End current labyrinth run
        timer.Pause();
        labyrinth.Clear();

        // Set up intermission
        intermission.SetActive(true);

        run.state = State.Intermission;
    }

    public void EnterNextStage() {
        if (run.state != State.Intermission) {
            Debug.LogError($"Incorrect game state: should be in the intermission, instead we're on {run.state}!");
            return;
        }

        run.state = State.Transitioning;

        // Clear intermission UI
        intermission.SetActive(false);

        // Set up next stage
        run.stage++;
        labyrinth.Generate(run.stage + 1, run.stage + 1);
        timer.Resume();

        run.state = State.InGame;
    }

    public void EndGame() {
        if (run.state != State.InGame) {
            Debug.LogError($"Incorrect game state: should be in-game, instead we're on {run.state}!");
            return;
        }

        run.state = State.Transitioning;

        // Game can end before we run out of time
        if (!timer.IsFinished()) timer.Pause();

        // End current labyrinth run and wrap up any final game state
        labyrinth.Clear();

        if (run.stage >= maxStagesEscaped) {
            run.won = true;
            maxStagesEscaped = run.stage;
        }

        run.state = State.End;

        // Set up final outcome screen
        outcome.SetActive(true);
    }

    public void RestartGame() {
        if (run.state != State.End) {
            Debug.LogError($"Incorrect game state: should be at the end, instead we're on {run.state}!");
            return;
        }

        run.state = State.Transitioning;

        // Clear outcome UI
        outcome.SetActive(false);

        // Reset all state as needed and launch the labyrinth again
        input = new FrameInputBundle();

        run.won = false;
        run.stage = beginningStage;

        labyrinth.Generate(run.stage + 1, run.stage + 1);
        timer.Restart();

        run.state = State.InGame;
    }

    public RunState GetRunState() => run;

    public int GetMaxStagesEscaped() => maxStagesEscaped;
}