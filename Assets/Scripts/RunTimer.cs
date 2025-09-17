using UnityEngine;

public class RunTimer : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] [Min(0f)] private float duration;
    [SerializeField] [Min(0.01f)] private float speedMultiplier;

    private Status currentStatus;
    private float currentTime;

    private void Awake() {
        currentTime = duration;
        currentStatus = Status.Finished;
    }

    private void Update() {
        if (currentStatus != Status.Running) return;

        currentTime -= Time.deltaTime * speedMultiplier;

        if (currentTime < 0f) {
            currentTime = 0f;
            currentStatus = Status.Finished;
        }
    }

    public void Resume() => currentStatus = Status.Running;
    public void Pause() => currentStatus = Status.Paused;
    public bool IsFinished() => currentStatus == Status.Finished;

    public float GetCurrentTime() => currentTime;
    public void AddCurrentTime(float timeAdded) => currentTime += timeAdded;

    public void SetSpeedMultiplier(float multiplier) {
        if (multiplier < 0.01f) return;
        speedMultiplier = multiplier;
    }

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