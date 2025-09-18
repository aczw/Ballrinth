using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            GameManager.I.sfxSource.PlayOneShot(GameManager.I.hitWall, 1f);
        }
    }
}