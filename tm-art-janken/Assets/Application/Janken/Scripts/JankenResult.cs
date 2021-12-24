using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using JankenDefine;

public class JankenResult : MonoBehaviour
{

    [SerializeField]
    private Animator anim = default;

    private static readonly int BeginHash = Animator.StringToHash("Begin");
    private static readonly int JudgeHash = Animator.StringToHash("Judge");
    private static readonly int EndHash = Animator.StringToHash("End");

    private readonly Subject<Unit> onCompleteEnd = new Subject<Unit>();

    private void Start()
    {
        ObservableStateMachineTrigger stateMachine = anim.GetBehaviour<ObservableStateMachineTrigger>();

        stateMachine
            .OnStateExitAsObservable()
            .Where(_ => _.StateInfo.IsTag("End"))
            .SkipWhile(_ => _.StateInfo.normalizedTime <= 1.0f)
            .Subscribe(_ =>
            {
                onCompleteEnd.OnNext(Unit.Default);
            });
    }

    public void Begin(JankenJudge jankenJudge)
    {
        anim.SetTrigger(BeginHash);
        anim.SetInteger(JudgeHash, (int)jankenJudge);
    }

    public IObservable<Unit> End()
    {
        anim.SetTrigger(EndHash);

        return onCompleteEnd;
    }

}
