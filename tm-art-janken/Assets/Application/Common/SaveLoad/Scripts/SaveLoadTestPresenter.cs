using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SaveLoadTestPresenter : MonoBehaviour
{

    [SerializeField]
    private Button btnSave = default;

    [SerializeField]
    private Button btnLoad = default;

    [SerializeField]
    private Button btnWin = default;

    [SerializeField]
    private Button btnLose = default;

    [SerializeField]
    private Button btnReset = default;

    [SerializeField]
    private Text textWin = default;

    [SerializeField]
    private Text textLose = default;

    private BattleRecordManager.BattleRecordSaveData battleRecordSaveData = default;

    private void Start()
    {
        btnSave.OnClickAsObservable().Subscribe(_ =>
        {
            SaveLoadManager.Instance.SaveBattleRecord();
        });

        btnLoad.OnClickAsObservable().Subscribe(_ =>
        {
            SaveLoadManager.Instance.LoadBattleRecord();
            Init();
        });

        btnWin.OnClickAsObservable().Subscribe(_ =>
        {
            SaveLoadManager.Instance.AddWin();
            battleRecordSaveData = SaveLoadManager.Instance.GetBattleRecordSaveData();
            textWin.text = battleRecordSaveData.wins.ToString();
        });

        btnLose.OnClickAsObservable().Subscribe(_ =>
        {
            SaveLoadManager.Instance.AddLose();
            battleRecordSaveData = SaveLoadManager.Instance.GetBattleRecordSaveData();
            textLose.text = battleRecordSaveData.loses.ToString();
        });

        btnReset.OnClickAsObservable().Subscribe(_ =>
        {
            SaveLoadManager.Instance.Reset();
            battleRecordSaveData = SaveLoadManager.Instance.GetBattleRecordSaveData();
            textWin.text = battleRecordSaveData.wins.ToString();
            textLose.text = battleRecordSaveData.loses.ToString();
        });

        Init();
    }

    private void Init()
    {
        battleRecordSaveData = SaveLoadManager.Instance.GetBattleRecordSaveData();
        textWin.text = battleRecordSaveData.wins.ToString();
        textLose.text = battleRecordSaveData.loses.ToString();
    }

}
