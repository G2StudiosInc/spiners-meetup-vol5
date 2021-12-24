#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AchievementEnumGenerator : MonoBehaviour
{

	[Serializable]
	public class AchievementJsonData
	{

		public string title = default; // 実績名
		public string conditions = default; // 解放条件
		public string achievementName = default; // enumで指定するときの名前

	}

	// メニューの「Tools -> Generate AchievementEnum Script」を選択するとGenerateメソッドが呼ばれる
	[MenuItem("Tools/Generator/Generate AchievementEnum Script")]
	public static void Generate()
	{
		string jsonStr = "";

		StreamReader streamReader = new StreamReader($"{Application.dataPath}/Common/SaveLoad/JSON/AchievementList.json");
		jsonStr = streamReader.ReadToEnd();
		streamReader.Close();

		AchievementJsonData[] dataJson = JsonHelper.FromJson<AchievementJsonData>(jsonStr);

		// 作成するアセットのパス
		string generateScriptPath = "Assets/Common/SaveLoad/Scripts/AchievementEnum.cs";

		List<string> enumNames = new List<string>();

		foreach (AchievementJsonData data in dataJson)
		{
			enumNames.Add(data.achievementName);
		}

		string enumMessage = "public enum AchievementName {\n";

		foreach (string enumName in enumNames)
		{
			Debug.Log(enumName);
			enumMessage += "	" + enumName.ToUpper() + ",\n";
		}

		enumMessage += "}";

		// アセット(.cs)を作成する
		File.WriteAllText(generateScriptPath, enumMessage);

		// 変更があったアセットをインポートする(UnityEditorの更新)
		AssetDatabase.Refresh();
	}

}
#endif