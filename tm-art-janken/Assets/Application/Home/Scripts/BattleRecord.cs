using UnityEngine;
using UnityEngine.UI;

public class BattleRecord : MonoBehaviour
{

    [SerializeField]
    private Text textBattleTotal = default;

    [SerializeField]
    private Text[] textJudges = new Text[3];

    [SerializeField]
    private Text textWinRate = default;

    [SerializeField]
    private Text textWinningStreakBest = default;

    [SerializeField]
    private Text[] textJankenHandsSelects = new Text[3];

    public void Init()
    {
        BattleRecordManager.BattleRecordSaveData battleRecordSaveData = SaveLoadManager.Instance.GetBattleRecordSaveData();

        int battleTotal = battleRecordSaveData.wins + battleRecordSaveData.loses;
        int jankenHandTotal = battleRecordSaveData.jankenHandsSelect[0] + battleRecordSaveData.jankenHandsSelect[1] + battleRecordSaveData.jankenHandsSelect[2];

        textBattleTotal.text = battleTotal.ToString("N0");
        textJudges[0].text = battleRecordSaveData.wins.ToString("N0");
        textJudges[1].text = battleRecordSaveData.loses.ToString("N0");
        textJudges[2].text = battleRecordSaveData.draws.ToString("N0");

        if (battleTotal == 0)
        {
            textWinRate.text = "0.00";
        }
        else
        {
            textWinRate.text = ((float)(battleRecordSaveData.wins) / (float)(battleTotal) * 100.0f).ToString("N2");
        }

        textWinningStreakBest.text = battleRecordSaveData.winningStreakBest.ToString("N0");

        if (jankenHandTotal == 0)
        {
            for (int i = 0; i < battleRecordSaveData.jankenHandsSelect.Length; i++)
            {
                textJankenHandsSelects[i].text = "0.00";
            }
        }
        else
        {
            for (int i = 0; i < battleRecordSaveData.jankenHandsSelect.Length; i++)
            {
                textJankenHandsSelects[i].text = ((float)battleRecordSaveData.jankenHandsSelect[i] / (float)jankenHandTotal * 100.0f).ToString("N2");
            }
        }

    }

}
