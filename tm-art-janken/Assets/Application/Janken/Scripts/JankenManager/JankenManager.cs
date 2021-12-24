using System;
using UniRx;
using UnityEngine;
using JankenDefine;

public partial class JankenManager : MonoBehaviour
{

    private GameObject objMainManager = default;
    private MainManager mainManager = default;

    private static readonly JankenManagerStateStart jankenManagerStateStart = new JankenManagerStateStart();
    private static readonly JankenManagerStatePre jankenManagerStatePre = new JankenManagerStatePre();
    private static readonly JankenManagerStateDraw jankenManagerStateDraw = new JankenManagerStateDraw();
    private static readonly JankenManagerStateMain jankenManagerStateMain = new JankenManagerStateMain();
    private static readonly JankenManagerStateJudge jankenManagerStateJudge = new JankenManagerStateJudge();
    private static readonly JankenManagerStateUserWin jankenManagerStateUserWin = new JankenManagerStateUserWin();
    private static readonly JankenManagerStateUserLose jankenManagerStateUserLose = new JankenManagerStateUserLose();
    private static readonly JankenManagerStateEnd jankenManagerStateEnd = new JankenManagerStateEnd();

    // 現在の状態
    private JankenManagerStateBase currentState = jankenManagerStateStart;

    // あいこ通知
    public IObservable<Unit> OnJudgedDraw => onJudgedDraw;
    private readonly Subject<Unit> onJudgedDraw = new Subject<Unit>();

    // ユーザーの情報を管理するクラスの取得
    [SerializeField]
    private PlayerManager[] playerManager = new PlayerManager[2];

    // 0：ユーザー 1：キャラクターとして選択した手を保存
    [SerializeField]
    private JankenHand[] jankenHands = new JankenHand[2];

    // ユーザーとキャラクターが手を選択したら加算され2になると両方が選んだ通知を送る
    private int selectedFlag = 0;

    [SerializeField]
    private JankenCanvas jankenCanvas = default;

    private CharacterController characterController = default;

    private void Awake()
    {
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    private void OnStart()
    {

        objMainManager = GameObject.Find("MainManager");
        characterController = GameObject.FindWithTag("Character").GetComponent<CharacterController>();

        mainManager = objMainManager?.GetComponent<MainManager>();

        mainManager?.OnEnterJanken.Subscribe(a =>
        {
            ChangeState(jankenManagerStateStart);
            playerManager[(int)PlayerCategory.RIVAL].Init();
        });

        currentState.OnEnter(this, null);

        // ユーザーが手を選択した通知
        playerManager[(int)PlayerCategory.USER].OnHandSelected
            .Where(_ => selectedFlag < 2)
            .Subscribe(handNum =>
            {
                jankenHands[(int)PlayerCategory.USER] = (JankenHand)handNum;
                SetSelectedFlag();
            }).AddTo(this);

        // キャラクターが手を選択した通知
        playerManager[(int)PlayerCategory.RIVAL].OnHandSelected
            .Where(_ => selectedFlag < 2)
            .Subscribe(handNum =>
            {
                jankenHands[(int)PlayerCategory.RIVAL] = (JankenHand)handNum;
                SetSelectedFlag();
            }).AddTo(this);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void OnUpdate()
    {
        currentState.OnUpdate(this);
    }

    /// <summary>
    /// 選択した時にフラグを更新
    /// ユーザーとキャラクターの両方が手を選択したらじゃんけんの状態を勝敗判定に
    /// </summary>
    private void SetSelectedFlag()
    {
        selectedFlag++;

        if (selectedFlag >= 2)
        {
            selectedFlag = 0;
            ChangeState(jankenManagerStateMain);
        }
    }

    private void ChangeState(JankenManagerStateBase nextState)
    {
        currentState.OnExit(this, nextState);
        nextState.OnEnter(this, currentState);
        currentState = nextState;
    }

}
