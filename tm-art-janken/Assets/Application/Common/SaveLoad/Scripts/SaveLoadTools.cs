#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class SaveLoadTools : MonoBehaviour
{

    // メニューの「Tools -> SaveData Reset」を選択するとRESETメソッドが呼ばれる
    [MenuItem("Tools/SaveData Reset")]
    public static void Reset()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("セーブデータをリセット");
    }

}
#endif