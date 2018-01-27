using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileData))]
public class TileEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Recalculate Tiles"))
        {
            ((TileData)target).RecalculateTiles();
        }
    }
}
