using DG.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class AchievementWindow : MonoBehaviour
{

	[SerializeField]
	private RectTransform rectWindows = default;

	[SerializeField]
	private CanvasGroup groupWindow = default;

	[SerializeField]
	private Vector2 defaultPos = default;

	[SerializeField]
	private Vector2 targetPos = default;

	[SerializeField]
	private Text textTitle = default;

	private readonly Subject<Unit> onComplete = new Subject<Unit>();

	private Sequence mainSequence = default;
	private Sequence subSequence = default;
	private readonly float duration = 0.5f;
	private readonly float delay = 1f;

	private readonly string titleFormat = "{0}を達成しました！";

	[SerializeField]
	private List<string> titleList = new List<string>();

	private void Start()
	{
		rectWindows.anchoredPosition = defaultPos;
		groupWindow.alpha = 0;
	}

	/// <summary>
	/// 実績ウィンドウを表示させる処理を再生する
	/// </summary>
	/// <param name="title">達成された実績の名前</param>
	public IObservable<Unit> Run(string title)
	{
		titleList.Add(String.Format(titleFormat, title));
		textTitle.text = titleList[0];

		if (titleList.Count == 1)
		{
			MoveAnimation();
		}

		return onComplete;
	}

	/// <summary>
	/// 移動の開始から終了までを行う関数
	/// </summary>
	private void MoveAnimation()
	{
		mainSequence = DOTween.Sequence().SetAutoKill();

		// 初期状態の設定
		mainSequence.OnStart(() =>
		{
			rectWindows.anchoredPosition = defaultPos;
			groupWindow.alpha = 1;
		});

		// 画面外（上）から画面内（上）に移動させつつ拡大するUI登場アニメーション
		mainSequence.Append(rectWindows.DOAnchorPos(targetPos, duration).SetEase(Ease.OutBack));

		// UI登場アニメーション完了時の処理、一定時間待機後に画面外へ移動させるアニメーションを再生する
		// アニメーション再生中に他の実績が解放された場合は連続再生させる
		mainSequence.OnComplete(() =>
		{
			subSequence = DOTween.Sequence().SetAutoKill();
			subSequence.SetDelay(delay);
			subSequence.Append(rectWindows.DOAnchorPos(defaultPos, duration * 0.5f).SetEase(Ease.InExpo));

			subSequence.OnComplete(() =>
			{
				// 表示した実績をリストから削除
				titleList.RemoveAt(0);

				// 未表示の実績があれば再度MoveAnimationを再生する
				if (titleList.Count > 0)
				{
					textTitle.text = titleList[0];
					MoveAnimation();
				}
				else
				{
					groupWindow.alpha = 0;
					onComplete.OnNext(Unit.Default);
				}
			});
		});
	}

}
