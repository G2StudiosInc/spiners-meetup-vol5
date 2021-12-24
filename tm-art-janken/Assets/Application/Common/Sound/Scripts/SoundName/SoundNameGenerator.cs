#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class SoundNameGenerator : MonoBehaviour
{
	
	private static readonly string pathSoundFolder = "Assets/Application/Common/Sound";

	/// <summary> サウンドスクリプトデータ格納フォルダパス </summary>
	private static readonly string pathSoundNameScriptFolder = $"{pathSoundFolder}/Scripts/SoundName";
	/// <summary> サウンドデータ格納フォルダパス </summary>
	private static readonly string pathSoundDataFolder = $"{pathSoundFolder}/Data";

	/// <summary> BGMスクリプトのパス </summary>
	private static readonly string pathSoundNameScriptBGM = $"{pathSoundNameScriptFolder}/SoundNameBGM.cs";
	/// <summary> BGMデータのパス </summary>
	private static readonly string pathDataFolderBGM = $"{pathSoundDataFolder}/BGM";

	/// <summary> SEスクリプトのパス </summary>
	private static readonly string pathSoundNameScriptSE = $"{pathSoundNameScriptFolder}/SoundNameSE.cs";
	/// <summary> SEデータのパス </summary>
	private static readonly string pathDataFolderSE = $"{pathSoundDataFolder}/SE";

	/// <summary> VOICEスクリプトのパス </summary>
	private static readonly string pathSoundNameScriptVOICE = $"{pathSoundNameScriptFolder}/SoundNameVOICE.cs";
	/// <summary> VOICEデータのパス </summary>
	private static readonly string pathDataFolderVOICE = $"{pathSoundDataFolder}/VOICE";

	// メニューの「Tools -> Generate Script」を選択するとGenerateメソッドが呼ばれる
	// [MenuItem("Tools/Generate SoundNames Script")]
	public static List<string> GenerateBGM()
	{
		List<string> filePaths = Directory.GetFiles(pathDataFolderBGM, "*", SearchOption.TopDirectoryOnly)
			.Where(_ => !_.EndsWith(".meta", System.StringComparison.OrdinalIgnoreCase))
			.ToList();

		List<string> enumNames = new List<string>();

		foreach (string filePath in filePaths)
		{
			enumNames.Add(Path.GetFileNameWithoutExtension(filePath));
		}

		string enumMessage = "public enum BGMName {\n";

		foreach (string enumName in enumNames)
		{
			Debug.Log(enumName);
			enumMessage += "	" + enumName.ToUpper() + ",\n";
		}

		enumMessage += "}";

		// アセット(.cs)を作成する
		File.WriteAllText(pathSoundNameScriptBGM, enumMessage);

		// 変更があったアセットをインポートする(UnityEditorの更新)
		AssetDatabase.Refresh();

		return filePaths;
	}

	public static List<string> GenerateSE()
	{
		List<string> filePaths = Directory.GetFiles(pathDataFolderSE, "*", SearchOption.TopDirectoryOnly)
			.Where(_ => !_.EndsWith(".meta", System.StringComparison.OrdinalIgnoreCase))
			.ToList();

		List<string> enumNames = new List<string>();

		foreach (string filePath in filePaths)
		{
			enumNames.Add(Path.GetFileNameWithoutExtension(filePath));
		}

		string enumMessage = "public enum SEName {\n";

		foreach (string enumName in enumNames)
		{
			Debug.Log(enumName);
			enumMessage += "	" + enumName.ToUpper() + ",\n";
		}

		enumMessage += "}";

		// アセット(.cs)を作成する
		File.WriteAllText(pathSoundNameScriptSE, enumMessage);

		// 変更があったアセットをインポートする(UnityEditorの更新)
		AssetDatabase.Refresh();

		return filePaths;
	}

	public static List<string> GenerateVOICE()
	{
		List<string> filePaths = Directory.GetFiles(pathDataFolderVOICE, "*", SearchOption.TopDirectoryOnly)
			.Where(_ => !_.EndsWith(".meta", System.StringComparison.OrdinalIgnoreCase))
			.ToList();

		List<string> enumNames = new List<string>();

		foreach (string filePath in filePaths)
		{
			enumNames.Add(Path.GetFileNameWithoutExtension(filePath));
		}

		string enumMessage = "public enum VOICEName {\n";

		foreach (string enumName in enumNames)
		{
			Debug.Log(enumName);
			enumMessage += "	" + enumName.ToUpper() + ",\n";
		}

		enumMessage += "}";

		// アセット(.cs)を作成する
		File.WriteAllText(pathSoundNameScriptVOICE, enumMessage);

		// 変更があったアセットをインポートする(UnityEditorの更新)
		AssetDatabase.Refresh();

		return filePaths;
	}

}
#endif