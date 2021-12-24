using UniRx;
using UnityEngine;

public class UserManager : PlayerManager
{

	// じゃんけんの手選択ボタンを管理するクラスを取得
	[SerializeField]
	private JankenHandButtonManager jankenHandButtonManager = default;

	private void Start()
	{
		// ユーザーがじゃんけんの手を選択した通知
		jankenHandButtonManager.OnClickBtn.Subscribe(handNum =>
		{
			SetHand(handNum);
		}).AddTo(this);
	}

}
