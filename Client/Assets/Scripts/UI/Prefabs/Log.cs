using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Log : MonoBehaviour
{
    private static TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        transform.SetAsLastSibling();
    }

    public static void Bind(string log)
    {
        _text.text += $"{log}\n";
    }
}
