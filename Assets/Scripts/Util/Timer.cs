using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] [Min(0f)] private float duration;

    private Status currentStatus;
    private float currentTime;

    private void Awake() {
        currentTime = duration;
        currentStatus = Status.Finished;
    }

    private void Update() {
        if (currentStatus != Status.Running) return;

        currentTime -= Time.deltaTime;

        if (currentTime < 0f) {
            currentTime = 0f;
            currentStatus = Status.Finished;
        }
    }

    public void Resume() => currentStatus = Status.Running;
    public void Pause() => currentStatus = Status.Paused;
    public bool IsFinished() => currentStatus == Status.Finished;

    public float GetCurrentTime() => currentTime;

    public void Restart() {
        currentStatus = Status.Finished;
        Awake();
        currentStatus = Status.Running;
    }

    private enum Status
    {
        Running,
        Paused,
        Finished
    }
}