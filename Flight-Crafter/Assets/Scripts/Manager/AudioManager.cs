using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource rocketLoopSource;
    [SerializeField] private AudioSource carLoopSource;
    [SerializeField] private AudioSource flyLoopSource;
    [SerializeField] private AudioSource waterLoopSource; // 水中ループ用のAudioSource

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

    // SFXをPlay/Stop方式で再生・停止する
    public void PlaySFX(string soundName, float pitch = 1f, float startTime = 0f)
    {
        foreach (var sound in sfxSounds)
        {
            if (sound.soundName == soundName)
            {
                sfxSource.pitch = pitch;
                sfxSource.clip = sound.clip;
                sfxSource.volume = sound.volume;
                sfxSource.loop = false;

                // ★ 再生開始位置を設定
                sfxSource.time = Mathf.Clamp(startTime, 0f, sound.clip.length);

                sfxSource.Play();
                return;
            }
        }
        Debug.LogWarning($"SFX '{soundName}' が見つかりません");
    }


    public void StopSFX(string soundName)
    {
        foreach (var sound in sfxSounds)
        {
            if (sound.soundName == soundName)
            {
                if (sfxSource.isPlaying && sfxSource.clip == sound.clip)
                {
                    sfxSource.Stop();
                }
                return;
            }
        }
    }

    public async void PlayRocketLoopSFX(string soundName)
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
                rocketLoopSource.loop = true;
                rocketLoopSource.volume = 0f;
                rocketLoopSource.Play();
                await FadeRocketLoopVolume(0f, sound.volume, 0.2f);
                return;
            }
        }
        Debug.LogWarning($"ループSFX '{soundName}' が見つかりません");
    }

    public async void StopRocketLoopSFX()
    {
        if (rocketLoopSource.isPlaying)
        {
            float currentVolume = rocketLoopSource.volume;
            await FadeRocketLoopVolume(currentVolume, 0f, 0.2f);
            rocketLoopSource.Stop();
        }
    }

    private async UniTask FadeRocketLoopVolume(float from, float to, float duration)
    {
        float elapsed = 0f;
        rocketLoopSource.volume = from;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rocketLoopSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            await UniTask.Yield();
        }
        rocketLoopSource.volume = to;
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

    public void PlayFlyLoopSFX(string soundName)
    {
        if (flyLoopSource.isPlaying && flyLoopSource.clip != null && flyLoopSource.clip.name == soundName)
        {
            return;
        }

        foreach (var sound in sfxSounds)
        {
            if (sound.soundName == soundName)
            {
                flyLoopSource.clip = sound.clip;
                flyLoopSource.volume = sound.volume;
                flyLoopSource.loop = true;
                flyLoopSource.Play();
                return;
            }
        }
        Debug.LogWarning($"FlyLoopSFX '{soundName}' が見つかりません");
    }

    public void StopFlyLoopSFX()
    {
        if (flyLoopSource.isPlaying)
        {
            flyLoopSource.Stop();
        }
    }

    public void SetFlyLoopVolume(float volume)
    {
        flyLoopSource.volume = Mathf.Clamp01(volume);
    }

    public void PlayWaterLoopSFX(string soundName)
    {
        if (waterLoopSource.isPlaying)
        {
            return; // すでに再生中なら無視
        }

        foreach (var sound in sfxSounds)
        {
            if (sound.soundName == soundName)
            {
                waterLoopSource.clip = sound.clip;
                waterLoopSource.volume = sound.volume;
                waterLoopSource.loop = true;
                waterLoopSource.Play();
                return;
            }
        }
        Debug.LogWarning($"WaterLoopSFX '{soundName}' が見つかりません");
    }
    public void StopWaterLoopSFX()
    {
        if (waterLoopSource.isPlaying)
        {
            waterLoopSource.Stop();
        }
    }

}