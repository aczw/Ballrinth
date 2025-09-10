using UnityEngine;

public class FloorTile : MonoBehaviour
{
    public virtual void OnCollisionEnter(Collision other) {
        Debug.Log($"Entered floor tile w/ pos {transform.localPosition}!");

        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Ball entered this tile!");
        }
    }
}