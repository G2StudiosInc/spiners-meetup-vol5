using UnityEngine;
using UnityEngine.UI;

public class SoundDebugButton : MonoBehaviour
{

    [SerializeField]
    private Text textBtnName = default;

    public void SetBtnName(string btnName)
    {
        textBtnName.text = btnName;
    }

}
