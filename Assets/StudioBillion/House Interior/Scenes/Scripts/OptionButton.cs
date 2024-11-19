using System;
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

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_image = GetComponent<Image>();
        m_text = transform.GetChild(0).GetComponent<Text>();
        m_originalColor = m_image.color;
    }

    public void Construtc(Option option, Action<OptionButton> callback)
    {
        // Limpiar los listeners existentes para evitar múltiples asignaciones
        m_button.onClick.RemoveAllListeners();

        // Configurar el texto del botón y el color original
        m_text.text = option.text;
        m_button.enabled = true;
        m_image.color = m_originalColor;

        // Configurar la opción
        Option = option;

        // Agregar el listener al botón
        m_button.onClick.AddListener(() => callback(this));

        // Aquí podrías añadir lógica adicional para manejar la opción correcta,
        // por ejemplo, cambiar el color del botón si la opción es correcta
        // if (option.correct)
        // {
        //     m_image.color = Color.green; // Ejemplo: color verde para respuestas correctas
        // }
    }

    public void SetColor(Color c)
    {
        m_button.enabled = false;
        m_image.color = c;
    }
}
