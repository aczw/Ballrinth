using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] [Min(1)] private int beginningStage;

    [Header("Systems")]
    [SerializeField] private Labyrinth labyrinth;

    // Overall game state
    private int currentStage;

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
    }

    private void Start() {
        labyrinth.Generate(currentStage + 1, currentStage + 1);
    }

    private void Update() {
        input = new FrameInputBundle {
            rotateUp = Keyboard.current.upArrowKey.isPressed,
            rotateDown = Keyboard.current.downArrowKey.isPressed,
            rotateLeft = Keyboard.current.leftArrowKey.isPressed,
            rotateRight = Keyboard.current.rightArrowKey.isPressed
        };
    }

    private void FixedUpdate() => labyrinth.ProcessRotation(input);

    public void EscapeStage() {
        labyrinth.Clear();
        currentStage++;
        labyrinth.Generate(currentStage + 1, currentStage + 1);
    }
}