using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    void Awake()
    {
        if (instance == null) //soundManager 최초 할당
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //씬이 바뀔 때 자신이 파괴되는 것을 방지
        }
        else
            Destroy(gameObject); // 이 오브젝트가 존재하는 씬으로 올 때 instance에 또 할당이 되기 때문인데
                                 // 이미 instance에 할당이 되어있다면 사운드매니저는 활성화 되어있다는 뜻이므로
                                 // instance는 null이 아님. 따라서 새로 생성된 해당 오브젝트를 제거해주어야 함
        Initialize();
    }

    /// <summary>
    /// 사운드 매니저 초기화
    /// </summary>
    void Initialize()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKey.REGISTERED))
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.REGISTERED, 1);
            PlayerPrefs.SetInt(PlayerPrefsKey.BGM_ON, 1);
            PlayerPrefs.SetInt(PlayerPrefsKey.SFX_ON, 1);
        }
    }

    public BgmAudio bgmAudio;
    public SfxAudio sfxAudio;
}
