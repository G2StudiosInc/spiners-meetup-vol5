using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager
{

	[Serializable]
	public class AchievementData
	{

		public string title = default; // 実績名
		public string conditions = default; // 解放条件
		public AchievementName achievementName = default; // enumで指定するときの名前

	}

	private readonly List<AchievementData> achievementDataList = new List<AchievementData>();

	private List<bool> isClears = new List<bool>();

	/// <summary>
	/// 初期化処理
	/// CSVのデータ読み込み、AchievementCSVに設定
	/// </summary>
	public void Init()
	{
		isClears.Clear();

		string jsonPath = "JSON/AchievementList";
		string jsonStr = Resources.Load<TextAsset>(jsonPath).ToString();

		AchievementData[] dataJson = JsonHelper.FromJson<AchievementData>(jsonStr);

		for (int i = 0; i < dataJson.Length; i++)
		{
			dataJson[i].achievementName = (AchievementName)Enum.ToObject(typeof(AchievementName), i);
			achievementDataList.Add(dataJson[i]);
			isClears.Add(false); // 実績の解放状態の初期化
		}
	}

	/// <summary>
	/// 解放の進捗を初期化
	/// </summary>
	public void Reset()
	{
		isClears.Clear();

		for (int i = 0; i < achievementDataList.Count; i++)
		{
			isClears.Add(false);
		}
	}

	/// <summary>
	/// 実績の表示に必要なAchievementDataクラスを取得
	/// </summary>
	/// <returns></returns>
	public List<AchievementData> GetAchievementDataList()
	{
		return achievementDataList;
	}

	/// <summary>
	/// 実績解放処理
	/// </summary>
	/// <param name="achievementName">解放する実績のenumを指定</param>
	public void SetIsClear(AchievementName achievementName, bool isClear = true)
	{
		isClears[(int)achievementName] = isClear;
	}

	/// <summary>
	/// 任意の実績が解放されているかどうかを取得
	/// </summary>
	/// <param name="achievementName">解放する実績のenumを指定</param>
	public bool GetIsClear(AchievementName achievementName)
	{
		return isClears[(int)achievementName];
	}

	/// <summary>
	/// 解放の進捗を設定
	/// SaveLoadManager.csから使用
	/// </summary>
	/// <param name="isClears">解放されて実績のindexリスト</param>
	public void SetIsLocks(List<bool> isClears)
	{
		this.isClears = isClears;
	}

	/// <summary>
	/// 解放の進捗を取得
	/// </summary>
	/// <returns>現在の解放された実績のindexリスト</returns>
	public List<bool> GetIsClears()
	{
		return isClears;
	}

}
