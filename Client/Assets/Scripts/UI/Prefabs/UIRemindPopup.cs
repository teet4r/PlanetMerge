using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIRemindPopup : UI
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    public UIRemindPopup SetTitle(string title)
    {
        _titleText.text = title;

        return this;
    }

    public UIRemindPopup SetDescription(string description)
    {
        _descriptionText.text = description;

        return this;
    }

    public UIRemindPopup SetYesButton(UnityAction call)
    {
        _yesButton.onClick.RemoveAllListeners();
        if (call == null)
            _yesButton.onClick.AddListener(() => Hide());
        else
            _yesButton.onClick.AddListener(call);

        return this;
    }

    public UIRemindPopup SetNoButton(UnityAction call)
    {
        _noButton.onClick.RemoveAllListeners();
        if (call == null)
            _noButton.onClick.AddListener(() => Hide());
        else
            _noButton.onClick.AddListener(call);

        return this;
    }
}
