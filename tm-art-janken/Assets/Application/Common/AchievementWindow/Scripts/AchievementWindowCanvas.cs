using UniRx;
using UnityEngine;

public class AchievementWindowCanvas : MonoBehaviour
{

    private static AchievementWindowCanvas instance;

    public static AchievementWindowCanvas Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType<AchievementWindowCanvas>();
            DontDestroyOnLoad(instance.gameObject);

            return instance;
        }
    }

    [SerializeField]
    private Canvas canvas = default;

    [SerializeField]
    private AchievementWindow achievementWindow = default;

    private void Start()
    {
        canvas.enabled = false;
    }

    public void Run(string title)
    {
        canvas.enabled = true;

        achievementWindow.Run(title).Subscribe(_ =>
        {
            canvas.enabled = false;
        });
    }

}
