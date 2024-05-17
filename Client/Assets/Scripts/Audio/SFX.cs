using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFX : SingletonBehaviour<SFX>
{
    private static AudioSource _audioSource;
    private static Dictionary<Sfx, AudioClip> _sfxs = new();

    public static bool Mute
    {
        get => _audioSource.mute;
        set
        {
            _audioSource.mute = value;
            PlayerPrefs.SetInt(PlayerPrefsKey.SFX_ON, value ? 0 : 1);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (!PlayerPrefs.HasKey(PlayerPrefsKey.SFX_ON))
            PlayerPrefs.SetInt(PlayerPrefsKey.SFX_ON, 1);

        _audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt(PlayerPrefsKey.SFX_ON) == 0)
            Mute = true;
    }

    public static void Play(Sfx sfx)
    {
        if (!_sfxs.TryGetValue(sfx, out AudioClip clip))
            _sfxs.Add(sfx, clip = Resources.Load<AudioClip>($"AudioClips/Sfx/{sfx}"));

        _audioSource.PlayOneShot(clip);
    }
}
