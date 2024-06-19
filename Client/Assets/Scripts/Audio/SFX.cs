using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFX : SingletonBehaviour<SFX>
{
    private static AudioSource _audioSource;
    private static Dictionary<Sfx, AudioClip> _sfxs = new();

    public static float Volume
    {
        get => _audioSource.volume;
        set
        {
            _audioSource.volume = value;
            PlayerPrefs.SetFloat(PlayerPrefsKey.SFX_VALUE, value);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _audioSource = GetComponent<AudioSource>();

        _audioSource.volume = PlayerPrefs.GetFloat(PlayerPrefsKey.SFX_VALUE, 1f);
    }

    public static void Play(Sfx sfx)
    {
        if (!_sfxs.TryGetValue(sfx, out AudioClip clip))
            _sfxs.Add(sfx, clip = Resources.Load<AudioClip>($"AudioClips/Sfxs/{sfx}"));

        _audioSource.PlayOneShot(clip);
    }

    public static void PlayButtonClick() => Play(Sfx.Button);
}
