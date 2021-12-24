using System.Collections.Generic;

public class AchievementClearChecker
{

	private AchievementName achievementName = AchievementName.ACHV_01_FIRST_WIN;
	private List<AchievementManager.AchievementData> achievementDataList = new List<AchievementManager.AchievementData>();
	private List<bool> isClears = new List<bool>();
	private BattleRecordManager.BattleRecordSaveData battleRecordSaveData = default;

	public void Init()
	{
		achievementDataList = SaveLoadManager.Instance.GetAchievementDataList();
		isClears = SaveLoadManager.Instance.GetAchievementIsClears();
		battleRecordSaveData = SaveLoadManager.Instance.GetBattleRecordSaveData();
	}

	public void ClearCheck()
	{

		achievementName = AchievementName.ACHV_01_FIRST_WIN;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.wins == 1)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_02_FIRST_LOSE;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.loses == 1)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_03_WIN_005;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.wins == 5)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_04_WIN_010;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.wins == 10)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_05_WIN_025;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.wins == 25)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_06_WIN_050;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.wins == 50)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_07_WIN_100;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.wins == 100)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_08_WINNING_STREAK_03;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.winningStreakBest == 3)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_09_WINNING_STREAK_05;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.winningStreakBest == 5)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_10_WINNING_STREAK_10;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.winningStreakBest == 10)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_11_WINNING_STREAK_15;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.winningStreakBest == 15)
			{
				AchievementClear(achievementName);
			}
		}

		achievementName = AchievementName.ACHV_12_WINNING_STREAK_20;

		if (!isClears[(int)achievementName])
		{
			if (battleRecordSaveData.winningStreakBest == 20)
			{
				AchievementClear(achievementName);
			}
		}

	}

	private void AchievementClear(AchievementName name)
	{
		SoundController.Instance.PlaySE(SEName.SE_ACHIEVEMENT);
		SaveLoadManager.Instance.SetAchievementClear(name);
		AchievementWindowCanvas.Instance.Run(achievementDataList[(int)name].title);
	}

}
