using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
	public AudioMixerGroup musicGroup, sfxGroup;

    public Sound[] soundsBGM;
    public Sound[] sounds;

	public const string onClickNormal = "OnClickNormal";
	public const string onClickError = "OnClickError";

    private void Awake()
	{
        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = sfxGroup;
		}
        foreach (Sound s in soundsBGM) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = musicGroup;
        }
    }
    
    private void Start()
    {
        var music = PlayerPrefs.GetInt("GetMusicState");
        var sfx = PlayerPrefs.GetInt("GetSoundState");
        if (music == 1)
            mixer.SetFloat("music", 0);
        else
            mixer.SetFloat("music", -80);
        if (sfx == 1)
            mixer.SetFloat("sfx", 0);
        else
            mixer.SetFloat("sfx", -80);


        // play background music 
        PlayBGM(Constant.AUDIO_MUSIC_BG);
    }
    public void PlayBGM(string sound)
    {
        foreach (Sound t in soundsBGM)
        {
            if (!t.source)
            {
                continue;
            }

            if (t.name != sound)
            {
                t.source.Stop(); 
            }
        }

        PlayBgm(sound);
    }

    private void PlayBgm(string sound)
    {
        Sound s = Array.Find(soundsBGM, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        if (!s.source.isPlaying)
        {
            s.source.Play(); 
        }
    }
    public void Play(string sound)
	{
        Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.volume = s.volume/* * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f))*/;
		s.source.pitch = s.pitch/* * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f))*/;

		s.source.PlayOneShot(s.clip);
	}
    public void PauseBGM()
    {
        foreach (Sound s in soundsBGM)
        {
            s.source.Pause();
        }
    }
    public void UnPauseBGM()
    {
        bool didnotplaying = false;
        
        foreach (Sound s in soundsBGM)
        {
            didnotplaying = s.source.isPlaying;
            s.source.UnPause();
        }
        
        if (!didnotplaying)
        {
            PlayBGM(Constant.AUDIO_MUSIC_BG);
        }
    }
    public void PauseSFX()
    {
        foreach (Sound s in sounds) 
        {
            s.source.Pause();
        }
    }
    public void UnPauseSFX() 
    {
        foreach (Sound s in sounds)
        {
            s.source.UnPause();
        }
    }
    public void Pause(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume /** (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f))*/;
        s.source.pitch = s.pitch/* * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f))*/;

        s.source.Pause();
    }

    public void MusicOn()
    {
        mixer.SetFloat("Music", -1);
        PlayerPrefs.SetInt("GetMusicState",1);
    }

    public void MusicOff()
    {
        mixer.SetFloat("Music", -80);
        PlayerPrefs.SetInt("GetMusicState",0);
    }

    public void SoundOn()
    {
        mixer.SetFloat("SFX", 0);
        PlayerPrefs.SetInt("GetSoundState",1);
    }

    public void SoundOff()
    {
        mixer.SetFloat("SFX", -80);
        PlayerPrefs.SetInt("GetSoundState",0);
    }
}

[Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = .75f;
    [Range(0f, 1f)]
    public float volumeVariance = .1f;

    [Range(.1f, 3f)]
    public float pitch = 1f;
    [Range(0f, 1f)]
    public float pitchVariance = .1f;

    public bool loop = false;

    public AudioMixerGroup mixerGroup;

    [HideInInspector]
    public AudioSource source;

}
