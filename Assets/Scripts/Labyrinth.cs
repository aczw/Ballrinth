using System;
using UnityEngine;

public class Labyrinth : MonoBehaviour
{
    [SerializeField] private Prefabs prefabs;

    public void Generate(int rows, int columns) {
        var rowOffset = Mathf.Floor(rows / 2f);
        var colOffset = Mathf.Floor(columns / 2f);

        if (rows % 2 == 0) { rowOffset -= 0.5f; }
        if (columns % 2 == 0) { colOffset -= 0.5f; }
        
        Debug.Log($"row offset: {rowOffset}, col offset: {colOffset}");
        
        // Generate floor tiles
        for (var z = -rowOffset; z < rows - rowOffset; ++z) {
            for (var x = -colOffset; x < columns - colOffset; ++x) {
                Instantiate(prefabs.floor, new Vector3(x, -0.125f, z), Quaternion.identity, transform);
            }
        }
        
        Debug.Log($"generated with {rows} rows, {columns} columns");
    }

    [Serializable]
    private struct Prefabs {
        public GameObject floor;
        public GameObject corner;
        public GameObject wall;
        public GameObject wallEnd;
    }
}
