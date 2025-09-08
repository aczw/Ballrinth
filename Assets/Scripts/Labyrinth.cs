using System;
using UnityEngine;

public class Labyrinth : MonoBehaviour
{
    [SerializeField] private Prefabs prefabs;

    public void Generate(int rows, int columns) {
        Debug.Log($"generated with {rows} rows, {columns} columns");
    }

    [Serializable]
    private struct Prefabs {
        [SerializeField] private GameObject floor;
        [SerializeField] private GameObject corner;
        [SerializeField] private GameObject wall;
        [SerializeField] private GameObject wallEnd;
    }
}
