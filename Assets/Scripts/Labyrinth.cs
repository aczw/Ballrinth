using System;
using UnityEngine;

public class Labyrinth : MonoBehaviour
{
    [SerializeField] private Prefabs prefabs;

    public void Generate(int rows, int columns) {
        var rowOffset = Mathf.Floor(rows / 2f) + (rows % 2 == 0 ? -0.5f : 0f);
        var colOffset = Mathf.Floor(columns / 2f) + (rows % 2 == 0 ? -0.5f : 0f);

        Debug.Log($"row offset: {rowOffset}, col offset: {colOffset}");

        var rowBounds = new Vector2(-rowOffset, rows - rowOffset - 1f);
        var colBounds = new Vector2(-colOffset, columns - colOffset - 1f);

        Debug.Log($"row bounds: {rowBounds}, col bounds: {colBounds}");

        // Depending on maze size parity the exit is spawned in a different location
        var exitTilePosition =
            rows % 2 == 0 ? new Vector2(colBounds.x, rowBounds.x) : new Vector2(colBounds.y, rowBounds.x);

        // Generate floor tiles
        for (var z = rowBounds.x; z <= rowBounds.y; ++z) {
            for (var x = colBounds.x; x <= colBounds.y; ++x) {
                var tilePrefab = exitTilePosition == new Vector2(x, z)
                    ? prefabs.labyrinth.tiles.exit
                    : prefabs.labyrinth.tiles.floor;

                Instantiate(tilePrefab, new Vector3(x, -0.125f, z), Quaternion.identity, transform);

                // Spawn ball in the top left corner
                if (Mathf.Approximately(z, rowBounds.y) && Mathf.Approximately(x, colBounds.x)) {
                    Instantiate(prefabs.ball, new Vector3(x, 5f, z), Quaternion.identity);
                }
            }
        }

        var wallT = Instantiate(prefabs.labyrinth.wall,
                                new Vector3(0f, 0.25f, rowBounds.y + 0.5f + 0.125f),
                                Quaternion.identity, transform);
        var wallB = Instantiate(prefabs.labyrinth.wall,
                                new Vector3(0f, 0.25f, rowBounds.x - 0.5f - 0.125f),
                                Quaternion.identity, transform);
        var wallL = Instantiate(prefabs.labyrinth.wall,
                                new Vector3(colBounds.x - 0.5f - 0.125f, 0.25f, 0f),
                                Quaternion.Euler(0f, 90f, 0f), transform);
        var wallR = Instantiate(prefabs.labyrinth.wall,
                                new Vector3(colBounds.y + 0.5f + 0.125f, 0.25f, 0f),
                                Quaternion.Euler(0f, 90f, 0f), transform);

        var columnScale = new Vector3(columns, 1f, prefabs.labyrinth.wall.transform.localScale.z);
        wallT.transform.localScale = columnScale;
        wallB.transform.localScale = columnScale;

        var rowScale = new Vector3(rows, 1f, prefabs.labyrinth.wall.transform.localScale.z);
        wallL.transform.localScale = rowScale;
        wallR.transform.localScale = rowScale;

        Instantiate(prefabs.labyrinth.corner,
                    new Vector3(colBounds.x - 0.625f, 0.25f, rowBounds.y + 0.625f),
                    Quaternion.identity, transform);
        Instantiate(prefabs.labyrinth.corner,
                    new Vector3(colBounds.y + 0.625f, 0.25f, rowBounds.y + 0.625f),
                    Quaternion.identity, transform);
        Instantiate(prefabs.labyrinth.corner,
                    new Vector3(colBounds.x - 0.625f, 0.25f, rowBounds.x - 0.625f),
                    Quaternion.identity, transform);
        Instantiate(prefabs.labyrinth.corner,
                    new Vector3(colBounds.y + 0.625f, 0.25f, rowBounds.x - 0.625f),
                    Quaternion.identity, transform);

        Debug.Log($"generated with {rows} rows, {columns} columns");
    }

    public void ProcessRotation(bool up, bool down, bool left, bool right) {
        var deltaVertical = 0f;
        var deltaHorizontal = 0f;

        if (up) deltaVertical += 1f;
        if (down) deltaVertical -= 1f;
        if (left) deltaHorizontal += 1f;
        if (right) deltaHorizontal -= 1f;

        var rotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(rotation.x + deltaVertical, 0f, rotation.z + deltaHorizontal);
    }

    [Serializable]
    private struct Tiles
    {
        public GameObject floor;
        public GameObject exit;
    }

    [Serializable]
    private struct LabyrinthPieces
    {
        public Tiles tiles;
        public GameObject corner;
        public GameObject wall;
        public GameObject wallEnd;
    }

    [Serializable]
    private struct Prefabs
    {
        public LabyrinthPieces labyrinth;
        public GameObject ball;
    }
}