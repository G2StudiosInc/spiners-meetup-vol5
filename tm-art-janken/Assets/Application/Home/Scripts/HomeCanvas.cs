using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class HomeCanvas : CanvasBase
{

	[SerializeField]
	private Animator anim = default;

	private readonly Subject<Unit> onCompleteEnd = new Subject<Unit>();

	private static readonly int BeginHash = Animator.StringToHash("Begin");
	private static readonly int EndHash = Animator.StringToHash("End");

	protected override void Start()
	{
		base.Start();

		ObservableStateMachineTrigger stateMachine = anim.GetBehaviour<ObservableStateMachineTrigger>(); // ステートマシンを取得

		stateMachine
			.OnStateExitAsObservable()
			.Where(_ => _.StateInfo.IsTag("End"))
			.SkipWhile(_ => _.StateInfo.normalizedTime <= 1.0f)
			.Subscribe(_ =>
			{
				onCompleteEnd.OnNext(Unit.Default);
			});
	}

	public void Begin()
	{
		anim.SetTrigger(BeginHash);
	}

	public IObservable<Unit> End()
	{
		anim.SetTrigger(EndHash);

		return onCompleteEnd;
	}

}