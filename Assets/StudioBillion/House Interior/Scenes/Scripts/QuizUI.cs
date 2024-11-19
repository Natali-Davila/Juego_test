using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    [SerializeField] private Text m_question = null;
    [SerializeField] private List<OptionButton> m_buttonList = null;
    [SerializeField] private Image m_questionImage = null; // Agregado para la imagen
    

    public void Construtc(Question q, Action<OptionButton> callback)
    {
        if (q == null)
        {
            Debug.LogError("Question object is null");
            return;
        }

        if (m_buttonList == null || m_buttonList.Count != q.options.Count)
        {
            Debug.LogError($"Mismatch between button list count ({m_buttonList?.Count ?? 0}) and options count ({q.options.Count})");
            return;
        }

        if (m_question != null)
        {
            m_question.text = q.text;
        }
        else
        {
            Debug.LogError("Question Text component is not assigned");
        }

        // Establecer la imagen correspondiente a la pregunta
        if (m_questionImage != null)
        {
            m_questionImage.sprite = q.image; // Asegúrate de que 'image' es de tipo 'Sprite'
        }
        else
        {
            Debug.LogError("Question Image component is not assigned");
        }

        // Mezclar las opciones de manera aleatoria
        List<Option> shuffledOptions = ShuffleOptions(q.options);

        // Asignar los datos a los botones de opción
        for (int n = 0; n < m_buttonList.Count; n++)
        {
            // Check if the index is within bounds for options
            if (n >= shuffledOptions.Count)
            {
                Debug.LogError($"No option available for button index {n}");
                continue;
            }

            m_buttonList[n].Construtc(shuffledOptions[n], callback);
        }
    }

    private List<Option> ShuffleOptions(List<Option> options)
    {
        List<Option> shuffledOptions = new List<Option>(options);
        System.Random rng = new System.Random();
        int n = shuffledOptions.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Option value = shuffledOptions[k];
            shuffledOptions[k] = shuffledOptions[n];
            shuffledOptions[n] = value;
        }
        return shuffledOptions;
    }
}
