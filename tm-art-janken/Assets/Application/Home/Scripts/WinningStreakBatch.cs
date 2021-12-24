using UnityEngine;
using UnityEngine.UI;

public class WinningStreakBatch : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup group = default;

    [SerializeField]
    private Image[] imgNums = new Image[2];

    [SerializeField]
    private Sprite[] spriteNums = new Sprite[10];

    private int winningStreak = 0;

    /// <summary>
    /// 連勝表示の初期化
    /// </summary>
    public void Init()
    {
        group.alpha = 0;
        winningStreak = SaveLoadManager.Instance.GetBattleRecordWinningStreakNow();

        if (winningStreak <= 0) return;

        group.alpha = 1;

        // 10の位
        imgNums[0].sprite = spriteNums[winningStreak / 10];
        //  1の位
        imgNums[1].sprite = spriteNums[winningStreak % 10];
    }

}
