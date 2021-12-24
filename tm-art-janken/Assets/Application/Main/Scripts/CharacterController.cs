using System;
using System.Collections;
using UnityEngine;
using UniRx;

public class CharacterController : MonoBehaviour
{

	[SerializeField]
	private MainManager mainManager = default;

	private MainCameraManager mainCameraManager = default;

	[SerializeField]
	private SpineAnimationController spineAnimationController = default;

	private readonly CompositeDisposable compositeDisposableMainTrack = new CompositeDisposable();

	private readonly Subject<Unit> onCompleteAnim = new Subject<Unit>();

	private readonly Subject<Unit> onCompleteAnimStart = new Subject<Unit>();

	public IObservable<Unit> OnCompleteAnimJudgeIdle => onCompleteAnimJudgeIdle;
	private readonly Subject<Unit> onCompleteAnimJudgeIdle = new Subject<Unit>();

	[SerializeField]
	private bool isEyeBlink = false;

	private readonly float eyeBlinkTimerMin = 6f;
	private readonly float eyeBlinkTimerMax = 12f;
	private float eyeBlinkInterval = default;

	[SerializeField]
	private bool isHeadNod = false;

	private readonly float headNodTimerMin = 60f;
	private readonly float headNodTimerMax = 90f;
	private float headNodInterval = default;

	private readonly float mixDurationSadIdle = 0.55f;
	private readonly float mixDurationJoyIdle = 0.5f;
	private readonly float mixDurationIdleActionIdle = 0.5f;

	private bool isDraw = false;

	private IObservable<Unit> OnCompleteEmotionSad => onCompleteEmotionSad;
	private readonly Subject<Unit> onCompleteEmotionSad = new Subject<Unit>();

	private IObservable<Unit> OnCompleteEmotionJoyLarge => onCompleteEmotionJoyLarge;
	private readonly Subject<Unit> onCompleteEmotionJoyLarge = new Subject<Unit>();

	private IObservable<Unit> OnCompleteEmotionJoySmall => onCompleteEmotionJoySmall;
	private readonly Subject<Unit> onCompleteEmotionJoySmall = new Subject<Unit>();

	private void Start()
	{
		mainCameraManager = GameObject.Find("Main Camera").GetComponent<MainCameraManager>();

		// タイトル遷移でカメラ移動終了時に手を振るアニメーション再生
		mainCameraManager.OnCompleteEnterHome.First().Subscribe(_ =>
		{
			PlayAnim(CharaAnimType.janken, CharaAnimName.wave_hand_left, false, TrackIndex.ID_MAIN_TRACK, 0.7f).First().Subscribe(_ =>
			{
				PlayAnimIdle(0);
			}).AddTo(compositeDisposableMainTrack);
		});

		mainManager?.OnEnterHomeRecordEnd.Subscribe(_ =>
		{
			PlayIdleAction();
		});

		mainManager?.OnEnterHomeRecord.Subscribe(_ =>
		{
			spineAnimationController.PlayAnimationAutoLipSync(0.5f);
		});

		// キャラクターが負け（ユーザーの勝ち）
		mainManager?.OnEnterJankenWin.Subscribe(_ =>
		{
			spineAnimationController.SetSkin(SkinName.DEFAULT);

			PlayAnim(CharaAnimType.janken, CharaAnimName.emotion_sad, false, TrackIndex.ID_MAIN_TRACK, 0).First().Subscribe(_ =>
			{
				onCompleteEmotionSad.OnNext(Unit.Default);
			}).AddTo(compositeDisposableMainTrack);
		});

		// emotion_sad再生完了後にemotion_sad_idleを再生させる
		OnCompleteEmotionSad.Subscribe(_ =>
		{
			PlayAnim(CharaAnimType.janken, CharaAnimName.emotion_sad_idle, false, TrackIndex.ID_MAIN_TRACK, 0).First().Subscribe(_ =>
			{
				onCompleteAnimJudgeIdle.OnNext(Unit.Default);
				PlayAnimIdle(mixDurationSadIdle);
			});
		});

		// キャラクターが勝ち（ユーザーの負け）
		mainManager?.OnEnterJankenLose.Subscribe(_ =>
		{
			spineAnimationController.SetSkin(SkinName.DEFAULT);

			if (isDraw)
			{
				PlayAnim(CharaAnimType.janken, CharaAnimName.emotion_joy_small, false, TrackIndex.ID_MAIN_TRACK, 0).First().Subscribe(_ =>
				{
					onCompleteEmotionJoySmall.OnNext(Unit.Default);
				}).AddTo(compositeDisposableMainTrack);
			}
			else
			{
				PlayAnim(CharaAnimType.janken, CharaAnimName.emotion_joy_large, false, TrackIndex.ID_MAIN_TRACK, 0).First().Subscribe(_ =>
				{
					onCompleteEmotionJoyLarge.OnNext(Unit.Default);
				}).AddTo(compositeDisposableMainTrack);
			}

		});

		// emotion_joy_small再生完了後にemotion_joy_small_idleを再生させる
		OnCompleteEmotionJoySmall.Subscribe(_ =>
		{
			PlayAnim(CharaAnimType.janken, CharaAnimName.emotion_joy_small_idle, false, TrackIndex.ID_MAIN_TRACK, 0).First().Subscribe(_ =>
			{
				onCompleteAnimJudgeIdle.OnNext(Unit.Default);
				PlayAnimIdle(mixDurationJoyIdle);
			});
		});

		// emotion_joy_large再生完了後にemotion_joy_large_idleを再生させる
		OnCompleteEmotionJoyLarge.Subscribe(_ =>
		{
			PlayAnim(CharaAnimType.janken, CharaAnimName.emotion_joy_large_idle, false, TrackIndex.ID_MAIN_TRACK, 0).First().Subscribe(_ =>
			{
				onCompleteAnimJudgeIdle.OnNext(Unit.Default);
				PlayAnimIdle(mixDurationJoyIdle);
			});
		});

		// じゃんけん中
		mainManager?.OnEnterJanken.Subscribe(_ =>
		{
			isDraw = false;
			spineAnimationController.SetSkin(SkinName.SMILE_SMALL);
		});

		spineAnimationController.OnSpineCompleteMainTrack.Subscribe(_ =>
		{
			onCompleteAnim.OnNext(Unit.Default);
		});

		StartCoroutine(PlayEyeBlink());
		StartCoroutine(PlayHeadNod());

	}

	/// <summary>
	/// 不定期に再生する目パチ
	/// </summary>
	/// <returns></returns>
	private IEnumerator PlayEyeBlink()
	{
		while (true)
		{
			eyeBlinkInterval = UnityEngine.Random.Range(eyeBlinkTimerMin, eyeBlinkTimerMax);

			yield return new WaitForSeconds(eyeBlinkInterval);

			if (isEyeBlink)
				PlayAnim(CharaAnimType.janken, CharaAnimName.facial_eye_blink_B, false, TrackIndex.ID_EYE_BLINK);
		}
	}

	private void PlayIdleAction()
	{
		PlayAnim(CharaAnimType.janken, CharaAnimName.idle_action_01).First().Subscribe(_ =>
		{
			PlayAnimIdle(mixDurationIdleActionIdle);
		});
	}

	/// <summary>
	/// 待機モーション中に不定期で再生するアクション
	/// </summary>
	/// <returns></returns>
	private IEnumerator PlayHeadNod()
	{
		while (true)
		{
			headNodInterval = UnityEngine.Random.Range(headNodTimerMin, headNodTimerMax);

			yield return new WaitForSeconds(headNodInterval);

			Debug.Log("HeadNod");

			if (isHeadNod)
				PlayAnim(CharaAnimType.janken, CharaAnimName.head_nod, false, TrackIndex.ID_HEAD_NOD);
		}
	}
	
	private void PlayAnimIdle(float mixDuration = 0.2f)
	{
		PlayAnim(CharaAnimType.janken, CharaAnimName.idle, true, TrackIndex.ID_MAIN_TRACK, mixDuration);
	}

	/// <summary>
	/// じゃんけんの開始時のアクション
	/// </summary>
	public IObservable<Unit> PlayAnimJankenStart()
	{
		PlayAnim(CharaAnimType.janken, CharaAnimName.janken_start).First().Subscribe(_ =>
		{
			onCompleteAnimStart.OnNext(Unit.Default);
			PlayAnimJankenPre();
		}).AddTo(compositeDisposableMainTrack);

		return onCompleteAnimStart;
	}

	/// <summary>
	/// じゃんけんの手を出す前の振りかぶり
	/// </summary>
	public void PlayAnimJankenPre(bool isDraw = false)
	{
		this.isDraw = isDraw;
		CharaAnimName preName = isDraw ? CharaAnimName.janken_pre_draw : CharaAnimName.janken_pre;

		PlayAnim(CharaAnimType.janken, preName).First().Subscribe(_ =>
		{
			PlayAnimJankenPreLoop();
		}).AddTo(compositeDisposableMainTrack);
	}

	/// <summary>
	/// じゃんけんの手を出す前の振りかぶり後のループ
	/// </summary>
	private void PlayAnimJankenPreLoop()
	{
		PlayAnim(CharaAnimType.janken, CharaAnimName.janken_pre_loop, true, TrackIndex.ID_MAIN_TRACK, 1.5f).First().Subscribe(_ => { }).AddTo(compositeDisposableMainTrack);
	}

	/// <summary>
	/// じゃんけんの手を出すアニメーション再生
	/// </summary>
	/// <param name="jankenHand">グーチョキパーのいずれか</param>
	public void PlayAnimJanken(JankenDefine.JankenHand jankenHand, bool isDraw = false)
	{
		CharaAnimName[] charaAnimNames = new CharaAnimName[2];

		switch (jankenHand)
		{
			case JankenDefine.JankenHand.GU:
				charaAnimNames[0] = isDraw ? CharaAnimName.janken_gu_draw : CharaAnimName.janken_gu;
				charaAnimNames[1] = CharaAnimName.janken_gu_idle;

				break;
			case JankenDefine.JankenHand.CHOKI:
				charaAnimNames[0] = isDraw ? CharaAnimName.janken_choki_draw : CharaAnimName.janken_choki;
				charaAnimNames[1] = CharaAnimName.janken_choki_idle;

				break;
			case JankenDefine.JankenHand.PA:
				charaAnimNames[0] = isDraw ? CharaAnimName.janken_pa_draw : CharaAnimName.janken_pa;
				charaAnimNames[1] = CharaAnimName.janken_pa_idle;

				break;
		}

		PlayAnim(CharaAnimType.janken, charaAnimNames[0], false, TrackIndex.ID_MAIN_TRACK, 0).First().Subscribe(_ =>
		{
			PlayAnim(CharaAnimType.janken, charaAnimNames[1], false, TrackIndex.ID_MAIN_TRACK, 0);
		});
	}

	private IObservable<Unit> PlayAnim(CharaAnimType charaAnimType, CharaAnimName charaAnimName, bool isLoop = false, TrackIndex trackID = TrackIndex.ID_MAIN_TRACK, float mixDuration = 0.2f)
	{
		string animName = $"{charaAnimType}/{charaAnimName}";
		spineAnimationController.PlayAnimation(animName, trackID, isLoop, mixDuration);

		return onCompleteAnim;
	}

}
