using System;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class MainCameraManager : MonoBehaviour
{

	[SerializeField]
	private MainManager mainManager = default;

	private Sequence sequence = default;

	private readonly Subject<Unit> onCompleteEnterHome = new Subject<Unit>();
	public IObservable<Unit> OnCompleteEnterHome => onCompleteEnterHome;

	private enum TargetPosName
	{

		Title = 0,
		Home,
		HomeRecord,
		Janken,
		JankenJudge,
		JankenResult,

	}

	[Serializable]
	private class TargetPosList
	{

		public string name = default; // インスペクター表示用
		public Vector3 pos = default;
		public float duration = default;
		public Ease easeType = default;

	}

	[SerializeField]
	private TargetPosList[] targetPosList = new TargetPosList[3];

	private void Start()
	{
		int index = 0;

		this.transform.position = targetPosList[(int)TargetPosName.Title].pos;

		mainManager.OnEnterHome.Subscribe(_ =>
		{
			index = (int)TargetPosName.Home;
			sequence = DOTween.Sequence().SetAutoKill();
			sequence.Append(this.transform.DOLocalMove(targetPosList[index].pos, targetPosList[index].duration).SetEase(targetPosList[index].easeType));

			sequence.OnComplete(() =>
			{
				onCompleteEnterHome.OnNext(Unit.Default);
			});
		});

		mainManager.OnEnterHomeRecord.Subscribe(_ =>
		{
			index = (int)TargetPosName.HomeRecord;
			sequence = DOTween.Sequence().SetAutoKill();
			sequence.Append(this.transform.DOLocalMove(targetPosList[index].pos, targetPosList[index].duration).SetEase(targetPosList[index].easeType));
			sequence.OnComplete(() => { });
		});

		mainManager.OnEnterJanken.Subscribe(_ =>
		{
			index = (int)TargetPosName.Janken;
			sequence = DOTween.Sequence().SetAutoKill();
			sequence.Append(this.transform.DOLocalMove(targetPosList[index].pos, targetPosList[index].duration).SetEase(targetPosList[index].easeType));
		});

		mainManager.OnEnterJankenDraw.Subscribe(_ =>
		{
			index = (int)TargetPosName.Janken;
			sequence = DOTween.Sequence().SetAutoKill();
			sequence.Append(this.transform.DOLocalMove(targetPosList[index].pos, targetPosList[index].duration).SetEase(targetPosList[index].easeType));
		});

		mainManager.OnEnterJankenJudge.Subscribe(_ =>
		{
			index = (int)TargetPosName.JankenJudge;
			sequence = DOTween.Sequence().SetAutoKill();
			sequence.Append(this.transform.DOLocalMove(targetPosList[index].pos, targetPosList[index].duration).SetEase(targetPosList[index].easeType));
		});

		mainManager.OnEnterJankenWin.Subscribe(_ =>
		{
			index = (int)TargetPosName.JankenResult;
			sequence = DOTween.Sequence().SetAutoKill();
			sequence.Append(this.transform.DOLocalMove(targetPosList[index].pos, targetPosList[index].duration).SetEase(targetPosList[index].easeType));
		});

		mainManager.OnEnterJankenLose.Subscribe(_ =>
		{
			index = (int)TargetPosName.JankenResult;
			sequence = DOTween.Sequence().SetAutoKill();
			sequence.Append(this.transform.DOLocalMove(targetPosList[index].pos, targetPosList[index].duration).SetEase(targetPosList[index].easeType));
		});
	}

}
