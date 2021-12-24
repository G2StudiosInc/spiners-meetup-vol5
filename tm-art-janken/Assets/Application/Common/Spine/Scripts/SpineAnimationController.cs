using System;
using UnityEngine;
using UniRx;
using Spine.Unity;
using Spine;

public enum TrackIndex
{

	ID_LIP_SYNC = 10,
	ID_EYE_BLINK = 11,
	ID_HEAD_NOD = 12,
	ID_MAIN_TRACK = 100,

}

public enum SkinName
{

	NONE = 0,
	DEFAULT,
	SAD,
	SMILE_LARGE,
	SMILE_SMALL,

}

public class SpineAnimationController : MonoBehaviour
{

	private SkeletonAnimation skeletonAnimation = default;
	private Spine.AnimationState spineAnimationState = default;

	public IObservable<Unit> OnSpineCompleteMainTrack => onSpineCompleteMainTrack;
	private readonly Subject<Unit> onSpineCompleteMainTrack = new Subject<Unit>();

	private readonly string targetEventName = "OnPlayVoice";
	private readonly string targetEventNamePlayLipSync = "OnPlayLipSync";

	private void Start()
	{
		skeletonAnimation = GetComponent<SkeletonAnimation>();
		spineAnimationState = skeletonAnimation.AnimationState;
	}

	/// <summary>
	/// アニメーションの再生
	/// </summary>
	/// <param name="animName">再生するアニメーション名</param>
	/// <param name="trackId">再生するトラックのID</param>
	/// <param name="isLoop">ループするかどうか</param>
	/// <param name="mixDuration">アニメーション同士を混ぜて再生する時間</param>
	public void PlayAnimation(string animName, TrackIndex trackId = TrackIndex.ID_MAIN_TRACK, bool isLoop = true, float mixDuration = 0.2f)
	{
		PlayAnimation(animName, (int)trackId, isLoop, mixDuration);
	}

	/// <summary>
	/// 自動口パク再生
	/// </summary>
	/// <param name="playTime">再生時間</param>
	public void PlayAnimationAutoLipSync(float playTime)
	{
		PlayAutoLipSync(playTime);
	}

	/// <summary>
	/// アニメーションの再生
	/// TrackIDを直接数値で指定する
	/// </summary>
	/// <param name="animName">再生するアニメーション名</param>
	/// <param name="trackId">再生するトラックのID</param>
	/// <param name="isLoop">ループするかどうか</param>
	/// <param name="mixDuration"></param>
	private void PlayAnimation(string animName, int trackId = (int)TrackIndex.ID_MAIN_TRACK, bool isLoop = true, float mixDuration = 0.2f)
	{
		TrackEntry trackEntry = spineAnimationState.SetAnimation(trackId, animName, isLoop);
		trackEntry.MixDuration = mixDuration;

		// 各コールバックを設定
		trackEntry.Complete += OnSpineComplete;
		trackEntry.Interrupt += OnSpineInterrupt;
		trackEntry.Event += OnPlayVoice;
		trackEntry.Event += OnPlayLipSync;
	}

	/// <summary>
	/// スキンの設定
	/// </summary>
	/// <param name="skinName">スキン名をstringで指定</param>
	public void SetSkin(string skinName)
	{
		skeletonAnimation.skeleton.SetSkin(skinName);
		skeletonAnimation.skeleton.SetSlotsToSetupPose();
	}

	/// <summary>
	/// スキンの設定
	/// </summary>
	/// <param name="skinName">スキン名をSkinName(enum)で指定</param>
	public void SetSkin(SkinName skinName)
	{
		skeletonAnimation.skeleton.SetSkin(skeletonAnimation.Skeleton.Data.Skins.Items[(int)skinName]);
		skeletonAnimation.skeleton.SetSlotsToSetupPose();
	}

	/// <summary>
	/// アニメーション完了通知.
	/// </summary>
	/// <param name="trackEntry">完了したTrack情報</param>
	private void OnSpineComplete(TrackEntry trackEntry)
	{
		trackEntry.Complete -= OnSpineComplete;

		// アニメーションの完了通知は全てのトラックから来る為
		// TrackIndexで通知を出し分けている
		switch (trackEntry.TrackIndex)
		{
			case (int)TrackIndex.ID_MAIN_TRACK:
				onSpineCompleteMainTrack.OnNext(Unit.Default);

				break;
		}
	}

	/// <summary>
	/// アニメーションの割り込み発生時の処理
	/// mixDurationの関係からか、直前のアニメーションの完了通知がアニメーション再生中に邪魔をする問題がある
	/// </summary>
	/// <param name="trackEntry"></param>
	private void OnSpineInterrupt(TrackEntry trackEntry)
	{
		trackEntry.Complete -= OnSpineComplete;
		trackEntry.Interrupt -= OnSpineInterrupt;
	}

	/// <summary>
	/// Spineのキーに登録されたボイスを再生
	/// </summary>
	/// <param name="trackEntry"></param>
	/// <param name="e"></param>
	private void OnPlayVoice(Spine.TrackEntry trackEntry, Spine.Event e)
	{
		if (e.Data.Name != targetEventName) return;

		SoundController.Instance.PlayVOICE(e.String.ToUpper(), e.Int);
	}

	/// <summary>
	/// Spineのキーに登録されたタイミングで口パクを再生
	/// </summary>
	/// <param name="trackEntry"></param>
	/// <param name="e"></param>
	private void OnPlayLipSync(Spine.TrackEntry trackEntry, Spine.Event e)
	{
		if (e.Data.Name != targetEventNamePlayLipSync) return;
		PlayAutoLipSync(e.Float);
	}

	/// <summary>
	/// 自動口パク再生
	/// </summary>
	/// <param name="playTime">再生時間</param>
	private void PlayAutoLipSync(float playTime)
	{
		float interval = .25f;
		string animName = $"{CharaAnimType.janken}/{CharaAnimName.facial_lip_sync}";

		PlayAnimation(animName, TrackIndex.ID_LIP_SYNC, false);

		CompositeDisposable compositeDisposable = new CompositeDisposable();
		Observable.Timer(TimeSpan.FromSeconds(playTime)).Subscribe(_ =>
		{
			compositeDisposable.Dispose();
		}).AddTo(this);
		
		Observable.Interval(TimeSpan.FromSeconds(interval)).Subscribe(_ =>
		{
			PlayAnimation(animName, TrackIndex.ID_LIP_SYNC, false);
		}).AddTo(compositeDisposable);
	}

}