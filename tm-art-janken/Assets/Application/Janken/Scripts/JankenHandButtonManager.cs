using System;
using UniRx;
using UnityEngine;

public class JankenHandButtonManager : MonoBehaviour
{

	[SerializeField]
	private Animator anim = default;

	[SerializeField]
	private JankenHandButton[] jankenHandButtons = new JankenHandButton[3];

	public IObservable<int> OnClickBtn => onClickBtn;
	private readonly Subject<int> onClickBtn = new Subject<int>();

	private static readonly int BeginHash = Animator.StringToHash("Begin");
	private static readonly int EndHash = Animator.StringToHash("End");

	private void Start()
	{
		for (int i = 0; i < jankenHandButtons.Length; i++)
		{
			int num = i;

			// 個々のボタンを選択した時の通知、選択したボタンの番号を取得
			jankenHandButtons[i].OnClickBtn.Subscribe(_ =>
			{
				SoundController.Instance.PlaySE(SEName.SE_COMMON_TAP);
				SetButtonsEnabled(false);
				onClickBtn.OnNext(num);
				anim.SetTrigger(EndHash);
			});
		}
	}

	/// <summary>
	/// じゃんけんの選択手アイコンを表示する処理
	/// </summary>
	public void Begin()
	{
		SetButtonsEnabled(true);
		anim.SetTrigger(BeginHash);
	}

	/// <summary>
	/// 「グー」「チョキ」「パー」のボタンの入力受付を切り替える
	/// </summary>
	/// <param name="isActive"></param>
	private void SetButtonsEnabled(bool isActive = true)
	{
		foreach (JankenHandButton jankenHandButton in jankenHandButtons)
		{
			jankenHandButton.SetButtonEnabled(isActive);
		}
	}

}
