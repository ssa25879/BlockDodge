using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class SetupIntroScene
{
    public static void Execute()
    {
        // 1. Import all images as Sprite
        string[] imagePaths = new[]
        {
            "Assets/Images/Intro_img.png",
            "Assets/Images/Start_btn.png",
            "Assets/Images/Exit_btn.png"
        };

        foreach (string path in imagePaths)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }

        // 2. Load sprites
        Sprite introSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Intro_img.png");
        Sprite startSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Start_btn.png");
        Sprite exitSprite  = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Exit_btn.png");

        // 3. Set Intro_img background (Canvas/Image)
        GameObject introImgGo = GameObject.Find("Canvas/Image");
        if (introImgGo != null && introSprite != null)
        {
            Image img = introImgGo.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = introSprite;
                img.preserveAspect = false;
                EditorUtility.SetDirty(introImgGo);
            }
        }

        // 4. Setup Start button (existing Canvas/Button → rename to Start_btn)
        GameObject startGo = GameObject.Find("Canvas/Button");
        if (startGo != null)
        {
            startGo.name = "Start_btn";

            Image btnImg = startGo.GetComponent<Image>();
            if (btnImg != null && startSprite != null)
            {
                btnImg.sprite = startSprite;
                btnImg.color = Color.white;
            }

            // Remove text child
            Transform textChild = startGo.transform.Find("Text (TMP)");
            if (textChild != null)
                Object.DestroyImmediate(textChild.gameObject);

            // Position: left side, ~63% down from top → anchored from center
            RectTransform rt = startGo.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot     = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(-672f, -140f);
            rt.sizeDelta = new Vector2(200f, 70f);

            EditorUtility.SetDirty(startGo);
        }

        // 5. Create Exit button
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null) { Debug.LogError("Canvas not found"); return; }

        GameObject exitGo = new GameObject("Exit_btn");
        exitGo.transform.SetParent(canvas.transform, false);

        Image exitImg = exitGo.AddComponent<Image>();
        if (exitSprite != null) exitImg.sprite = exitSprite;
        exitImg.color = Color.white;

        exitGo.AddComponent<Button>();

        RectTransform exitRt = exitGo.GetComponent<RectTransform>();
        exitRt.anchorMin = new Vector2(0.5f, 0.5f);
        exitRt.anchorMax = new Vector2(0.5f, 0.5f);
        exitRt.pivot     = new Vector2(0.5f, 0.5f);
        exitRt.anchoredPosition = new Vector2(-672f, -240f);
        exitRt.sizeDelta = new Vector2(200f, 70f);

        EditorUtility.SetDirty(exitGo);

        EditorSceneManager.MarkSceneDirty(canvas.gameObject.scene);

        Debug.Log("Intro scene setup complete.");
    }
}
