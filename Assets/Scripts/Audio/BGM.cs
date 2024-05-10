using Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGM : SingletonBehaviour<BGM>
{
    private static AudioSource audioSource;
    private static Dictionary<Bgm, AudioClip> _bgms = new();

    public static bool Mute
    {
        get => audioSource.mute;
        set
        {
            audioSource.mute = value;
            PlayerPrefs.SetInt(PlayerPrefsKey.BGM_ON, value ? 0 : 1);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (!PlayerPrefs.HasKey(PlayerPrefsKey.BGM_ON))
            PlayerPrefs.SetInt(PlayerPrefsKey.BGM_ON, 1);

        audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt(PlayerPrefsKey.BGM_ON) == 0)
            Mute = true;
    }

    public static void Play(Bgm bgm, bool loop = true)
    {
        if (!_bgms.TryGetValue(bgm, out AudioClip clip))
            _bgms.Add(bgm, clip = Resources.Load<AudioClip>($"AudioClips/Bgm/{bgm}"));

        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public static void Stop()
    {
        if (!audioSource.isPlaying)
            return;

        audioSource.Stop();
    }
}
