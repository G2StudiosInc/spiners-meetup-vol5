using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using JankenDefine;

public class PlayerManager : MonoBehaviour
{
	// じゃんけんの管理クラス
	[SerializeField]
	protected JankenManager jankenManager = default;

	// 手の選択完了通知
	private Subject<int> onHandSelected = new Subject<int>();
	public IObservable<int> OnHandSelected => onHandSelected;

	// 選択した手
	[SerializeField]
	protected JankenHand jankenHand = default;

	[SerializeField]
	private Text handText = default;

	/// <summary>
	/// キャラクターの選択した手を保存
	/// </summary>
	/// <param name="handNum"></param>
	protected void SetHand(int handNum)
	{
		jankenHand = (JankenHand)handNum;
		handText.text = jankenHand.ToString();
		onHandSelected.OnNext(handNum);
	}

	public virtual void Init() { }

}
