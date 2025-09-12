using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] [Min(1)] private int beginningStage;

    [Header("Systems")]
    [SerializeField] private Labyrinth labyrinth;

    [Header("Canvas Objects")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject intermission;

    // Overall game state
    private int currentStage;
    private GameState currentState;

    // Per-frame state
    private FrameInputBundle input;

    public static GameManager I { get; private set; }

    private void Awake() {
        if (I == null) {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        currentStage = beginningStage;
        currentState = GameState.MainMenu;
    }

    private void Start() {
        labyrinth.Generate(currentStage + 1, currentStage + 1);
    }

    private void Update() {
        if (currentState != GameState.InGame) return;

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

        mainMenu.SetActive(false);

        currentState = GameState.InGame;
    }

    public void EscapeStage() {
        if (currentState != GameState.InGame) {
            Debug.LogError($"Incorrect game state: should be in-game, instead we're on {currentState}!");
            return;
        }

        labyrinth.Clear();

        currentState = GameState.Intermission;

        intermission.SetActive(true);
    }

    public void EnterNextStage() {
        if (currentState != GameState.Intermission) {
            Debug.LogError($"Incorrect game state: should be in the intermission, instead we're on {currentState}!");
            return;
        }

        intermission.SetActive(false);

        currentState = GameState.InGame;

        currentStage++;
        labyrinth.Generate(currentStage + 1, currentStage + 1);
    }

    private enum GameState
    {
        MainMenu,
        InGame,
        Intermission
    }
}