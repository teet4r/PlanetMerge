using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmOnOff : ButtonOnOff
{
    void OnEnable()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKey.BGM_ON) == 0) // bgm�� ���� ä�� �����ߴ� ���
        {
            isOn = false;
            image.sprite = OffSprite;
        }
        else
        {
            isOn = true;
            image.sprite = OnSprite;
        }
    }

    public override void OnClickEvent()
    {
        isOn = !isOn;
        image.sprite = isOn ? OnSprite : OffSprite;
        SoundManager.instance.bgmAudio.mute = !isOn;
        PlayerPrefs.SetInt(PlayerPrefsKey.BGM_ON, isOn ? 1 : 0);
    }
}
