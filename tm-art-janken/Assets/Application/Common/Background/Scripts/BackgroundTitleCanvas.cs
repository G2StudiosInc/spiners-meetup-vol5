using UnityEngine;
using UniRx;
using DG.Tweening;

public class BackgroundTitleCanvas : MonoBehaviour
{

	[SerializeField]
	private MainManager mainManager = default;

	[SerializeField]
	private CanvasGroup group = default;

	private readonly float duration = 0.3f;

	private void Start()
	{
		mainManager.OnEnterHome.Subscribe(_ =>
		{
			Sequence sequence = DOTween.Sequence().SetAutoKill();
			sequence.Append(group.DOFade(0f, duration));
		});
	}

}
