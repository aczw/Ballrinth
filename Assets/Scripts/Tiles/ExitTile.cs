using UnityEngine;

public class ExitTile : FloorTile
{
    public override void OnCollisionEnter(Collision other) {
        base.OnCollisionEnter(other);
        GameManager.I.EscapeStage();
    }
}