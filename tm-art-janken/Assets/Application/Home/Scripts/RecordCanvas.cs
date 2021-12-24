using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RecordCanvas : CanvasBase
{

    [SerializeField]
    private CanvasGroup group = default;

    [SerializeField]
    private Button btnBackground = default;

    [SerializeField]
    private BattleRecord battleRecord = default;

    [SerializeField]
    private CanvasGroup groupAchievement = default;

    [SerializeField]
    private CanvasGroup groupBattleRecord = default;

    [SerializeField]
    private Button[] btnTabs = new Button[2];

    [SerializeField]
    private CanvasGroup[] groupAchievementTabs = new CanvasGroup[2];

    [SerializeField]
    private CanvasGroup[] groupBattleRecordTabs = new CanvasGroup[2];

    [SerializeField]
    private AchievementUIs achievementUIs = default;

    private readonly Subject<Unit> onExitHomeRecord = new Subject<Unit>();
    public IObservable<Unit> OnExitHomeRecord => onExitHomeRecord;

    protected override void Start()
    {
        base.Start();

        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;

        btnBackground.OnClickAsObservable().Subscribe(_ =>
        {
            SoundController.Instance.PlaySE(SEName.SE_WINDOW_CLOSE);
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
            onExitHomeRecord.OnNext(Unit.Default);
        });

        btnTabs[0].OnClickAsObservable().Where(_ => groupBattleRecord.alpha == 0f).Subscribe(_ =>
        {
            SoundController.Instance.PlaySE(SEName.SE_COMMON_TAP);
            EnableBattleRecord();
        });

        btnTabs[1].OnClickAsObservable().Where(_ => groupAchievement.alpha == 0f).Subscribe(_ =>
        {
            SoundController.Instance.PlaySE(SEName.SE_COMMON_TAP);
            EnableAchievement();
        });

        EnableBattleRecord();
    }

    public void ShowHomeRecord()
    {
        battleRecord.Init();
        achievementUIs.Init();

        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    /// <summary>
    /// 実績の表示
    /// </summary>
    public void EnableAchievement()
    {
        groupAchievement.alpha = 1;
        groupBattleRecord.alpha = 0;
        groupBattleRecord.blocksRaycasts = false;

        groupAchievementTabs[0].alpha = 1;
        groupAchievementTabs[1].alpha = 0;

        groupBattleRecordTabs[0].alpha = 0;
        groupBattleRecordTabs[1].alpha = 1;
    }

    /// <summary>
    /// 戦績の表示
    /// </summary>
    private void EnableBattleRecord()
    {
        groupAchievement.alpha = 0;
        groupBattleRecord.alpha = 1;
        groupBattleRecord.blocksRaycasts = true;

        groupAchievementTabs[0].alpha = 0;
        groupAchievementTabs[1].alpha = 1;

        groupBattleRecordTabs[0].alpha = 1;
        groupBattleRecordTabs[1].alpha = 0;
    }

}
