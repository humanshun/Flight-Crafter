using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource rocketLoopSource;
    [SerializeField] private AudioSource carLoopSource;

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private SoundData[] bgmSounds; // BGM用のSoundData
    [SerializeField] private SoundData[] sfxSounds; // SFX用のSound


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerPrefs.DeleteKey("BGMVolume");
        PlayerPrefs.DeleteKey("SFXVolume");
        PlayerPrefs.Save();

        // 保存された音量設定を適用
        SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 0.5f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));

        // ゲーム開始時にBGM再生
        if (!string.IsNullOrEmpty(bgmSounds[0].soundName))
        {
            PlayBGM(bgmSounds[0].soundName);
        }
    }
    public void SetBGMVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("BGMVolume", dB);
    }

    public void SetSFXVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("SFXVolume", dB);
    }

    public void PlayBGM(string soundName)
    {
        foreach (var sound in bgmSounds)
        {
            if (sound.soundName == soundName)
            {
                bgmSource.clip = sound.clip;
                bgmSource.volume = sound.volume;
                bgmSource.loop = sound.loop;
                bgmSource.Play();
                return;
            }
        }
        Debug.LogWarning($"BGM '{soundName}' が見つかりません");
    }

    public void PlaySFX(string soundName)
    {
        foreach (var sound in sfxSounds)
        {
            if (sound.soundName == soundName)
            {
                sfxSource.PlayOneShot(sound.clip, sound.volume);
                return;
            }
        }
        Debug.LogWarning($"SFX '{soundName}' が見つかりません");
    }

    public void PlayLoopSFX(string soundName)
    {
        if (rocketLoopSource.isPlaying && rocketLoopSource.clip != null && rocketLoopSource.clip.name == soundName)
        {
            return; // すでに同じ音が再生中なら再生しない
        }

        foreach (var sound in sfxSounds)
        {
            if (sound.soundName == soundName)
            {
                rocketLoopSource.clip = sound.clip;
                rocketLoopSource.volume = sound.volume;
                rocketLoopSource.loop = true;
                rocketLoopSource.Play();
                return;
            }
        }
        Debug.LogWarning($"ループSFX '{soundName}' が見つかりません");
    }

    public void StopLoopSFX()
    {
        if (rocketLoopSource.isPlaying)
        {
            rocketLoopSource.Stop();
        }
    }

    public void PlayCarLoopSFX(string soundName)
    {
        if (carLoopSource.isPlaying && carLoopSource.clip != null && carLoopSource.clip.name == soundName)
        {
            return; // すでに再生中なら無視
        }

        foreach (var sound in sfxSounds)
        {
            if (sound.soundName == soundName)
            {
                carLoopSource.clip = sound.clip;
                carLoopSource.volume = sound.volume;
                carLoopSource.loop = true;
                carLoopSource.Play();
                return;
            }
        }
        Debug.LogWarning($"CarLoopSFX '{soundName}' が見つかりません");
    }

    public void StopCarLoopSFX()
    {
        if (carLoopSource.isPlaying)
        {
            carLoopSource.Stop();
        }
    }

    public void SetCarLoopVolume(float volume)
    {
        carLoopSource.volume = Mathf.Clamp01(volume);
    }

}