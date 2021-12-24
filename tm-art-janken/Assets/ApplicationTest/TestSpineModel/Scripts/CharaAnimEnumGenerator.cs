#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CharaAnimEnumGenerator : MonoBehaviour {
	
	[SerializeField,Header("生成するSkeletonDataAssetを設定")]
	private Spine.Unity.SkeletonDataAsset skeletonDataAsset = default;

	[SerializeField,Header("生成するフォルダをパスで指定")]
	private string outputFileFolderPath = "Assets/Common/SaveLoad/Scripts";

	[SerializeField,Header("生成するファイル名を指定")]
	private string outputFileName = "CharaAnimEnum.cs";
	
	
	[ContextMenu("CharaAnimEnum生成")]
	public void Generate() {
		Generate(skeletonDataAsset, outputFileFolderPath, outputFileName);
	}

	[ContextMenu("CharaAnimEnum生成")]
//	[MenuItem("Tools/Generator/Generate CharaAnimEnum Script")]
	public static void Generate(Spine.Unity.SkeletonDataAsset skeletonDataAsset, string outputFileFolderPath, string outputFileName) {
		
		// 作成するアセットのパス
		var generateScriptPath = $"{outputFileFolderPath}/{outputFileName}";
		Debug.Log($"ファイルパス：{generateScriptPath}");

		List<string> enumNames = new List<string>();

		string debugEnumName = "[enum name]\n";
		foreach (var item in skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations.Items) {
			enumNames.Add(Path.GetFileNameWithoutExtension(item.Name));
			debugEnumName += " - " + Path.GetFileNameWithoutExtension(item.Name) + "\n";
		}
		Debug.Log(debugEnumName);

		string enumTypeMessage = @"
public enum CharaAnimType {
	janken = 0,
}

";
		string enumNameMessage = "public enum CharaAnimName {\n";
		foreach (string enumName in enumNames) {
			enumNameMessage += "	" + enumName + ",\n";
		}

		enumNameMessage += "}";

		// アセット(.cs)を作成する
		File.WriteAllText(generateScriptPath, enumTypeMessage + enumNameMessage);

		// 変更があったアセットをインポートする(UnityEditorの更新)
		AssetDatabase.Refresh();
	}

}
#endif