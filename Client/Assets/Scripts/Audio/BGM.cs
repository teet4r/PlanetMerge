using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGM : SingletonBehaviour<BGM>
{
    private static AudioSource _audioSource;
    private static Dictionary<Bgm, AudioClip> _bgms = new();

    public static bool Mute
    {
        get => _audioSource.mute;
        set
        {
            _audioSource.mute = value;
            PlayerPrefs.SetInt(PlayerPrefsKey.BGM_ON, value ? 0 : 1);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _audioSource = GetComponent<AudioSource>();
        
        if (!PlayerPrefs.HasKey(PlayerPrefsKey.BGM_ON))
            PlayerPrefs.SetInt(PlayerPrefsKey.BGM_ON, 1);

        if (PlayerPrefs.GetInt(PlayerPrefsKey.BGM_ON) == 0)
            Mute = true;
    }

    public static void Play(Bgm bgm)
    {
        if (!_bgms.TryGetValue(bgm, out AudioClip clip))
            _bgms.Add(bgm, clip = Resources.Load<AudioClip>($"AudioClips/Bgm/{bgm}"));

        if (clip == _audioSource.clip)
            return;

        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public static void Stop()
    {
        if (!_audioSource.isPlaying)
            return;

        _audioSource.Stop();
    }
}
