using UnityEngine;

public class MainMenuCanvas : MonoBehaviour
{
    public static void StartGame() => GameManager.I.StartGame();

    public static void QuitGame() => Application.Quit();
}