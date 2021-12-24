public class BattleRecordManager
{

	public class BattleRecordSaveData
	{

		public int wins;
		public int loses;
		public int draws;
		public int winningStreakNow;
		public int winningStreakBest;
		public readonly int[] jankenHandsSelect;

		public BattleRecordSaveData(int winsValue, int losesValue, int drawsValue, int winningStreakNowValue, int winningStreakBestValue, int[] jankenHandsSelectValue)
		{
			wins = winsValue;
			loses = losesValue;
			draws = drawsValue;
			winningStreakNow = winningStreakNowValue;
			winningStreakBest = winningStreakBestValue;
			jankenHandsSelect = jankenHandsSelectValue;
		}

	}

	private readonly float[] jankenHandsRate = new float[3]; // じゃんけんの手の選択率

	private BattleRecordSaveData battleRecordSaveData = new BattleRecordSaveData(0, 0, 0, 0, 0, new int[] { 0, 0, 0 });

	public void Init()
	{
		battleRecordSaveData = new BattleRecordSaveData(
			0, 0, 0, 0, 0, new int[] { 0, 0, 0 });

		for (int i = 0; i < jankenHandsRate.Length; i++)
		{
			jankenHandsRate[i] = 0;
		}
	}

	public void Reset()
	{
		battleRecordSaveData.wins = 0;
		battleRecordSaveData.loses = 0;
		battleRecordSaveData.draws = 0;
		battleRecordSaveData.winningStreakNow = 0;
		battleRecordSaveData.winningStreakBest = 0;

		for (int i = 0; i < battleRecordSaveData.jankenHandsSelect.Length; i++)
		{
			battleRecordSaveData.jankenHandsSelect[i] = 0;
		}

		for (int i = 0; i < jankenHandsRate.Length; i++)
		{
			jankenHandsRate[i] = 0;
		}
	}

	public void SetBattleRecordSaveData(BattleRecordSaveData battleRecordSaveData)
	{
		this.battleRecordSaveData = battleRecordSaveData;
	}

	public BattleRecordSaveData GetBattleRecordSaveData()
	{
		return battleRecordSaveData;
	}

	public int GetBattleRecordWinningStreakNow()
	{
		return battleRecordSaveData.winningStreakNow;
	}

	public void AddWin()
	{
		battleRecordSaveData.winningStreakNow++;

		// 最高連勝数更新
		if (battleRecordSaveData.winningStreakBest < battleRecordSaveData.winningStreakNow)
		{
			battleRecordSaveData.winningStreakBest = battleRecordSaveData.winningStreakNow;
		}

		battleRecordSaveData.wins++;
	}

	public void AddLose()
	{
		battleRecordSaveData.winningStreakNow = 0;
		battleRecordSaveData.loses++;
	}

	public void AddDraw()
	{
		battleRecordSaveData.draws++;
	}

	public void AddJankenHands(JankenDefine.JankenHand jankenHand)
	{
		battleRecordSaveData.jankenHandsSelect[(int)jankenHand]++;
	}

}