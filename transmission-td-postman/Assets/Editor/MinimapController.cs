using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MinimapData))]
public class MinimapController : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Recalculate Minimap"))
        {
            MinimapData data = target as MinimapData;
            Transform transform = data.gameObject.transform;
            while (transform.childCount > 0)
            {
                Transform child = transform.GetChild(0);
                if (Application.isEditor)
                    DestroyImmediate(child.gameObject);
                else
                    Destroy(child.gameObject);
            }
        }
        GetOrCreateCamera();
        GetOrCreateCanvas();
    }

    private GameObject GetOrCreateCamera()
    {
        MinimapData data = target as MinimapData;
        Transform transform = data.gameObject.transform;
        Transform cameraTransform = transform.Find("Minimap-Camera");

        Bounds? terrainBounds = null;
        GameObject cameraNode = cameraTransform != null ? cameraTransform.gameObject : null;
        if (cameraNode == null)
        {
            cameraNode = new GameObject("Minimap-Camera");
            cameraTransform = cameraNode.transform;
            cameraTransform.parent = transform;
            if (data.Terrain != null)
            {
                terrainBounds = CalculateBounds(data.Terrain);
                cameraTransform.Translate(terrainBounds.Value.center + new Vector3(0, 100, 0));
            }
            cameraTransform.Rotate(new Vector3(1, 0, 0), 90);
        }

        Camera camera = cameraNode.GetComponent<Camera>();
        if (camera == null)
        {
            camera = cameraNode.AddComponent<Camera>();
            camera.targetTexture = (RenderTexture)AssetDatabase.LoadAssetAtPath("Assets/Minimap/MinimapTexture.renderTexture", typeof(RenderTexture));
            camera.orthographic = true;
            if (terrainBounds == null && data.Terrain != null)
            {
                terrainBounds = CalculateBounds(data.Terrain);
            }
            if (terrainBounds != null)
            {
                camera.orthographicSize = terrainBounds.Value.extents.magnitude / 2;
            }
        }

        return cameraNode;
    }

    private Bounds CalculateBounds(GameObject gameObject)
    {
        Bounds box = new Bounds();
        Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in allRenderers)
        {
            Bounds bounds = renderer.bounds;
            Vector3 min = Vector3.Min(box.min, bounds.min);
            Vector3 max = Vector3.Max(box.max, bounds.max);
            box.Encapsulate(min);
            box.Encapsulate(max);
        }
        return box;
    }

    private GameObject GetOrCreateCanvas()
    {
        MinimapData data = target as MinimapData;
        Transform transform = data.gameObject.transform;
        Transform canvasTransform = transform.Find("Minimap-Canvas");

        GameObject canvasNode = canvasTransform != null ? canvasTransform.gameObject : null;
        if (canvasNode == null)
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Minimap/Canvas.prefab", typeof(GameObject));
            canvasNode = Instantiate(prefab, transform);
            canvasNode.name = "Minimap-Canvas";
        }

        return canvasNode;
    }
}
