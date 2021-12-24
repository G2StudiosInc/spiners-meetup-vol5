using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SoundTestPresenter : MonoBehaviour
{

	private enum SoundType
	{

		BGM = 0,
		SE,
		VOICE,

	}

	[SerializeField]
	private GameObject[] objBtnsRoot = new GameObject[3];

	[SerializeField]
	private GameObject[] cloneBtns = new GameObject[3];

	[SerializeField]
	private Button[] stopBtns = new Button[3];

	[SerializeField]
	private Button[] pauseBtns = new Button[3];

	[SerializeField]
	private Text[] pauseTexts = new Text[3];

	private readonly ReactiveProperty<bool> isPauseBGM = new ReactiveProperty<bool>(false);
	private readonly ReactiveProperty<bool> isPauseSE = new ReactiveProperty<bool>(false);

	private void Start()
	{
		Array bgmValues = Enum.GetValues(typeof(BGMName));

		foreach (object value in bgmValues)
		{
			GameObject cloneBGMBtn = Instantiate(cloneBtns[(int)SoundType.BGM], objBtnsRoot[(int)SoundType.BGM].transform);
			cloneBGMBtn.name = value.ToString();
			cloneBGMBtn.GetComponent<SoundDebugButton>().SetBtnName(value.ToString());

			cloneBGMBtn.GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
			{
				SoundController.Instance.StopBGM();
				SoundController.Instance.PlayBGM((BGMName)value);
				isPauseBGM.Value = false;
			});
		}

		Array seValues = Enum.GetValues(typeof(SEName));

		foreach (object value in seValues)
		{
			GameObject cloneSEBtn = Instantiate(cloneBtns[(int)SoundType.SE], objBtnsRoot[(int)SoundType.SE].transform);
			cloneSEBtn.name = value.ToString();
			cloneSEBtn.GetComponent<SoundDebugButton>().SetBtnName(value.ToString());

			cloneSEBtn.GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
			{
				SoundController.Instance.PlaySE((SEName)value);
				isPauseSE.Value = false;
			});
		}

		Array voiceValues = Enum.GetValues(typeof(VOICEName));

		foreach (object value in voiceValues)
		{
			GameObject cloneVOICEBtn = Instantiate(cloneBtns[(int)SoundType.VOICE], objBtnsRoot[(int)SoundType.VOICE].transform);
			cloneVOICEBtn.name = value.ToString();
			cloneVOICEBtn.GetComponent<SoundDebugButton>().SetBtnName(value.ToString());

			cloneVOICEBtn.GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
			{
				SoundController.Instance.PlayVOICE((VOICEName)value);
			});
		}

		// クローン元のゲームオブジェクトを非表示にする
		cloneBtns[(int)SoundType.BGM].SetActive(false);
		cloneBtns[(int)SoundType.SE].SetActive(false);
		cloneBtns[(int)SoundType.VOICE].SetActive(false);

		// BGMのPauseボタン設定
		pauseBtns[(int)SoundType.BGM].OnClickAsObservable().Subscribe(_ =>
		{
			isPauseBGM.Value = !isPauseBGM.Value;

			if (isPauseBGM.Value)
			{
				SoundController.Instance.PauseBGM();
			}
			else
			{
				SoundController.Instance.UnPauseBGM();
			}
		});

		// SEのPauseボタン設定
		pauseBtns[(int)SoundType.SE].OnClickAsObservable().Subscribe(_ =>
		{
			isPauseSE.Value = !isPauseSE.Value;

			if (isPauseSE.Value)
			{
				SoundController.Instance.PauseSE();
			}
			else
			{
				SoundController.Instance.UnPauseSE();
			}
		});

		// BGMのPause状態の切り替えに対応した処理
		isPauseBGM
			.DistinctUntilChanged()
			.Subscribe(flag =>
			{
				if (flag)
				{
					pauseTexts[(int)SoundType.BGM].text = "UnPause\nBGM";
				}
				else
				{
					pauseTexts[(int)SoundType.BGM].text = "Pause\nBGM";
				}
			}).AddTo(this);

		// SEのPause状態の切り替えに対応した処理
		isPauseSE
			.DistinctUntilChanged()
			.Subscribe(flag =>
			{
				if (flag)
				{
					pauseTexts[(int)SoundType.SE].text = "UnPause\nSE";
				}
				else
				{
					pauseTexts[(int)SoundType.SE].text = "Pause\nSE";
				}
			}).AddTo(this);

		// BGMのStop設定
		stopBtns[(int)SoundType.BGM].OnClickAsObservable().Subscribe(_ =>
		{
			SoundController.Instance.StopBGM();
			isPauseBGM.Value = false;
		});

		// SEのStop設定
		stopBtns[(int)SoundType.SE].OnClickAsObservable().Subscribe(_ =>
		{
			SoundController.Instance.StopSE();
			isPauseSE.Value = false;
		});

	}

}
