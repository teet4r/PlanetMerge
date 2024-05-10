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

        // 인스펙터에서 넣어준 audioClip들을 bgm 및 sfx dictionary에 넣어줌으로써
        // O(1)시간에 원하는 클립을 재생시킬 수 있도록 함.
        // O(1)시간은 자료를 탐색할 때 가장 빠른 속도를 보여주는 지표(시간복잡도).
        for (int i = 0; i < clips.Length; i++)
            bgmDictionary.Add(clips[i].name, clips[i]);
    }

    /// <summary>
    /// 원하는 bgm이름을 입력하면 재생시켜주는 함수
    /// 두번째인자는 루프 인자, 생략할 시 자동으로 loop됨
    /// </summary>
    /// <param name="bgmName"></param>
    /// <param name="loop"></param>
    public void Play(string bgmName, bool loop = true)
    {
        // bgmDictionary에 실행하려는 bgmName이 등록돼있지 않다면 아무것도 하지않고 함수 종료
        if (!bgmDictionary.ContainsKey(bgmName))
            return;

        audioSource.loop = loop; // 루프할지 말지 인자로 결정
        audioSource.clip = bgmDictionary[bgmName];
        audioSource.Play();
    }

    public void Play(Bgm bgm, bool loop = true)
    {
        audioSource.loop = loop; // 루프할지 말지 인자로 결정
        audioSource.clip = clips[(int)bgm];
        audioSource.Play();
    }

    /// <summary>
    /// 실행중인 bgm멈춤.
    /// 이미 실행중인 bgm이 없으면 아무것도 실행하지 않고 종료
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
    Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>(); // key와 value를 가진 dictionary구조
}
