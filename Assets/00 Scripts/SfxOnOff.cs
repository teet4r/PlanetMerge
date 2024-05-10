using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxOnOff : ButtonOnOff
{
    void OnEnable()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKey.SFX_ON) == 0) // bgm�� ���� ä�� �����ߴ� ���
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
        SoundManager.instance.sfxAudio.mute = !isOn;
        PlayerPrefs.SetInt(PlayerPrefsKey.SFX_ON, isOn ? 1 : 0);
    }
}
