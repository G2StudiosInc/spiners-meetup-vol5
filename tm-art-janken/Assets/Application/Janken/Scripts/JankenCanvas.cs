using UnityEngine;

public class JankenCanvas : CanvasBase
{

	[SerializeField]
	private CanvasGroup groupRoot = default;

	public void Begin()
	{
		groupRoot.alpha = 1;
		groupRoot.interactable = true;
		groupRoot.blocksRaycasts = true;
	}

	public void End()
	{
		groupRoot.alpha = 0;
		groupRoot.interactable = false;
		groupRoot.blocksRaycasts = false;
	}

}
