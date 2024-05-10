using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public abstract class ButtonOnOff : MonoBehaviour
{
    void Awake()
    {
        image = GetComponent<Image>();
    }

    public abstract void OnClickEvent();

    public bool isOn { get; protected set; }

    [SerializeField]
    protected Sprite OnSprite;
    [SerializeField]
    protected Sprite OffSprite;

    protected Image image;
}
