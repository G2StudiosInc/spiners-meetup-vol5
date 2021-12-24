using UniRx;
using UnityEngine;
using JankenDefine;

public partial class JankenManager
{

    [SerializeField]
    private JankenHandButtonManager jankenHandButtonManager = default;

    [SerializeField]
    private JankenCallTexts jankenCallTexts = default;

    [SerializeField]
    private JankenVSManager jankenVSManager = default;

    [SerializeField]
    private JankenResult jankenResult = default;

    private bool isDraw = false;

    /// <summary>
    /// じゃんけんシーン開始状態
    /// </summary>
    public class JankenManagerStateStart : JankenManagerStateBase
    {

        public override void OnEnter(JankenManager owner, JankenManagerStateBase prevState)
        {
            // キャラクターアニメーション再生
            // 開始モーション再生後にUI演出を再生する
            owner.characterController.PlayAnimJankenStart().First().Subscribe(_ =>
            {
                owner.jankenHandButtonManager.Begin();
                owner.jankenCanvas.Begin();
                owner.ChangeState(jankenManagerStatePre);
            });

            owner.isDraw = false;
            owner.jankenCallTexts.Initialize();
        }

    }

    /// <summary>
    /// じゃんけん掛け声状態
    /// </summary>
    public class JankenManagerStatePre : JankenManagerStateBase
    {

        public override void OnEnter(JankenManager owner, JankenManagerStateBase prevState)
        {
            owner.jankenCallTexts.Begin();
        }

    }

    /// <summary>
    /// 引き分け状態（最初の予備状態に送られてくるprevStateを使って統一するかも）
    /// </summary>
    public class JankenManagerStateDraw : JankenManagerStateBase
    {

        public override void OnEnter(JankenManager owner, JankenManagerStateBase prevState)
        {
            owner.isDraw = true;
            owner.onJudgedDraw.OnNext(Unit.Default);

            owner.jankenCallTexts.SetDraw();
            owner.jankenCallTexts.Begin();
            owner.jankenHandButtonManager.Begin();
            owner.mainManager.StateCallJankenDraw();

            // キャラクターアニメーション再生
            owner.characterController.PlayAnimJankenPre(owner.isDraw);
        }

    }

    /// <summary>
    /// じゃんけんを実際に行う状態
    /// </summary>
    public class JankenManagerStateMain : JankenManagerStateBase
    {

        public override void OnEnter(JankenManager owner, JankenManagerStateBase prevState)
        {
            SoundController.Instance.PlaySE(SEName.SE_JUDGE);

            owner.jankenCallTexts.Battle();
            owner.jankenVSManager.SetJankenIcons(owner.jankenHands[(int)PlayerCategory.USER], owner.jankenHands[(int)PlayerCategory.RIVAL]);
            owner.jankenVSManager.Begin();
            owner.mainManager.StateCallJankenJudge();

            // キャラクターアニメーション再生
            owner.characterController.PlayAnimJanken(owner.jankenHands[(int)PlayerCategory.RIVAL], owner.isDraw);

            Observable.Timer(System.TimeSpan.FromSeconds(1.5f)).Subscribe(_ =>
            {
                owner.ChangeState(jankenManagerStateJudge);
            });
        }

    }

    /// <summary>
    /// じゃんけんの勝敗判定状態
    /// </summary>
    public class JankenManagerStateJudge : JankenManagerStateBase
    {

        private readonly float delayWinLose = 0.5f;

        public override void OnEnter(JankenManager owner, JankenManagerStateBase prevState)
        {

            // ユーザーの選択した手を加算+セーブ
            SaveLoadManager.Instance.AddJankenHands(owner.jankenHands[(int)PlayerCategory.USER]);

            // あいこ
            if (owner.jankenHands[(int)PlayerCategory.USER] == owner.jankenHands[(int)PlayerCategory.RIVAL])
            {
                SaveLoadManager.Instance.AddDraw();
                // VSのUIを消す演出再生
                owner.jankenVSManager.End();
                owner.ChangeState(jankenManagerStateDraw);

                return;
            }

            owner.jankenCallTexts.End();

            switch (owner.jankenHands[(int)PlayerCategory.USER])
            {
                case JankenHand.GU:
                    switch (owner.jankenHands[(int)PlayerCategory.RIVAL])
                    {
                        case JankenHand.CHOKI: // ユーザー勝利
                            UserWin(owner);

                            break;
                        case JankenHand.PA: // ユーザー敗北
                            UserLose(owner);

                            break;
                    }

                    break;
                case JankenHand.CHOKI:
                    switch (owner.jankenHands[(int)PlayerCategory.RIVAL])
                    {
                        case JankenHand.PA: // ユーザー勝利
                            UserWin(owner);

                            break;
                        case JankenHand.GU: // ユーザー敗北
                            UserLose(owner);

                            break;
                    }

                    break;
                case JankenHand.PA:
                    switch (owner.jankenHands[(int)PlayerCategory.RIVAL])
                    {
                        case JankenHand.GU: // ユーザー勝利
                            UserWin(owner);

                            break;
                        case JankenHand.CHOKI: // ユーザー敗北
                            UserLose(owner);

                            break;
                    }

                    break;
            }
        }

        private void UserWin(JankenManager owner)
        {
            SaveLoadManager.Instance.AddWin();

            Observable.Timer(System.TimeSpan.FromSeconds(delayWinLose)).Subscribe(_ =>
            {
                // VSのUIを消す演出再生
                owner.jankenVSManager.End();

                SoundController.Instance.PlaySE(SEName.SE_RESULT_WIN);
                owner.jankenResult.Begin(JankenJudge.WIN);

                owner.ChangeState(jankenManagerStateUserWin);
            });
        }

        private void UserLose(JankenManager owner)
        {
            SaveLoadManager.Instance.AddLose();

            Observable.Timer(System.TimeSpan.FromSeconds(delayWinLose)).Subscribe(_ =>
            {
                // VSのUIを消す演出再生
                owner.jankenVSManager.End();

                SoundController.Instance.PlaySE(SEName.SE_RESULT_LOSE);
                owner.jankenResult.Begin(JankenJudge.LOSE);

                owner.ChangeState(jankenManagerStateUserLose);
            });
        }

    }

    /// <summary>
    /// ユーザー勝利状態
    /// </summary>
    public class JankenManagerStateUserWin : JankenManagerStateBase
    {

        public override void OnEnter(JankenManager owner, JankenManagerStateBase prevState)
        {
            owner.mainManager.StateCallJankenWin();

            owner.characterController.OnCompleteAnimJudgeIdle.First().Subscribe(_ =>
            {
                owner.ChangeState(jankenManagerStateEnd);
            });
        }

    }

    /// <summary>
    /// ユーザー敗北状態
    /// </summary>
    public class JankenManagerStateUserLose : JankenManagerStateBase
    {

        public override void OnEnter(JankenManager owner, JankenManagerStateBase prevState)
        {
            owner.mainManager.StateCallJankenLose();

            owner.characterController.OnCompleteAnimJudgeIdle.First().Subscribe(_ =>
            {
                owner.ChangeState(jankenManagerStateEnd);
            });
        }

    }

    /// <summary>
    /// じゃんけんの終了状態
    /// </summary>
    public class JankenManagerStateEnd : JankenManagerStateBase
    {

        public override void OnEnter(JankenManager owner, JankenManagerStateBase prevState)
        {
            // じゃんけんリザルトの表示終了演出
            owner.jankenResult.End().First().Subscribe(_ =>
            {
                owner.jankenCanvas.End();
                owner.mainManager.ChangeNextState();
            });
        }

    }

}
