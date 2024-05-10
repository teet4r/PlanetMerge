using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Bgm
{
    BGM1, BGM2, BGM3, BGM4
}

[RequireComponent(typeof(AudioSource))]
public class BgmAudio : MonoBehaviour
{
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt(PlayerPrefsKey.BGM_ON) == 0)
            mute = true;

        // �ν����Ϳ��� �־��� audioClip���� bgm �� sfx dictionary�� �־������ν�
        // O(1)�ð��� ���ϴ� Ŭ���� �����ų �� �ֵ��� ��.
        // O(1)�ð��� �ڷḦ Ž���� �� ���� ���� �ӵ��� �����ִ� ��ǥ(�ð����⵵).
        for (int i = 0; i < clips.Length; i++)
            bgmDictionary.Add(clips[i].name, clips[i]);
    }

    /// <summary>
    /// ���ϴ� bgm�̸��� �Է��ϸ� ��������ִ� �Լ�
    /// �ι�°���ڴ� ���� ����, ������ �� �ڵ����� loop��
    /// </summary>
    /// <param name="bgmName"></param>
    /// <param name="loop"></param>
    public void Play(string bgmName, bool loop = true)
    {
        // bgmDictionary�� �����Ϸ��� bgmName�� ��ϵ����� �ʴٸ� �ƹ��͵� �����ʰ� �Լ� ����
        if (!bgmDictionary.ContainsKey(bgmName))
            return;

        audioSource.loop = loop; // �������� ���� ���ڷ� ����
        audioSource.clip = bgmDictionary[bgmName];
        audioSource.Play();
    }

    public void Play(Bgm bgm, bool loop = true)
    {
        audioSource.loop = loop; // �������� ���� ���ڷ� ����
        audioSource.clip = clips[(int)bgm];
        audioSource.Play();
    }

    /// <summary>
    /// �������� bgm����.
    /// �̹� �������� bgm�� ������ �ƹ��͵� �������� �ʰ� ����
    /// </summary>
    public void Stop()
    {
        if (!audioSource.isPlaying)
            return;

        audioSource.Stop();
    }

    public bool mute
    {
        get { return audioSource.mute; }
        set
        {
            audioSource.mute = value;
            PlayerPrefs.SetInt(PlayerPrefsKey.BGM_ON, value ? 0 : 1);
        }
    }

    [SerializeField]
    AudioClip[] clips;

    AudioSource audioSource;
    Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>(); // key�� value�� ���� dictionary����
}
