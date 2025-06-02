using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class OptionButton : MonoBehaviour
{
    private Text m_text;
    private Button m_button;
    private Image m_image;
    private Color m_originalColor;

    public Option Option { get; private set; }

    private static readonly Dictionary<string, Color> colorMap = new Dictionary<string, Color>()
    {
        { "red", Color.red },
        { "rojo", Color.red },

        { "green", Color.green },
        { "verde", Color.green },

        { "blue", new Color(0.53f, 0.81f, 0.98f) },
        { "azul", new Color(0.53f, 0.81f, 0.98f) },

        { "yellow", Color.yellow },
        { "amarillo", Color.yellow },

        { "purple", new Color(0.75f, 0.58f, 0.85f) },
        { "morado", new Color(0.75f, 0.58f, 0.85f) },

        { "white", Color.white },
        { "blanco", Color.white },

        { "gray", Color.gray },
        { "gris", Color.gray },

        { "cyan", Color.cyan },
        { "cian", Color.cyan },

        { "magenta", Color.magenta },

        { "orange", new Color(1f, 0.5f, 0f) },
        { "naranja", new Color(1f, 0.5f, 0f) }
    };

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_image = GetComponent<Image>();
        m_text = transform.GetChild(0).GetComponent<Text>();
        m_originalColor = m_image.color;
    }

    public void Construtc(Option option, Action<OptionButton> callback)
    {
        m_button.onClick.RemoveAllListeners();

        m_text.text = option.text;
        m_button.enabled = true;
        m_image.color = GetColorFromText(option.text);
        Option = option;

        m_button.onClick.AddListener(() =>
        {
            callback(this); 
        });
    }

    public void SetColor(Color c)
    {
        m_button.enabled = false;
        m_image.color = c;
    }

    private Color GetColorFromText(string colorName)
    {
        string key = colorName.ToLower().Trim();

        if (colorMap.TryGetValue(key, out Color mappedColor))
        {
            return mappedColor;
        }

        Debug.LogWarning($"Color no reconocido: {colorName}. Usando color original.");
        return m_originalColor;
    }
}
