using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class SoundController : MonoBehaviour
{

	[SerializeField]
	private bool isMute = false;

	[SerializeField]
	private float[] soundVolumes = new float[3];

	[SerializeField]
	private List<GameObject> objMute = new List<GameObject>();

	private enum SoundType
	{

		BGM = 0,
		SE,
		VOICE,

	}

	[SerializeField]
	private List<SoundManager> soundManagers = new List<SoundManager>();

	private static SoundController instance;

	public static SoundController Instance
	{
		get
		{
			if (instance != null) return instance;

			instance = FindObjectOfType<SoundController>();
			DontDestroyOnLoad(instance.gameObject);

			return instance;
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			if (this == instance) return;

			Destroy(gameObject);
		}

		foreach (SoundManager soundManager in soundManagers)
		{
			soundManager.Init();
		}
	}

	private void Start()
	{
		objMute[0].SetActive(isMute);
		objMute[1].SetActive(!isMute);

		foreach (SoundManager soundManager in soundManagers)
		{
			if (isMute)
				soundManager.Mute();
			else
				soundManager.UnMute();
		}

	}

	public void PlayBGM(BGMName bgmName)
	{
		soundManagers[(int)SoundType.BGM].Play((int)bgmName, true);
	}

	public void StopBGM()
	{
		soundManagers[(int)SoundType.BGM].Stop();
	}

	public void PauseBGM()
	{
		soundManagers[(int)SoundType.BGM].Pause();
	}

	public void UnPauseBGM()
	{
		soundManagers[(int)SoundType.BGM].UnPause();
	}

	public void PlaySE(SEName seName)
	{
		soundManagers[(int)SoundType.SE].Play((int)seName);
	}

	public void StopSE()
	{
		soundManagers[(int)SoundType.SE].Stop();
	}

	public void PauseSE()
	{
		soundManagers[(int)SoundType.SE].Pause();
	}

	public void UnPauseSE()
	{
		soundManagers[(int)SoundType.SE].UnPause();
	}

	public void PlayVOICE(VOICEName voiceName)
	{
		soundManagers[(int)SoundType.VOICE].PlayOverride((int)voiceName);
	}

	public void PlayVOICE(string voiceType, int voiceNumMax)
	{
		// 同一種のボイスデータのIndex指定
		int voiceIndex = Random.Range(1, voiceNumMax + 1);

		// 実際のファイル名に置換
		string voiceFileName = $"{voiceType}{voiceIndex}";

		VOICEName voiceName = (VOICEName)System.Enum.Parse(typeof(VOICEName), voiceFileName);

		PlayVOICE(voiceName);

	}

	public void StopVOICE()
	{
		soundManagers[(int)SoundType.VOICE].Stop();
	}

	public void PauseVOICE()
	{
		soundManagers[(int)SoundType.VOICE].Pause();
	}

	public void UnPauseVOICE()
	{
		soundManagers[(int)SoundType.VOICE].UnPause();
	}

	public void Mute()
	{
		isMute = isMute ? false : true;

		objMute[0].SetActive(isMute);
		objMute[1].SetActive(!isMute);

		foreach (var soundManager in soundManagers)
		{
			if (isMute)
				soundManager.Mute();
			else
				soundManager.UnMute();
		}

		if (!isMute) PlaySE(SEName.SE_COMMON_TAP);
	}

	#if UNITY_EDITOR
	[SerializeField]
	private BGMManager bgmManager = default;

	[SerializeField]
	private SEManager seManager = default;

	[SerializeField]
	private VOICEManager voiceManager = default;

	[ContextMenu("AudioClip関連の初期化")]
	public void InitAudioClip()
	{
		bgmManager.InitAudioClip();
		seManager.InitAudioClip();
		voiceManager.InitAudioClip();
	}

	[ContextMenu("BGM,SE,VOICEの音量変更")]
	public void SetVolumes()
	{
		bgmManager.SetVolume(soundVolumes[(int)SoundType.BGM]);
		seManager.SetVolume(soundVolumes[(int)SoundType.SE]);
		voiceManager.SetVolume(soundVolumes[(int)SoundType.VOICE]);
	}
	#endif

}