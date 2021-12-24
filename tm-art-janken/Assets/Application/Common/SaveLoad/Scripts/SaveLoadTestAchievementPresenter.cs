using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SaveLoadTestAchievementPresenter : MonoBehaviour
{

    [SerializeField]
    private Button btnSave = default;

    [SerializeField]
    private Button btnLoad = default;

    [SerializeField]
    private Button btnReset = default;

    [SerializeField]
    private List<Button> btnUnlockList = new List<Button>();

    [SerializeField]
    private GameObject rootUnlockBtn = default;

    [SerializeField]
    private GameObject cloneUnlockBtn = default;

    private List<AchievementManager.AchievementData> achievementDataList = new List<AchievementManager.AchievementData>();

    private void Start()
    {
        btnSave.OnClickAsObservable().Subscribe(_ =>
        {
            SaveLoadManager.Instance.SaveAchievement();
        });

        btnLoad.OnClickAsObservable().Subscribe(_ =>
        {
            SaveLoadManager.Instance.LoadAchievement();
            Init();
        });

        btnReset.OnClickAsObservable().Subscribe(_ =>
        {
            SaveLoadManager.Instance.Reset();

            foreach (Button btnUnlock in btnUnlockList)
            {
                btnUnlock.interactable = true;
            }
        });

        Init();
    }

    private void Init()
    {
        achievementDataList = SaveLoadManager.Instance.GetAchievementDataList();

        foreach (AchievementManager.AchievementData achievementData in achievementDataList)
        {
            GameObject clone = Instantiate(cloneUnlockBtn, rootUnlockBtn.transform);
            Button btn = clone.GetComponent<Button>();
            Text btnText = clone.transform.GetChild(0).GetComponent<Text>();

            btn.OnClickAsObservable().Subscribe(_ =>
            {
                SaveLoadManager.Instance.SetAchievementClear(achievementData.achievementName);
                btn.interactable = false;
            });

            btnText.text = achievementData.title;

            btn.interactable = !SaveLoadManager.Instance.GetAchievementClear(achievementData.achievementName);
            btnUnlockList.Add(btn);
        }

        cloneUnlockBtn.SetActive(false);

    }

}
