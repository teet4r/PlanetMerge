using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UITestLogPanelPopup : UI
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void Bind(string log)
    {
        _text.text = log;
    }
}
