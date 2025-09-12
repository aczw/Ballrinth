using UnityEngine;

public class MainMenuCanvas : MonoBehaviour
{
    public static void StartGame() => Debug.Log("Started game!");

    public static void QuitGame() => Application.Quit();
}