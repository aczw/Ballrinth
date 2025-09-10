using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Systems
    [SerializeField] private Labyrinth labyrinth;

    // Per-frame state
    private FrameInputBundle input;

    // Game state
    private int stage;

    public static GameManager I { get; private set; }

    private void Awake() {
        if (I == null) {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

        stage = 3;
    }

    private void Start() {
        labyrinth.Generate(stage + 1, stage + 1);
    }

    private void Update() {
        input = new FrameInputBundle {
            rotateUp = Keyboard.current.upArrowKey.isPressed,
            rotateDown = Keyboard.current.downArrowKey.isPressed,
            rotateLeft = Keyboard.current.leftArrowKey.isPressed,
            rotateRight = Keyboard.current.rightArrowKey.isPressed
        };
    }

    private void FixedUpdate() =>
        labyrinth.ProcessRotation(input.rotateUp, input.rotateDown, input.rotateLeft, input.rotateRight);

    public void EscapeStage() {
        Debug.Log($"Escaped stage {stage}!");

        labyrinth.Clear();
        stage++;
        labyrinth.Generate(stage + 1, stage + 1);
    }
}