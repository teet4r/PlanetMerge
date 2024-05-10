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
    /// 원하는 sfx이름을 입력하면 재생시켜주는 함수
    /// </summary>
    public void Play(string sfxName)
    {
        // sfxDictionary에 실행하려는 sfxName이 등록돼있지 않다면 아무것도 하지않고 함수 종료
        if (!sfxDictionary.ContainsKey(sfxName))
            return;

        // sfx는 일회성이므로 PlayOneShot()함수로 실행
        audioSource.PlayOneShot(sfxDictionary[sfxName]);
    }

    public void Play(Sfx sfx)
    {
        // sfx는 일회성이므로 PlayOneShot()함수로 실행
        audioSource.PlayOneShot(clips[(int)sfx]);
    }

    public void Play(int sfxIndex)
    {
        if (sfxIndex < 0 || sfxIndex >= clips.Length)
        {
            Debug.Log("sfxIndex Error.");
            return;
        }
        // sfx는 일회성이므로 PlayOneShot()함수로 실행
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
    Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>(); // key와 value를 가진 dictionary구조
}
