using System;
using UnityEngine;

public class Labyrinth : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] [Min(0f)] private float rotationLimit;
    [SerializeField] [Min(0f)] private float rotationStep;

    [Header("Prefabs")]
    [SerializeField] private LabyrinthPieces labyrinth;
    [SerializeField] private GameObject ball;

    private void GenerateOuterWalls(int rows, int columns, Vector2 rowBounds, Vector2 colBounds) {
        var wallT = Instantiate(labyrinth.wall,
                                new Vector3(0f, 0.25f, rowBounds.y + 0.5f + 0.125f), Quaternion.identity, transform);
        var wallB = Instantiate(labyrinth.wall,
                                new Vector3(0f, 0.25f, rowBounds.x - 0.5f - 0.125f), Quaternion.identity, transform);
        var wallL = Instantiate(labyrinth.wall,
                                new Vector3(colBounds.x - 0.5f - 0.125f, 0.25f, 0f), Quaternion.Euler(0f, 90f, 0f),
                                transform);
        var wallR = Instantiate(labyrinth.wall,
                                new Vector3(colBounds.y + 0.5f + 0.125f, 0.25f, 0f), Quaternion.Euler(0f, 90f, 0f),
                                transform);

        var columnScale = new Vector3(columns, 1f, labyrinth.wall.transform.localScale.z);
        wallT.transform.localScale = columnScale;
        wallB.transform.localScale = columnScale;

        var rowScale = new Vector3(rows, 1f, labyrinth.wall.transform.localScale.z);
        wallL.transform.localScale = rowScale;
        wallR.transform.localScale = rowScale;

        Instantiate(labyrinth.corner,
                    new Vector3(colBounds.x - 0.625f, 0.25f, rowBounds.y + 0.625f), Quaternion.identity, transform);
        Instantiate(labyrinth.corner,
                    new Vector3(colBounds.y + 0.625f, 0.25f, rowBounds.y + 0.625f), Quaternion.identity, transform);
        Instantiate(labyrinth.corner,
                    new Vector3(colBounds.x - 0.625f, 0.25f, rowBounds.x - 0.625f), Quaternion.identity, transform);
        Instantiate(labyrinth.corner,
                    new Vector3(colBounds.y + 0.625f, 0.25f, rowBounds.x - 0.625f), Quaternion.identity, transform);
    }

    public void Generate(int rows, int columns) {
        var rowOffset = Mathf.Floor(rows / 2f) + (rows % 2 == 0 ? -0.5f : 0f);
        var colOffset = Mathf.Floor(columns / 2f) + (rows % 2 == 0 ? -0.5f : 0f);
        var rowBounds = new Vector2(-rowOffset, rows - rowOffset - 1f);
        var colBounds = new Vector2(-colOffset, columns - colOffset - 1f);

        if (transform.localEulerAngles != Vector3.zero) {
            Debug.LogWarning($"Labyrinth rotation is not zero: {transform.localEulerAngles}");
            transform.localEulerAngles = Vector3.zero;
        }

        // Depending on maze size parity the exit is spawned in a different location
        var exitTilePosition =
            rows % 2 == 0 ? new Vector2(colBounds.x, rowBounds.x) : new Vector2(colBounds.y, rowBounds.x);

        // Generate floor tiles
        for (var z = rowBounds.x; z <= rowBounds.y; ++z) {
            for (var x = colBounds.x; x <= colBounds.y; ++x) {
                var tilePrefab = labyrinth.tiles.floor;

                if (Mathf.Approximately(exitTilePosition.x, x) && Mathf.Approximately(exitTilePosition.y, z)) {
                    tilePrefab = labyrinth.tiles.exit;
                }

                Instantiate(tilePrefab, new Vector3(x, -0.125f, z), Quaternion.identity, transform);

                // Spawn ball in the top left corner
                if (Mathf.Approximately(z, rowBounds.y) && Mathf.Approximately(x, colBounds.x)) {
                    Instantiate(ball, new Vector3(x, 5f, z), Quaternion.identity);
                }
            }
        }

        var leftWallEndBounds = new Vector2(colBounds.x + 1f, colBounds.y);
        var rightWallEndBounds = new Vector2(colBounds.x, colBounds.y - 1f);

        // Generate inner walls
        var wallEnd = rows % 2 == 0 ? WallEnd.Right : WallEnd.Left;
        for (var z = rowBounds.x + 1f; z <= rowBounds.y; ++z) {
            // Definition of "end" depends on which side we're currently considering as the wall end
            var actualColBounds = wallEnd switch {
                WallEnd.Left => leftWallEndBounds,
                WallEnd.Right => rightWallEndBounds,
                _ => throw new ArgumentOutOfRangeException()
            };

            for (var x = actualColBounds.x; x <= actualColBounds.y; ++x) {
                // If at the left end, generate left wall end + corner combo. 
                if (Mathf.Approximately(x, actualColBounds.x) && wallEnd == WallEnd.Left) {
                    Instantiate(labyrinth.wallEnd,
                                new Vector3(x + 0.0625f, 0.25f, z - 0.5f), Quaternion.identity, transform);
                    Instantiate(labyrinth.corner,
                                new Vector3(x - 0.5f, 0.25f, z - 0.5f), Quaternion.identity, transform);
                    continue;
                }

                // If at the right end, generate right wall end + corner combo. 
                if (x + 1f > actualColBounds.y && wallEnd == WallEnd.Right) {
                    Instantiate(labyrinth.wallEnd,
                                new Vector3(x - 0.0625f, 0.25f, z - 0.5f), Quaternion.identity, transform);
                    Instantiate(labyrinth.corner,
                                new Vector3(x + 0.5f, 0.25f, z - 0.5f), Quaternion.identity, transform);
                    continue;
                }

                // Else, generate regular wall
                Instantiate(labyrinth.wall,
                            new Vector3(x, 0.25f, z - 0.5f), Quaternion.identity, transform);
            }

            // Ping pong
            wallEnd = wallEnd == WallEnd.Right ? WallEnd.Left : WallEnd.Right;
        }

        GenerateOuterWalls(rows, columns, rowBounds, colBounds);

        Debug.Log($"Generated with {rows} rows, {columns} columns");
    }

    public void Clear() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        Destroy(GameObject.FindWithTag("Player"));

        transform.localEulerAngles = Vector3.zero;
    }

    public void ProcessRotation(RotationInputBundle input) {
        var deltaVertical = 0f;
        var deltaHorizontal = 0f;

        if (input.rotateUp) deltaVertical += rotationStep;
        if (input.rotateDown) deltaVertical -= rotationStep;
        if (input.rotateLeft) deltaHorizontal += rotationStep;
        if (input.rotateRight) deltaHorizontal -= rotationStep;

        // Unity normalizes the angle to be between [0, 360) so we correct that here
        var rotation = transform.localEulerAngles;
        if (rotation.x > 180f) rotation.x -= 360f;
        if (rotation.z > 180f) rotation.z -= 360f;

        var vertical = Mathf.Clamp(rotation.x + deltaVertical, -rotationLimit, rotationLimit);
        var horizontal = Mathf.Clamp(rotation.z + deltaHorizontal, -rotationLimit, rotationLimit);
        transform.localEulerAngles = new Vector3(vertical, 0f, horizontal);
    }

    private enum WallEnd
    {
        Left,
        Right
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
}