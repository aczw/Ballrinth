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

    // Current run state
    private int currentStage;
    private GameState currentState;

    // Per-frame state
    private FrameInputBundle input;

    // Overall game state
    private int maxStagesEscaped;

    public static GameManager I { get; private set; }

    private void Awake() {
        if (I == null) {
            I = this;
        }
        else {
            Destroy(gameObject);
        }

        currentStage = beginningStage;
        currentState = GameState.MainMenu;
        maxStagesEscaped = 0;
    }

    private void Update() {
        if (currentState != GameState.InGame) return;

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
        if (currentState != GameState.MainMenu) {
            Debug.LogError($"Incorrect game state: should be on the main menu, instead we're on {currentState}");
            return;
        }

        currentState = GameState.Transitioning;

        // Clear main menu UI
        mainMenu.SetActive(false);

        // Set up first labyrinth
        labyrinth.Generate(currentStage + 1, currentStage + 1);
        timer.Restart();

        currentState = GameState.InGame;
    }

    public void EscapeStage() {
        if (currentState != GameState.InGame) {
            Debug.LogError($"Incorrect game state: should be in-game, instead we're on {currentState}!");
            return;
        }

        currentState = GameState.Transitioning;

        // End current labyrinth run
        timer.Pause();
        labyrinth.Clear();

        // Set up intermission
        intermission.SetActive(true);

        currentState = GameState.Intermission;
    }

    public void EnterNextStage() {
        if (currentState != GameState.Intermission) {
            Debug.LogError($"Incorrect game state: should be in the intermission, instead we're on {currentState}!");
            return;
        }

        currentState = GameState.Transitioning;

        // Clear intermission UI
        intermission.SetActive(false);

        // Set up next stage
        currentStage++;
        labyrinth.Generate(currentStage + 1, currentStage + 1);
        timer.Resume();

        currentState = GameState.InGame;
    }

    public void EndGame() {
        if (currentState != GameState.InGame) {
            Debug.LogError($"Incorrect game state: should be in-game, instead we're on {currentState}!");
            return;
        }

        currentState = GameState.Transitioning;

        // Game can end before we run out of time
        if (!timer.IsFinished()) timer.Pause();

        // End current labyrinth run and wrap up any final game state
        labyrinth.Clear();

        var wonGame = false;
        if (currentStage >= maxStagesEscaped) {
            wonGame = true;
            maxStagesEscaped = currentStage;
        }

        Debug.Log(wonGame
                      ? $"YOU WON THE GAME WITH {maxStagesEscaped} STAGES CLEARED!"
                      : $"YOU LOST! NUM STAGES ESCAPED: {currentStage}, HIGH SCORE: {maxStagesEscaped}");

        // Set up final outcome screen
        // TODO!

        currentState = GameState.End;
    }

    private enum GameState
    {
        MainMenu,
        InGame,
        Intermission,
        End,
        Transitioning
    }
}