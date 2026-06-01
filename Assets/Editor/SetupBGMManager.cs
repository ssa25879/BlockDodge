using UnityEngine;
using UnityEditor;

public class SetupBGMManager
{
    public static void Execute()
    {
        var go = GameObject.Find("BGMManager");
        if (go == null) { Debug.LogError("BGMManager not found"); return; }

        var bgmManager = go.GetComponent<BGMManager>();
        if (bgmManager == null) { Debug.LogError("BGMManager component not found"); return; }

        bgmManager.gameBGM = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Music/BGM.mp3");
        bgmManager.deadBGM = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Music/DeadBGM.mp3");

        EditorUtility.SetDirty(bgmManager);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(go.scene);

        Debug.Log($"BGMManager setup complete. gameBGM={bgmManager.gameBGM?.name}, deadBGM={bgmManager.deadBGM?.name}");
    }
}
