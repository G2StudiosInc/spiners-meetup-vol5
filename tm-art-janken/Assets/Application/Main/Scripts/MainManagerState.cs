using UniRx;

public partial class MainManager
{

    /// <summary>
    /// Titleシーン
    /// </summary>
    public class MainManagerStateTitle : MainManagerStateBase
    {

        public override void OnEnter(MainManager owner, MainManagerStateBase prevState)
        {
            SoundController.Instance.PlayBGM(BGMName.BGM_MAIN);
        }

    }

    /// <summary>
    /// Homeシーン
    /// </summary>
    public class MainManagerStateHome : MainManagerStateBase
    {

        public override void OnEnter(MainManager owner, MainManagerStateBase prevState)
        {
            if (!asyncSceneHome.allowSceneActivation)
                asyncSceneHome.allowSceneActivation = true;

            if (asyncSceneJanken == null)
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Title");
                asyncSceneJanken = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Janken", UnityEngine.SceneManagement.LoadSceneMode.Additive);
                asyncSceneJanken.allowSceneActivation = false;
            }

            owner.onEnterHome.OnNext(Unit.Default);
        }

    }

    /// <summary>
    /// Jankenシーン
    /// </summary>
    public class MainManagerStateJanken : MainManagerStateBase
    {

        public override void OnEnter(MainManager owner, MainManagerStateBase prevState)
        {
            asyncSceneJanken.allowSceneActivation = true;

            owner.onEnterJanken.OnNext(Unit.Default);
        }

    }

}
