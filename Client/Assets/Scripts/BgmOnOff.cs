using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmOnOff : ButtonOnOff
{
    void OnEnable()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsKey.BGM_ON) == 0) // bgm이 꺼진 채로 저장했던 경우
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
        BGM.Mute = !isOn;
        PlayerPrefs.SetInt(PlayerPrefsKey.BGM_ON, isOn ? 1 : 0);
    }
}
