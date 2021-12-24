using UnityEngine;

public class SEManager : SoundManager
{

    #if UNITY_EDITOR
    [ContextMenu("AudioClip関連の初期化")]
    public void InitAudioClip()
    {
        var filePathList = SoundNameGenerator.GenerateSE();

        audioClips.Clear();

        foreach (string filePath in filePathList)
        {
            audioClips.Add((AudioClip)UnityEditor.AssetDatabase.LoadAssetAtPath(filePath, typeof(AudioClip)));
        }
    }
    #endif

}
