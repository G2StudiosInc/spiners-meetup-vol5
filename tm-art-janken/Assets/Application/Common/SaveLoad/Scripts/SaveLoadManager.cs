using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{

    private static SaveLoadManager instance;

    public static SaveLoadManager Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType<SaveLoadManager>();
            DontDestroyOnLoad(instance.gameObject);

            return instance;
        }
    }

    private static readonly string keySaveLoadBattleRecord = "BattleRecord";
    private static readonly string keySaveLoadAchievement = "Achievement";

    private readonly BattleRecordManager battleRecordManager = new BattleRecordManager();
    private readonly AchievementManager achievementManager = new AchievementManager();

    private readonly AchievementClearChecker achievementClearChecker = new AchievementClearChecker();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this == instance) return;

            Destroy(gameObject);
        }

        achievementManager.Init();
        battleRecordManager.Init();

        LoadBattleRecord();
        LoadAchievement();

        achievementClearChecker.Init();
    }

    /// <summary>
    /// 戦績のデータをロード
    /// 勝利数など必要なデータのみ取得し、総戦闘回数などはそれらを計算して出す
    /// </summary>
    public void LoadBattleRecord()
    {
        if (!PlayerPrefs.HasKey(keySaveLoadBattleRecord)) return;

        string loadData = PlayerPrefs.GetString(keySaveLoadBattleRecord);
        string[] loadDataList = loadData.Split(',');

        int wins = Int32.Parse(loadDataList[0]);
        int loses = Int32.Parse(loadDataList[1]);
        int draws = Int32.Parse(loadDataList[2]);
        int winningStreakNow = Int32.Parse(loadDataList[3]);
        int winningStreakBest = Int32.Parse(loadDataList[4]);

        int[] jankenHands = new int[3]
        {
            Int32.Parse(loadDataList[5]),
            Int32.Parse(loadDataList[6]),
            Int32.Parse(loadDataList[7]),
        };

        BattleRecordManager.BattleRecordSaveData battleRecord = new BattleRecordManager.BattleRecordSaveData(
            wins, loses, draws, winningStreakNow, winningStreakBest, jankenHands);

        battleRecordManager.SetBattleRecordSaveData(battleRecord);
    }

    /// <summary>
    /// 実績のデータをロード
    /// 解放されたものを取得し、内容はAchievementManagerから取得する
    /// </summary>
    public void LoadAchievement()
    {
        if (!PlayerPrefs.HasKey(keySaveLoadAchievement)) return;

        // 解放された実績の文字列を取得し、配列化、"1"になっているものを解放済みとして扱う
        string unlockSaveData = PlayerPrefs.GetString(keySaveLoadAchievement);
        string[] unlockSaveDataList = unlockSaveData.Split(',');
        List<bool> isLocks = new List<bool>();

        for (int i = 0; i < unlockSaveDataList.Length; i++)
        {
            isLocks.Add((unlockSaveDataList[i] == "1") ? true : false);
        }

        // AchievementManagerのisLocksに値を入れ込む
        achievementManager.SetIsLocks(isLocks);
    }

    /// <summary>
    /// 戦績データのセーブ
    /// BattleRecordManagerからセーブするBattleRecordSaveDataデータを取得してセーブ実行
    /// </summary>
    public void SaveBattleRecord()
    {
        BattleRecordManager.BattleRecordSaveData battleRecordSaveData = battleRecordManager.GetBattleRecordSaveData();

        string saveData =
            $"{battleRecordSaveData.wins}," +
            $"{battleRecordSaveData.loses}," +
            $"{battleRecordSaveData.draws}," +
            $"{battleRecordSaveData.winningStreakNow}," +
            $"{battleRecordSaveData.winningStreakBest}," +
            $"{battleRecordSaveData.jankenHandsSelect[(int)JankenDefine.JankenHand.GU]}," +
            $"{battleRecordSaveData.jankenHandsSelect[(int)JankenDefine.JankenHand.CHOKI]}," +
            $"{battleRecordSaveData.jankenHandsSelect[(int)JankenDefine.JankenHand.PA]}";

        PlayerPrefs.SetString(keySaveLoadBattleRecord, saveData);
    }

    /// <summary>
    /// 実績データのセーブ
    /// AchievementManagerからセーブするstringデータを取得してセーブ実行
    /// </summary>
    public void SaveAchievement()
    {
        string unlockSaveData = String.Empty;

        foreach (var isClear in achievementManager.GetIsClears())
        {
            if (unlockSaveData != String.Empty) unlockSaveData += ",";
            unlockSaveData += isClear ? "1" : "0";
        }

        PlayerPrefs.SetString(keySaveLoadAchievement, unlockSaveData);
    }

    /// <summary>
    /// 戦績・実績全てのデータをリセットしてセーブデータを上書き
    /// </summary>
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        battleRecordManager.Reset();
        SaveBattleRecord();
        achievementManager.Reset();
        SaveAchievement();
    }

    public void AddWin()
    {
        battleRecordManager.AddWin();
        SaveBattleRecord();
        achievementClearChecker.ClearCheck();
    }

    public void AddLose()
    {
        battleRecordManager.AddLose();
        SaveBattleRecord();
        achievementClearChecker.ClearCheck();
    }

    public void AddDraw()
    {
        battleRecordManager.AddDraw();
        SaveBattleRecord();
    }

    public void AddJankenHands(JankenDefine.JankenHand jankenHand)
    {
        battleRecordManager.AddJankenHands(jankenHand);
    }

    public BattleRecordManager.BattleRecordSaveData GetBattleRecordSaveData()
    {
        return battleRecordManager.GetBattleRecordSaveData();
    }

    public int GetBattleRecordWinningStreakNow()
    {
        return battleRecordManager.GetBattleRecordWinningStreakNow();
    }

    /// <summary>
    /// 任意の実績を解放する
    /// </summary>
    /// <param name="achievementName">解放される実績名</param>
    public void SetAchievementClear(AchievementName achievementName)
    {
        achievementManager.SetIsClear(achievementName);
        SaveAchievement();
    }

    /// <summary>
    /// 任意の実績の解放状況を把握する
    /// </summary>
    /// <param name="achievementName"></param>
    /// <returns></returns>
    public bool GetAchievementClear(AchievementName achievementName)
    {
        return achievementManager.GetIsClear(achievementName);
    }

    /// <summary>
    /// 全実績の解放状況のリストを取得
    /// </summary>
    /// <returns></returns>
    public List<bool> GetAchievementIsClears()
    {
        return achievementManager.GetIsClears();
    }

    /// <summary>
    /// 実績の表示に必要なAchievementDataクラスを取得
    /// </summary>
    /// <returns></returns>
    public List<AchievementManager.AchievementData> GetAchievementDataList()
    {
        return achievementManager.GetAchievementDataList();
    }

}