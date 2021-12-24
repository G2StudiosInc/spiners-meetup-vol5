using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;

public class JankenCallTexts : MonoBehaviour
{

	[SerializeField]
	private Animator anim = default;

	// 「じゃん」「ぽん」をそれぞれ「あいこで」「しょ！」に変える為の変数
	[SerializeField]
	private Image[] imageCall_1 = new Image[2];

	[SerializeField]
	private Image imageCall_2 = default;

	[SerializeField]
	private Image[] imageCall_3 = new Image[2];

	private static readonly int BeginHash = Animator.StringToHash("Begin");
	private static readonly int BattleHash = Animator.StringToHash("Battle");
	private static readonly int EndHash = Animator.StringToHash("End");
	private static readonly int OneMoreHash = Animator.StringToHash("OneMore");

	public void Initialize()
	{
		// 「じゃん」「けん」「ぽん」の表示に対応
		imageCall_1[0].enabled = true;
		imageCall_1[1].enabled = false;
		imageCall_2.enabled = true;
		imageCall_3[0].enabled = true;
		imageCall_3[1].enabled = false;
	}

	/// <summary>
	/// 「じゃん」から「けん」までの流れで表示を開始する。
	/// </summary>
	public void Begin()
	{
		anim.SetTrigger(BeginHash);
	}

	public void Battle()
	{
		anim.SetTrigger(BattleHash);
	}

	public void End()
	{
		anim.SetTrigger(EndHash);
	}

	public void SetDraw()
	{
		anim.SetTrigger(OneMoreHash);

		// 「あいこで」「しょ」の表示に対応
		imageCall_1[0].enabled = false;
		imageCall_1[1].enabled = true;
		imageCall_2.enabled = false;
		imageCall_3[0].enabled = false;
		imageCall_3[1].enabled = true;
	}

}
