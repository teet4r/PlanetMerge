using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    public new bool enabled
    {
        get => button.enabled;
        set
        {
            if (button.enabled != value)
                _image.sprite = value ? _enabledSprite : _disabledSprite;
            button.enabled = value;
        }
    }

    [SerializeField] private Image _image;
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;

    protected Button button;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
    }

    public void AddListener(UnityAction call) => button.onClick.AddListener(call);
}
