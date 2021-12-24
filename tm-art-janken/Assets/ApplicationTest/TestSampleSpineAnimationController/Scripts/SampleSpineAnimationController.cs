using Spine;
using Spine.Unity;
using UnityEngine;

/// <summary> Spineアニメーションの再生を確認サンプルクラス </summary>
public class SampleSpineAnimationController : MonoBehaviour
{

	/// <summary> 最初に再生するアニメーション名 </summary>
	[SerializeField]
	private string testAnimationNameBefore = "janken/janken_pre";

	/// <summary> 次に再生するアニメーション名 </summary>
	[SerializeField]
	private string testAnimationNameAfter = "janken/janken_choki";

	/// <summary> 重ねる目ぱちアニメーション名 </summary>
	[SerializeField]
	private string testAnimationNameSubTrack = "janken/facial_eye_blink_B";

	/// <summary> 切り替えテスト用スキン名 </summary>
	[SerializeField]
	private string testSkinName = "skin_sad";
	
	/// <summary> ゲームオブジェクトに設定されているSkeletonAnimation </summary>
	private SkeletonAnimation skeletonAnimation = default;

	/// <summary> Spineアニメーションを適用するために必要なAnimationState </summary>
	private Spine.AnimationState spineAnimationState = default;

	/// <summary> メインのTrackIndex、全身のアニメーションの再生に使用 </summary>
	private readonly int mainTrackIndex = 100;

	/// <summary> サブのTrackIndex、一部分のアニメーションの再生に使用 </summary>
	private readonly int subTrackIndex = 10;

	private void Start()
	{
		// ゲームオブジェクトのSkeletonAnimationを取得
		skeletonAnimation = GetComponent<SkeletonAnimation>();

		// SkeletonAnimationからAnimationStateを取得
		spineAnimationState = skeletonAnimation.AnimationState;
	}

	private void Update()
	{
		// Aキーの入力でアニメーションを切り替えるテスト
		if (Input.GetKeyDown(KeyCode.A))
		{
			PlayAnimation(mainTrackIndex, testAnimationNameBefore, true);
		}

		// Bキーの入力でアニメーションを重ねるテスト
		if (Input.GetKeyDown(KeyCode.B))
		{
			PlayAnimation(subTrackIndex, testAnimationNameSubTrack, false);
		}

		// Cキーの入力でアニメーションを重ねるテスト
		if (Input.GetKeyDown(KeyCode.C))
		{
			SetSkin(testSkinName);
		}
	}

	/// <summary>
	/// Spineアニメーションを再生
	/// testAnimationNameに再生したいアニメーション名を記載してください。
	/// </summary>
	/// <param name="trackIndex">再生するTrackIndex</param>
	/// <param name="animName">再生するアニメーション名</param>
	/// <param name="isLoop">ループさせるかどうか</param>
	private void PlayAnimation(int trackIndex, string animName, bool isLoop)
	{
		// アニメーション「testAnimationName」を再生
		TrackEntry trackEntry = spineAnimationState.SetAnimation(trackIndex, animName, isLoop);

		// 完了通知を取得準備
		trackEntry.Complete += OnSpineComplete;
		// イベントを取得準備
		trackEntry.Event += OnTestEvent;
	}

	/// <summary>
	/// Spineアニメーションが完了した時の処理
	/// </summary>
	/// <param name="trackEntry"></param>
	private void OnSpineComplete(TrackEntry trackEntry)
	{
		// メイントラック以外は処理を中断
		if (trackEntry.TrackIndex != mainTrackIndex) return;

		spineAnimationState.SetAnimation(mainTrackIndex, testAnimationNameAfter, true);
	}

	/// <summary>
	/// スキンを変更する
	/// </summary>
	/// <param name="skinName">スキン名</param>
	private void SetSkin(string skinName)
	{
		skeletonAnimation.skeleton.SetSkin(skinName);
		skeletonAnimation.skeleton.SetSlotsToSetupPose();
	}
	
	/// <summary>
	/// Spineのキーに登録されたてテストイベントを実行
	/// </summary>
	/// <param name="trackEntry"></param>
	/// <param name="e"></param>
	private void OnTestEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (e.Data.Name != "OnTestEvent") return;

		Debug.Log(e.String);
	}
}
