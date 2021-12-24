using UnityEngine;

public class CanvasBase : MonoBehaviour
{

    [SerializeField]
    private Canvas canvas = default;

    [SerializeField]
    private Camera uiCamera = default;

    protected virtual void Start()
    {
        canvas = GetComponent<Canvas>();

        uiCamera = GameObject.Find("UICamera")?.GetComponent<Camera>();
        canvas.worldCamera = uiCamera;
    }

}
