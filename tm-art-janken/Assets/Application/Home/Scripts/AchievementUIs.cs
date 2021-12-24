using System.Collections.Generic;
using UnityEngine;

public class AchievementUIs : MonoBehaviour
{

    [SerializeField]
    private Transform transCellParent = default;

    [SerializeField]
    private GameObject cloneCell = default;

    private readonly List<AchievementCell> achievementCells = new List<AchievementCell>();
    private List<AchievementManager.AchievementData> loadDataList = new List<AchievementManager.AchievementData>();

    private void Awake()
    {
        loadDataList = SaveLoadManager.Instance.GetAchievementDataList();
    }

    private void Start()
    {
        loadDataList = SaveLoadManager.Instance.GetAchievementDataList();

        foreach (var loadData in loadDataList)
        {
            GameObject clone = Instantiate(cloneCell, transCellParent);
            AchievementCell achievementCell = clone.GetComponent<AchievementCell>();
            achievementCell.Init(loadData.title, loadData.conditions);

            if (SaveLoadManager.Instance.GetAchievementClear(loadData.achievementName))
            {
                achievementCell.SetIsClear();
            }

            achievementCells.Add(achievementCell);
        }

        cloneCell.SetActive(false);
    }

    public void Init()
    {
        for (int i = 0; i < loadDataList.Count; i++)
        {
            if (SaveLoadManager.Instance.GetAchievementClear(loadDataList[i].achievementName))
            {
                achievementCells[i].SetIsClear();
            }
        }
    }

}
