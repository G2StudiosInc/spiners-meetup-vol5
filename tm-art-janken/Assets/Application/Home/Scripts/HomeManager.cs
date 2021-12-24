using UnityEngine;
using UnityEngine.UI;
using UniRx;

public partial class HomeManager : MonoBehaviour
{

    private MainManager mainManager = default;

    [SerializeField]
    private MainCameraManager mainCameraManager = default;

    [SerializeField]
    private Button btnStart = default;

    [SerializeField]
    private Button btnRecord = default;

    [SerializeField]
    private HomeCanvas homeCanvas = default;

    [SerializeField]
    private HomeBackground homeBackground = default;

    [SerializeField]
    private RecordCanvas recordCanvas = default;

    [SerializeField]
    private WinningStreakBatch winningStreakBatch = default;

    private static readonly HomeManagerStateHome homeManagerStateHome = new HomeManagerStateHome();
    private static readonly HomeManagerStateRecord homeManagerStateRecord = new HomeManagerStateRecord();

    // 現在の状態
    private HomeManagerStateBase currentState = homeManagerStateHome;

    private void Start()
    {
        mainManager = GameObject.Find("MainManager")?.GetComponent<MainManager>();
        mainCameraManager = GameObject.Find("Main Camera")?.GetComponent<MainCameraManager>();

        mainCameraManager?.OnCompleteEnterHome.Subscribe(_ =>
        {
            InitContents();
        });

        // 開始ボタンをタップした時の処理
        btnStart.OnClickAsObservable().Subscribe(_ =>
        {
            SoundController.Instance.PlaySE(SEName.SE_START_BATTLE);
            btnStart.enabled = false;

            homeCanvas.End().First().Subscribe(_ =>
            {
                // ステートを更新
                mainManager?.ChangeNextState();
            });

            homeBackground.End();
        }).AddTo(this);

        // 戦績を閉じ切った時
        recordCanvas.OnExitHomeRecord.Subscribe(_ =>
        {
            ChangeState(homeManagerStateHome);
        });

        // 戦績ボタンが押された時
        btnRecord.OnClickAsObservable().Subscribe(_ =>
        {
            SoundController.Instance.PlayVOICE(VOICEName.VO_ACHIEVEMENT_1);
            SoundController.Instance.PlaySE(SEName.SE_WINDOW_OPEN);
            ChangeState(homeManagerStateRecord);
            recordCanvas.ShowHomeRecord();
            homeCanvas.End();
            homeBackground.End();
        }).AddTo(this);
    }

    private void ChangeState(HomeManagerStateBase nextState)
    {
        currentState.OnExit(this, nextState);
        nextState.OnEnter(this, currentState);
        currentState = nextState;
    }

    private void InitContents()
    {
        btnStart.enabled = true;
        homeCanvas.Begin();
        homeBackground.Begin();
        winningStreakBatch.Init();
    }

}