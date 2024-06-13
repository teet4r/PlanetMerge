using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class CustomToggle : MonoBehaviour
{
    public bool IsOn
    {
        get => toggle.isOn;
        set => toggle.isOn = value;
    }

    protected Toggle toggle;

    [SerializeField] protected Image _image;
    [SerializeField] protected Sprite _onSprite;
    [SerializeField] protected Sprite _offSprite;

    protected virtual void Awake()
    {
        toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(isOn => _image.sprite = isOn ? _offSprite : _onSprite);
    }

    public void AddListener(UnityAction<bool> call) => toggle.onValueChanged.AddListener(call);
}
