using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundColor : MonoBehaviour
{

	[SerializeField]
	private MainManager mainManager = default;

	public enum BGColorName
	{

		PINK = 0,
		GRAY,
		BLUE,
		YELLOW

	}

	[SerializeField]
	private Color[] backgroundColors = new Color[4];

	[SerializeField]
	private Image imgBackground = default;

	private readonly float duration = 0.3f;

	private void Start()
	{
		mainManager.OnEnterHome.Subscribe(_ =>
		{
			SetColor(BGColorName.PINK);
		});

		mainManager.OnEnterJanken.Subscribe(_ =>
		{
			SetColor(BGColorName.GRAY);
		});

		mainManager.OnEnterJankenWin.Subscribe(_ =>
		{
			SetColor(BGColorName.PINK);
		});

		mainManager.OnEnterJankenLose.Subscribe(_ =>
		{
			SetColor(BGColorName.BLUE);
		});

		mainManager.OnEnterHomeRecord.Subscribe(_ =>
		{
			SetColor(BGColorName.YELLOW);
		});
	}

	private void SetColor(BGColorName bgColorName = BGColorName.PINK)
	{
		Sequence sequence = DOTween.Sequence().SetAutoKill();

		sequence.Append(imgBackground.DOColor(backgroundColors[(int)bgColorName], duration));
	}

}
