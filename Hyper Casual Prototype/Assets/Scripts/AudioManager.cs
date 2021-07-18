// AudioManager
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;

	public Sound[] songs;

	public bool muted;

	public bool muteMusic = false;

	private float volume;

	public static AudioManager Instance;


	private void Awake()
	{

		Instance = this;

		
		volume = 1.2f;
		Instance = this;
		Sound[] array = sounds;
		foreach (Sound sound in array)
		{
			sound.source = base.gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.loop = sound.loop;
			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
			sound.source.bypassListenerEffects = sound.bypass;
		}
		array = songs;
		foreach (Sound sound2 in array)
		{
			sound2.source = base.gameObject.AddComponent<AudioSource>();
			sound2.source.clip = sound2.clip;
			sound2.source.loop = sound2.loop;
			sound2.source.volume = sound2.volume;
			sound2.source.pitch = sound2.pitch;
			sound2.source.bypassListenerEffects = sound2.bypass;
		}
		SetVolumeMusic(0.3f);
        
			StartMusic();
		
	}
    private void Start()
    {
		muted = !SaveManager.Instance.state.SFX;
		muteMusic = !SaveManager.Instance.state.Music;
	}
	public void MuteSounds()
	{
		muted = !muted;
		SaveManager.Instance.state.SFX = !muted;
		SaveManager.Instance.Save();
		if (!SaveManager.Instance.state.SFX)
		{
			GameManager.Instance.Sound.value = 1;
		}
		else
		{
			GameManager.Instance.Sound.value = 0;
		}
	}

	public void MuteMusic()
	{
		muteMusic = !muteMusic;
		SaveManager.Instance.state.Music = !muteMusic;
		SaveManager.Instance.Save();
		float num = (!muteMusic) ? 0.3f : 0f;
		songs[0].source.volume = num;
        if (SaveManager.Instance.state.Music)
        {
			StartMusic();
        }
		if (!SaveManager.Instance.state.Music)
		{
			GameManager.Instance.Music.value = 1;
		}
		else
		{
			GameManager.Instance.Music.value = 0;
		}
	}

	private void StartMusic()
	{
        if (SaveManager.Instance.state.Music)
        {
			//int num = Random.Range(0, songs.Length);
			songs[0].source.Play();
			Invoke("StartMusic", songs[0].source.clip.length);
		}
		
	}

	public void SetVolumeMusic(float v)
	{
		Sound[] array = songs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].source.volume = v;
		}
	}

	//public void SetVolumeSfx(float v)
	//{
	//	volume = v;
	//}

	public void UnmuteMusic()
	{
		Sound[] array = sounds;
		int num = 0;
		Sound sound;
		while (true)
		{
			if (num < array.Length)
			{
				sound = array[num];
				if (sound.name == "Song")
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		sound.source.volume = 1.15f;
	}

	public void Play(string n)
	{
		if (muted && n != "Song")
		{
			return;
		}
		Sound[] array = sounds;
		int num = 0;
		Sound sound;
		while (true)
		{
			if (num < array.Length)
			{
				sound = array[num];
				if (sound.name == n)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		sound.source.Play();
	}










}
