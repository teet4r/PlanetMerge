using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGM : SingletonBehaviour<BGM>
{
    private static AudioSource _audioSource;
    private static Dictionary<Bgm, AudioClip> _bgms = new();

    public static float Volume
    {
        get => _audioSource.volume;
        set
        {
            _audioSource.volume = value;
            PlayerPrefs.SetFloat(PlayerPrefsKey.BGM_VALUE, value);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _audioSource = GetComponent<AudioSource>();
    }

    public static void Initialize()
    {
        _bgms.Clear();
        _audioSource.volume = PlayerPrefs.GetFloat(PlayerPrefsKey.BGM_VALUE, 1f);
    }

    public static void Play(Bgm bgm)
    {
        if (!_bgms.TryGetValue(bgm, out AudioClip clip))
            _bgms.Add(bgm, clip = Resources.Load<AudioClip>($"AudioClips/Bgms/{bgm}"));

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
