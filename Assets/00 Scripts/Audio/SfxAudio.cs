using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sfx
{
    LEVELUP, ATTACH, BUTTON, NEXT, GAMEOVER
}

[RequireComponent(typeof(AudioSource))]
public class SfxAudio : MonoBehaviour
{
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt(PlayerPrefsKey.SFX_ON) == 0)
            mute = true;

        for (int i = 0; i < clips.Length; i++)
            sfxDictionary.Add(clips[i].name, clips[i]);
    }

    /// <summary>
    /// ���ϴ� sfx�̸��� �Է��ϸ� ��������ִ� �Լ�
    /// </summary>
    public void Play(string sfxName)
    {
        // sfxDictionary�� �����Ϸ��� sfxName�� ��ϵ����� �ʴٸ� �ƹ��͵� �����ʰ� �Լ� ����
        if (!sfxDictionary.ContainsKey(sfxName))
            return;

        // sfx�� ��ȸ���̹Ƿ� PlayOneShot()�Լ��� ����
        audioSource.PlayOneShot(sfxDictionary[sfxName]);
    }

    public void Play(Sfx sfx)
    {
        // sfx�� ��ȸ���̹Ƿ� PlayOneShot()�Լ��� ����
        audioSource.PlayOneShot(clips[(int)sfx]);
    }

    public void Play(int sfxIndex)
    {
        if (sfxIndex < 0 || sfxIndex >= clips.Length)
        {
            Debug.Log("sfxIndex Error.");
            return;
        }
        // sfx�� ��ȸ���̹Ƿ� PlayOneShot()�Լ��� ����
        audioSource.PlayOneShot(clips[sfxIndex]);
    }

    public bool mute
    {
        get { return audioSource.mute; }
        set
        {
            audioSource.mute = value;
            PlayerPrefs.SetInt(PlayerPrefsKey.SFX_ON, value ? 0 : 1);
        }
    }

    [SerializeField]
    AudioClip[] clips;

    AudioSource audioSource;
    Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>(); // key�� value�� ���� dictionary����
}
