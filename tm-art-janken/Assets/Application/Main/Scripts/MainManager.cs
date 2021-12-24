using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public partial class MainManager : MonoBehaviour
{

    private static readonly MainManagerStateTitle mainManagerStateTitle = new MainManagerStateTitle();
    private static readonly MainManagerStateHome mainManagerStateHome = new MainManagerStateHome();
    private static readonly MainManagerStateJanken mainManagerStateJanken = new MainManagerStateJanken();

    private MainManagerStateBase currentState = mainManagerStateTitle;

    private static AsyncOperation asyncSceneTitle = default;
    private static AsyncOperation asyncSceneHome = default;
    private static AsyncOperation asyncSceneJanken = default;

    public IObservable<Unit> OnEnterHome => onEnterHome;
    private readonly Subject<Unit> onEnterHome = new Subject<Unit>();

    public IObservable<Unit> OnEnterHomeRecord => onEnterHomeRecord;
    private readonly Subject<Unit> onEnterHomeRecord = new Subject<Unit>();

    public IObservable<Unit> OnEnterHomeRecordEnd => onEnterHomeRecordEnd;
    private readonly Subject<Unit> onEnterHomeRecordEnd = new Subject<Unit>();

    public IObservable<Unit> OnEnterJanken => onEnterJanken;
    private readonly Subject<Unit> onEnterJanken = new Subject<Unit>();

    public IObservable<Unit> OnEnterJankenJudge => onEnterJankenJudge;
    private readonly Subject<Unit> onEnterJankenJudge = new Subject<Unit>();

    public IObservable<Unit> OnEnterJankenDraw => onEnterJankenDraw;
    private readonly Subject<Unit> onEnterJankenDraw = new Subject<Unit>();

    public IObservable<Unit> OnEnterJankenWin => onEnterJankenWin;
    private readonly Subject<Unit> onEnterJankenWin = new Subject<Unit>();

    public IObservable<Unit> OnEnterJankenLose => onEnterJankenLose;
    private readonly Subject<Unit> onEnterJankenLose = new Subject<Unit>();

    [SerializeField]
    private GameObject objBlackMaskCanvas = default;

    private void Awake()
    {
        OnStart();

        // 横画面用の黒帯を表示させる
        objBlackMaskCanvas.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private void Update()
    {
        OnUpdate();
    }

    private void OnStart()
    {
        currentState.OnEnter(this, null);

    }

    private void OnUpdate()
    {
        currentState.OnUpdate(this);
    }

    /// <summary>
    /// じゃんけんの状態が判定に切り替わった時の通知処理
    /// </summary>
    public void StateCallJankenJudge()
    {
        onEnterJankenJudge.OnNext(Unit.Default);
    }

    /// <summary>
    /// じゃんけんの状態があいこに切り替わった時の通知処理
    /// </summary>
    public void StateCallJankenDraw()
    {
        onEnterJankenDraw.OnNext(Unit.Default);
    }

    /// <summary>
    /// じゃんけんの状態がユーザー勝利に切り替わった時の通知処理
    /// </summary>
    public void StateCallJankenWin()
    {
        onEnterJankenWin.OnNext(Unit.Default);
    }

    /// <summary>
    /// じゃんけんの状態がユーザー敗北に切り替わった時の通知処理
    /// </summary>
    public void StateCallJankenLose()
    {
        onEnterJankenLose.OnNext(Unit.Default);
    }

    /// <summary>
    /// ホームの状態が実績を表示に切り替わった時の通知処理
    /// </summary>
    public void StateCallHomeRecord()
    {
        onEnterHomeRecord.OnNext(Unit.Default);
    }

    /// <summary>
    /// ホームの状態が実績の表示終了時の通知処理
    /// </summary>
    public void StateCallHomeRecordEnd()
    {
        onEnterHomeRecordEnd.OnNext(Unit.Default);
    }

    /// <summary>
    /// ホームの状態が実績から戻った時の通知処理
    /// </summary>
    public void StateCallHome()
    {
        onEnterHome.OnNext(Unit.Default);
    }

    /// <summary>
    /// 状態の次の状態に移行する
    /// 次の状態は現在の状態から決め打ちで指定する
    /// </summary>
    public void ChangeNextState()
    {
        if (currentState == mainManagerStateTitle)
        {
            ChangeState(mainManagerStateHome);
        }
        else if (currentState == mainManagerStateHome)
        {
            ChangeState(mainManagerStateJanken);
        }
        else if (currentState == mainManagerStateJanken)
        {
            ChangeState(mainManagerStateHome);
        }
    }

    /// <summary>
    /// 状態を変更する
    /// </summary>
    /// <param name="nextState"></param>
    private void ChangeState(MainManagerStateBase nextState)
    {
        currentState.OnExit(this, nextState);
        nextState.OnEnter(this, currentState);
        currentState = nextState;
    }

    private IEnumerator LoadScene()
    {

        asyncSceneTitle = SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
        asyncSceneTitle.allowSceneActivation = false;

        asyncSceneHome = SceneManager.LoadSceneAsync("Home", LoadSceneMode.Additive);
        asyncSceneHome.allowSceneActivation = false;

        yield return new WaitForSeconds(1);

        // Titleシーンをアクティブにする
        asyncSceneTitle.allowSceneActivation = true;
        ChangeState(mainManagerStateTitle);
    }

}
