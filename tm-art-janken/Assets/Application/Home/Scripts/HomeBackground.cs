using UnityEngine;

public class HomeBackground : MonoBehaviour
{

    [SerializeField]
    private Animator anim = default;

    private static readonly int BeginHash = Animator.StringToHash("Begin");
    private static readonly int EndHash = Animator.StringToHash("End");

    public void Begin()
    {
        anim.SetTrigger(BeginHash);
    }

    public void End()
    {
        anim.SetTrigger(EndHash);
    }

}
