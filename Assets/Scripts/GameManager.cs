using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Systems
    [SerializeField] private Labyrinth labyrinth;

    // Game state
    private int stage;

    private void Awake() {
        stage = 1;
    }
    
    private void Start() {
        labyrinth.Generate(stage + 1, stage + 1);
    }
}
