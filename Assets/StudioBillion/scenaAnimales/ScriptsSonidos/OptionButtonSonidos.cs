using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionButtonSonidos : MonoBehaviour
{
    private Button m_buttons;
    private Image m_images;
    private Color m_originalColors;
    private Image m_buttonImages = null;
    //private AudioSource m_audioSources;
    public OptionSonidos Option { get; private set; }

    private void Awake()
    {
        m_buttons = GetComponent<Button>();
        m_images = GetComponent<Image>();
        m_buttonImages = GetComponent<Image>();
        m_originalColors = m_images.color;
    }

    public void Construtc(OptionSonidos optionSound, Action<OptionButtonSonidos> callback)
    {
        m_buttons.onClick.RemoveAllListeners();

        m_buttonImages.sprite = optionSound.image;
        m_buttons.enabled = true;
        m_images.color = m_originalColors;
        Option = optionSound;

        m_buttons.onClick.AddListener(() => callback(this));

    }

    public void SetColor(Color c)
    {
        m_buttons.enabled = false;
        m_images.color = c;
    }

}
