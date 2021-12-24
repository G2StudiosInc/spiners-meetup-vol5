using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

	[SerializeField]
	protected List<AudioSource> audioSources = new List<AudioSource>();

	[SerializeField]
	protected List<AudioClip> audioClips = new List<AudioClip>();

	public void Init()
	{
		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.loop = false;
			audioSource.clip = audioClips[0];
			audioSource.ignoreListenerVolume = true;
			audioSource.playOnAwake = false;
		}
	}

	public void Play(int audioIndex, bool isLoop = false)
	{
		// audioSourcesの中で再生に使われていないものを探し再生する
		// 全てのaudioSourceが使用中の場合新しいSEはならない
		foreach (AudioSource audioSource in audioSources)
		{
			if (audioSource.isPlaying) continue;

			audioSource.clip = audioClips[audioIndex];
			audioSource.loop = isLoop;
			audioSource.Play();

			break;
		}
	}

	public void PlayOverride(int audioIndex, bool isLoop = false)
	{
		// audioSourcesの中で再生に使われていないものを探し再生する
		// 全てのaudioSourceが使用中の場合新しいSEはならない
		audioSources[0].clip = audioClips[audioIndex];
		audioSources[0].loop = isLoop;
		audioSources[0].Play();
	}

	public void Mute()
	{
		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.mute = true;
		}
	}

	public void UnMute()
	{
		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.mute = false;
		}
	}

	public void Stop()
	{
		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.Stop();
		}
	}

	public void Pause()
	{
		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.Pause();
		}
	}

	public void UnPause()
	{
		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.UnPause();
		}
	}

	public void SetVolume(float volume)
	{
		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.volume = volume;
		}
	}

}
