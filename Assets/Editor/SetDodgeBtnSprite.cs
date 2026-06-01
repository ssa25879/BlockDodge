using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SetDodgeBtnSprite
{
    public static void Execute()
    {
        string assetPath = "Assets/Images/Dodge_btn.png";

        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        if (sprite == null)
        {
            Debug.LogError("Sprite load failed: " + assetPath);
            return;
        }

        GameObject go = GameObject.Find("Canvas/Dodge_btn");
        if (go == null)
        {
            Debug.LogError("Canvas/Dodge_btn not found");
            return;
        }

        Image img = go.GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("Image component not found on Canvas/Dodge_btn");
            return;
        }

        img.sprite = sprite;
        EditorUtility.SetDirty(go);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(go.scene);
        Debug.Log("Dodge_btn sprite updated successfully.");
    }
}
