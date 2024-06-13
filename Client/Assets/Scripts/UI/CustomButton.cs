using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    public bool interactable
    {
        get => button.interactable;
        set
        {
            if (button.interactable != value)
                _image.sprite = value ? _enabledSprite : _disabledSprite;
            button.interactable = value;
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
