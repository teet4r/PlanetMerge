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
    public bool Interactable
    {
        get => toggle.interactable;
        set => toggle.interactable = value;
    }

    [SerializeField] private Image _image;
    private Sprite _originSprite;

    protected Toggle toggle;

    protected virtual void Awake()
    {
        toggle = GetComponent<Toggle>();

        _originSprite = _image.sprite;
    }

    public void AddListener(UnityAction<bool> call) => toggle.onValueChanged.AddListener(call);

    public void SetSprite(SpriteName spriteName) => _image.sprite = ResourceLoader.LoadSprite(spriteName);
}
