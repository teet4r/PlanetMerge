using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ButtonSfx
{
    public Button button;
    public Sfx sfx;
}

public class AddButtonListener : MonoBehaviour
{
    void OnEnable()
    {
        for (int i = 0; i < buttonSfxes.Length; i++)
        {
            var delegateI = i;
            buttonSfxes[i].button.onClick.AddListener(delegate { SoundManager.instance.sfxAudio.Play(buttonSfxes[delegateI].sfx); });
        }
    }

    [SerializeField]
    ButtonSfx[] buttonSfxes;
}
