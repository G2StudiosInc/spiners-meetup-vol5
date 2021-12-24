using UnityEngine;
using UnityEngine.UI;
using UniRx;

public partial class TitleManager : MonoBehaviour
{
	
	private GameObject objMainManager = default;
	private MainManager maineManager = default;

	[SerializeField]
	private Button btnScreen = default;

	[SerializeField]
	private TitleCanvas titleCanvas = default;

	private void Start()
	{
		objMainManager = GameObject.Find("MainManager");
		maineManager = objMainManager?.GetComponent<MainManager>();

		// 画面をタップした時の処理
		btnScreen.OnClickAsObservable().First().Subscribe(_ =>
		{
			SoundController.Instance.PlaySE(SEName.SE_TAP_START);

			titleCanvas.End().First().Subscribe(_ =>
			{
				SoundController.Instance.PlaySE(SEName.SE_CHARACTER_IN);
				// ステートを更新
				maineManager.ChangeNextState();
			});
		}).AddTo(this);

		titleCanvas.Begin();
	}

}
