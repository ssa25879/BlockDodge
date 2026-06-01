using UnityEditor;

public class ImportSprites
{
    public static void Execute()
    {
        string[] paths = new[]
        {
            "Assets/Images/Intro_img.png",
            "Assets/Images/Start_btn.png",
            "Assets/Images/Exit_btn.png"
        };

        foreach (string path in paths)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null && importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }

        AssetDatabase.Refresh();
        UnityEngine.Debug.Log("Sprites imported successfully.");
    }
}
