using System;
using UniRx;
using JankenDefine;

public class CharacterManager : PlayerManager
{

	private void Start()
	{
		Init();

		// あいこ時の選択し直し
		jankenManager.OnJudgedDraw.Subscribe(_ =>
		{
			SetHand(UnityEngine.Random.Range(0, Enum.GetValues(typeof(JankenHand)).Length));
		}).AddTo(this);
	}

	public override void Init()
	{
		// スタート時にキャラクターの選択する手を決定する
		SetHand(UnityEngine.Random.Range(0, Enum.GetValues(typeof(JankenHand)).Length));
	}

}
