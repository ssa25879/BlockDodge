using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class FixButtonAnchors
{
    public static void Execute()
    {
        // Reference resolution from CanvasScaler: 3120 x 1440
        Vector2 refRes = new Vector2(3120f, 1440f);

        FixAnchor("Canvas/Start_btn", refRes);
        FixAnchor("Canvas/Exit_btn", refRes);

        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("Button anchors fixed and scene saved.");
    }

    static void FixAnchor(string path, Vector2 refRes)
    {
        GameObject go = GameObject.Find(path);
        if (go == null) { Debug.LogError("Not found: " + path); return; }

        RectTransform rt = go.GetComponent<RectTransform>();
        if (rt == null) return;

        // Current anchoredPosition with anchor at (0,0) = absolute position from bottom-left
        Vector2 absPos = rt.anchoredPosition;

        // Normalize to [0,1] range based on reference resolution
        Vector2 normalizedAnchor = new Vector2(absPos.x / refRes.x, absPos.y / refRes.y);

        rt.anchorMin = normalizedAnchor;
        rt.anchorMax = normalizedAnchor;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;

        EditorUtility.SetDirty(go);
        Debug.Log($"{path} anchor set to ({normalizedAnchor.x:F4}, {normalizedAnchor.y:F4})");
    }
}
