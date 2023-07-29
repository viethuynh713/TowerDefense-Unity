
using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.LocalDatabase;
using MythicEmpire.Manager.MythicEmpire.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<Sound> _sounds;
    private AudioSource _vfx;
    private AudioSource _music;
    [Inject] private ISettingLocalData _settingLocalData;

    void Start()
    {
        _vfx = gameObject.AddComponent<AudioSource>();
        _music = gameObject.AddComponent<AudioSource>();
        ApplySoundSetting(null);
        if (SceneManager.GetActiveScene().name.Equals("Lobby") || SceneManager.GetActiveScene().name.Equals("Menu"))
        {
            PlayLobbyMusic();
        }
        else if (SceneManager.GetActiveScene().name.Equals("Game"))
        {
            PlayIngameMusic();
        }
        EventManager.Instance.RegisterListener(EventID.ChangeSoundSetting,ApplySoundSetting);
    }

    public Sound GetSound(SoundID id)
    {
        var listSoundsMatch = _sounds.FindAll(sound => sound.Id == id);
        return listSoundsMatch[Random.Range(0, listSoundsMatch.Count)];
    }

    private void ApplySoundSetting(object obj)
    {
        var setting = _settingLocalData.GetSettingData();

        _vfx.volume = setting.EffectSoundVolume;

        _music.volume = setting.MusicSoundVolume;


    }

    private void PlayIngameMusic()
    {
        _music.Stop();
        var sound = GetSound(SoundID.InGame);
        _music.clip = sound.SourceAudioClip;
        _music.loop = sound.Loop;
        _music.priority = sound.Priority;
        _music.playOnAwake = sound.PlayOnWake;
        _music.Play();
    }

    private void PlayLobbyMusic()
    {
        _music.Stop();
        var sound = GetSound(SoundID.Lobby);
        _music.clip = sound.SourceAudioClip;
        _music.loop = sound.Loop;
        _music.priority = sound.Priority;
        _music.playOnAwake = sound.PlayOnWake;
        _music.Play();
    }

}
