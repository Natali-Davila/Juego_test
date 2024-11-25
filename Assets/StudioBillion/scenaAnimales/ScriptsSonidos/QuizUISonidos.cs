using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizUISonidos : MonoBehaviour
{
    [SerializeField] private Image m_questionImages = null;
    [SerializeField] private List<OptionButtonSonidos> m_buttonLists = null;

    public void Construtc(QuestionSonidos q, Action<OptionButtonSonidos> callback)
    {
        if (q == null)
        {
            Debug.LogError("Question object is null");
            return;
        }

        if (m_buttonLists == null || m_buttonLists.Count != q.optionSound.Count)
        {
            Debug.LogError($"Mismatch between button list count ({m_buttonLists?.Count ?? 0}) and options count ({q.optionSound.Count})");
            return;
        }

        if (m_questionImages != null)
        {
            if (q.image != null)
            {
                m_questionImages.sprite = q.image;
            }
        }
        else
        {
            Debug.LogError("Question Image component is not assigned");
        }

        List<OptionSonidos> shuffledOptions = ShuffleOptions(q.optionSound);

        for (int n = 0; n < m_buttonLists.Count; n++)
        {
            if (n >= shuffledOptions.Count)
            {
                Debug.LogError($"No option available for button index {n}");
                continue;
            }

            m_buttonLists[n].Construtc(shuffledOptions[n], callback);
        }
    }

    private List<OptionSonidos> ShuffleOptions(List<OptionSonidos> options)
    {
        List<OptionSonidos> shuffledOptionss = new List<OptionSonidos>(options);
        System.Random rng = new System.Random();
        int n = shuffledOptionss.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            OptionSonidos value = shuffledOptionss[k];
            shuffledOptionss[k] = shuffledOptionss[n];
            shuffledOptionss[n] = value;
        }
        return shuffledOptionss;
    }

}
