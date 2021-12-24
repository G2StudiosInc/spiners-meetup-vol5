using UnityEngine;
using UnityEngine.UI;

public class AchievementCell : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup[] groupIcons = new CanvasGroup[2];

    [SerializeField]
    private Text[] textTitle = new Text[2];

    private string condition = default;

    private void Awake()
    {
        groupIcons[0].alpha = 1;
        groupIcons[1].alpha = 0;
    }

    /// <summary>
    /// クリアの設定
    /// </summary>
    public void SetIsClear()
    {
        groupIcons[0].alpha = 0;
        groupIcons[1].alpha = 1;
    }

    /// <summary>
    /// 初期化
    /// 解放条件の文字列を設定
    /// </summary>
    /// <param name="condition">このセルの解放条件</param>
    public void Init(string title, string condition)
    {
        textTitle[0].text = textTitle[1].text = title;
        this.condition = condition;
    }

    /// <summary>
    /// 解放条件の文字列を取得
    /// </summary>
    /// <returns></returns>
    public string GetCondition()
    {
        return condition;
    }

}
