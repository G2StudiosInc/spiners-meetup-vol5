using UnityEngine;
using JankenDefine;

public class JankenVSManager : MonoBehaviour
{

    [SerializeField]
    private Animator anim = default;

    [SerializeField]
    private JankenIconView[] jankenIconView = new JankenIconView[2];

    private static readonly int BeginHash = Animator.StringToHash("Begin");
    private static readonly int EndHash = Animator.StringToHash("End");

    /// <summary>
    /// じゃんけんの選択した手を設定する
    /// </summary>
    /// <param name="userJankenHand"></param>
    /// <param name="rivalJankenHand"></param>
    public void SetJankenIcons(JankenHand userJankenHand, JankenHand rivalJankenHand)
    {
        jankenIconView[(int)PlayerCategory.USER].SetImages(userJankenHand);
        jankenIconView[(int)PlayerCategory.RIVAL].SetImages(rivalJankenHand);
    }

    public void Begin()
    {
        anim.SetTrigger(BeginHash);
    }

    public void End()
    {
        anim.SetTrigger(EndHash);
    }

}
