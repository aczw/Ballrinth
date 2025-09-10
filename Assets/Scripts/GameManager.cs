using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Labyrinth labyrinth;

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

    public void EscapeStage() {
        Debug.Log($"Escaped stage {stage}!");
    }
}