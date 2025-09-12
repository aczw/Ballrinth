public struct GameState
{
    public enum Status
    {
        MainMenu,
        InGame,
        Intermission,
        End,
        Transitioning
    }

    public Status status;
    public int maxStagesEscaped;
}