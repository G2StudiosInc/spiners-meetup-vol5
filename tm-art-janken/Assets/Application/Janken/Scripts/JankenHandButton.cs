using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class JankenHandButton : MonoBehaviour
{

	public IObservable<Unit> OnClickBtn => onClickBtn;
	private readonly Subject<Unit> onClickBtn = new Subject<Unit>();

	// 選択する手のボタンUI（グー・チョキ・パー）
	[SerializeField]
	private Button btnHand = default;

	private void Start()
	{
		// ボタンを押した通知
		btnHand.OnClickAsObservable().Subscribe(_ =>
		{
			onClickBtn.OnNext(Unit.Default);
		});

	}

	/// <summary>
	/// ボタンの入力受付を切り替える
	/// </summary>
	/// <param name="isActive"></param>
	public void SetButtonEnabled(bool isActive = true)
	{
		btnHand.enabled = isActive;
	}

}
