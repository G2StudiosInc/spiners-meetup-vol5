using UnityEngine;
using System.Collections.Generic;

public class VOICEManager : SoundManager
{

    #if UNITY_EDITOR
    [ContextMenu("AudioClip関連の初期化")]
    public void InitAudioClip()
    {
        List<string> filePathList = SoundNameGenerator.GenerateVOICE();

        audioClips.Clear();

        foreach (string filePath in filePathList)
        {
            audioClips.Add((AudioClip)UnityEditor.AssetDatabase.LoadAssetAtPath(filePath, typeof(AudioClip)));
        }
    }
    #endif

}