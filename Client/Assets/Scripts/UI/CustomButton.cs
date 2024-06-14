using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour
{
    public bool Interactable
    {
        get => button.interactable;
        set => button.interactable = value;
    }

    protected Button button;
    protected Sprite originSprite;

    [SerializeField] private Image _image;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();

        originSprite = _image.sprite;
    }

    public void AddListener(UnityAction call) => button.onClick.AddListener(call);

    public void SetSprite(SpriteName spriteName)
    {
        _image.sprite = ResourceLoader.LoadSprite(spriteName);
    }
}
