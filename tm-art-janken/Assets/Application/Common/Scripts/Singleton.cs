using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

	public static T Instance { get; private set; } = null;

	/// <summary>
	/// Singletonが有効か
	/// </summary>
	public static bool IsValid() => Instance != null;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this as T;
			Instance.Init();
			DontDestroyOnLoad(gameObject);

			return;
		}

		Destroy(gameObject);
	}

	/// <summary>
	/// 派生クラス用のAwake
	/// </summary>
	protected virtual void Init() { }


	private void OnDestroy()
	{
		if (Instance == null)
		{
			Instance = null;
		}

		OnRelease();
	}

	/// <summary>
	/// 派生クラス用のDestroy
	/// </summary>
	protected virtual void OnRelease() { }

}
