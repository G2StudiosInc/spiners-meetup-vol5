using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BackgroundStar : MonoBehaviour
{

	[SerializeField]
	private MainManager mainManager = default;

	public enum LineColorName
	{

		PINK = 0,
		GRAY,
		BLUE,
		YELLOW

	}

	[SerializeField]
	private Color[] lineColors = new Color[4];

	[SerializeField]
	private SpriteRenderer[] lineSprites = new SpriteRenderer[2];

	private readonly float speed = 0.03f;
	private float respawnMinX = -15;
	private float respawnMaxX = 0;

	private readonly float respawnMinXHome = -15;
	private readonly float respawnMaxXHome = 0;
	private readonly float respawnMinXLose = -4;
	private readonly float respawnMaxXLose = 4;
	private readonly float respawnY = -10;

	private readonly float duration = 0.3f;

	private readonly CompositeDisposable compositeDisposable = new CompositeDisposable();

	private void Start()
	{
		// ホーム遷移時
		mainManager.OnEnterHome.Subscribe(_ =>
		{
			respawnMinX = respawnMinXHome;
			respawnMaxX = respawnMaxXHome;
			SetColor(LineColorName.PINK);
			SetAngle(-45f);
		}).AddTo(compositeDisposable);

		// じゃんけん遷移時
		mainManager.OnEnterJanken.Subscribe(_ =>
		{
			SetColor(LineColorName.GRAY);
		}).AddTo(compositeDisposable);

		// じゃんけん勝利遷移時
		mainManager.OnEnterJankenWin.Subscribe(_ =>
		{
			SetColor(LineColorName.PINK);
		}).AddTo(compositeDisposable);

		// じゃんけん敗北遷移時
		mainManager.OnEnterJankenLose.Subscribe(_ =>
		{
			respawnMinX = respawnMinXLose;
			respawnMaxX = respawnMaxXLose;
			SetColor(LineColorName.BLUE);
			SetAngle(-90f);
		}).AddTo(compositeDisposable);

		// ホーム戦績実績遷移時
		mainManager.OnEnterHomeRecord.Subscribe(_ =>
		{
			SetColor(LineColorName.YELLOW);
		}).AddTo(compositeDisposable);

		// 毎フレーム更新処理
		this.UpdateAsObservable().Subscribe(_ =>
		{
			this.transform.Translate(speed, 0f, 0f);
		}).AddTo(this);

		// 星が下端に到達した時
		this.ObserveEveryValueChanged(_ => _.transform.position.y)
			.Where(_ => _ < respawnY)
			.Subscribe(_ =>
			{
				this.transform.localPosition = new Vector3(UnityEngine.Random.Range(respawnMinX, respawnMaxX), 10f, 0f);
				this.transform.localScale = Vector3.one * (UnityEngine.Random.value * 0.1f + 1f);
			}).AddTo(this);
	}

	/// <summary>
	/// 線の色替え設定
	/// </summary>
	/// <param name="bgColorName"></param>
	/// <returns></returns>
	public void SetColor(LineColorName bgColorName = LineColorName.PINK)
	{
		Sequence sequence = DOTween.Sequence().SetAutoKill();

		sequence.Append(lineSprites[0].DOColor(lineColors[(int)bgColorName], duration));
		sequence.Join(lineSprites[1].DOColor(lineColors[(int)bgColorName], duration));
	}

	/// <summary>
	/// 星の角度を設定
	/// </summary>
	/// <param name="angle"></param>
	/// <returns></returns>
	private void SetAngle(float angle = -45f)
	{
		Sequence sequence = DOTween.Sequence().SetAutoKill();

		sequence.Append(this.transform.DORotate(Vector3.forward * angle, duration));
	}

	/// <summary>
	/// ゲームオブジェクトを削除した時にmainManagerの行動を停止する
	/// </summary>
	private void OnDestroy()
	{
		compositeDisposable.Dispose();
	}

}
