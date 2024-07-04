using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIRemindPopup : UI
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private Button _firstButton;
    [SerializeField] private Text _firstButtonDescription;
    [SerializeField] private Button _secondButton;
    [SerializeField] private Text __secondButtonDescription;

    private bool _activeFirstButton = false;
    private bool _activeSecondButton = false;

    public override void Show()
    {
        base.Show();

        _firstButton.gameObject.SetActive(_activeFirstButton);
        _secondButton.gameObject.SetActive(_activeSecondButton);
    }

    public override void Hide()
    {
        base.Hide();

        _activeFirstButton = false;
        _activeSecondButton = false;
    }

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

    public UIRemindPopup SetFirstButton(string description, UnityAction call = default)
    {
        _activeFirstButton = true;
        _firstButtonDescription.text = description;
        _firstButton.onClick.RemoveAllListeners();
        if (call == default)
            _firstButton.onClick.AddListener(() => Hide());
        else
            _firstButton.onClick.AddListener(call);

        return this;
    }

    public UIRemindPopup SetSecondButton(string description, UnityAction call = default)
    {
        _activeSecondButton = true;
        __secondButtonDescription.text = description;
        _secondButton.onClick.RemoveAllListeners();
        if (call == default)
            _secondButton.onClick.AddListener(() => Hide());
        else
            _secondButton.onClick.AddListener(call);

        return this;
    }
}
