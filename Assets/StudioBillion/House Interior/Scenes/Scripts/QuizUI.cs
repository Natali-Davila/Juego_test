using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    [SerializeField] private Text m_question = null;
    [SerializeField] private List<OptionButton> m_buttonList = null;
    [SerializeField] private Image m_questionImage = null;
    [SerializeField] private AudioSource m_audioSource = null;
    [SerializeField] private Button m_imageButton = null;

    public void Construtc(Question q, Action<OptionButton> callback)
    {
        if (q == null)
        {
            Debug.LogError("Question object is null");
            return;
        }

        if (m_buttonList == null || m_buttonList.Count < 2)
        {
            Debug.LogError("Se necesitan al menos 2 botones en la interfaz");
            return;
        }

        if (q.options == null || q.options.Count < 2)
        {
            Debug.LogError("Se necesitan al menos 2 opciones para mostrar");
            return;
        }

        if (m_question != null)
            m_question.text = q.text;
        else
            Debug.LogError("Question Text component is not assigned");

        if (m_questionImage != null)
            m_questionImage.sprite = q.image;
        else
            Debug.LogError("Question Image component is not assigned");

        List<Option> twoOptions = GetCorrectAndIncorrectOptions(q.options);
        ShuffleList(twoOptions);

        for (int i = 0; i < m_buttonList.Count; i++)
        {
            if (i < 2)
            {
                m_buttonList[i].gameObject.SetActive(true);
                m_buttonList[i].Construtc(twoOptions[i], callback);
            }
            else
            {
                m_buttonList[i].gameObject.SetActive(false);
            }
        }

        if (m_imageButton != null && q.audioClip != null)
        {
            m_imageButton.onClick.RemoveAllListeners();
            m_imageButton.onClick.AddListener(() =>
            {
                m_audioSource.clip = q.audioClip;
                m_audioSource.Play();
            });
        }
        else
        {
            Debug.LogWarning("Botón de imagen no asignado o audioClip nulo.");
        }
    }

    private List<Option> GetCorrectAndIncorrectOptions(List<Option> options)
    {
        Option correctOption = options.Find(o => o.correct);
        if (correctOption == null)
        {
            Debug.LogError("No se encontró opción correcta en la lista");
            return options.GetRange(0, Math.Min(2, options.Count));
        }

        List<Option> incorrectOptions = options.FindAll(o => !o.correct);
        if (incorrectOptions.Count == 0)
        {
            Debug.LogError("No hay opciones incorrectas disponibles");
            return new List<Option> { correctOption, null };
        }

        System.Random rng = new System.Random();
        Option randomIncorrect = incorrectOptions[rng.Next(incorrectOptions.Count)];

        return new List<Option> { correctOption, randomIncorrect };
    }

    private void ShuffleList<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class QuizUI : MonoBehaviour
//{
//    [SerializeField] private Text m_question = null;
//    [SerializeField] private List<OptionButton> m_buttonList = null;
//    [SerializeField] private Image m_questionImage = null;
//    [SerializeField] private AudioSource m_audioSource = null;  
//    public AudioClip ClickAudio;

//    public void ClickAudioOn()
//    {
//        if (m_audioSource != null && ClickAudio != null)
//        {
//            m_audioSource.PlayOneShot(ClickAudio);
//        }
//        else
//        {
//            Debug.LogWarning("AudioSource o AudioClip no asignado.");
//        }
//    }

//    public void Construtc(Question q, Action<OptionButton> callback)
//    {
//        if (q == null)
//        {
//            Debug.LogError("Question object is null");
//            return;
//        }

//        if (m_buttonList == null || m_buttonList.Count < 2)
//        {
//            Debug.LogError("Se necesitan al menos 2 botones en la interfaz");
//            return;
//        }

//        if (q.options == null || q.options.Count < 2)
//        {
//            Debug.LogError("Se necesitan al menos 2 opciones para mostrar");
//            return;
//        }
//        if (m_buttonList.Count != q.options.Count)
//        {
//            Debug.LogError($"Mismatch entre botones ({m_buttonList.Count}) y opciones ({q.options.Count})");
//            return;
//        }

//        if (m_questionImage != null)
//        {
//            if (q.image != null)
//            {
//                m_questionImage.sprite = q.image;
//            }

//            if (q.audioClip != null)
//            {
//                ClickAudio = q.audioClip;
//            }
//            else
//            {
//                ClickAudio = null;
//            }
//        }
//        else
//        {
//            Debug.LogError("Question Image component is not assigned");
//        }

//        if (m_question != null)
//        {
//            m_question.text = q.text;
//        }
//        else
//        {
//            Debug.LogError("Question Text component is not assigned");
//        }

//        List<Option> twoOptions = GetCorrectAndIncorrectOptions(q.options);
//        ShuffleList(twoOptions);

//        for (int i = 0; i < m_buttonList.Count; i++)
//        {
//            if (i < 2)
//            {
//                m_buttonList[i].gameObject.SetActive(true);
//                m_buttonList[i].Construtc(twoOptions[i], callback);
//            }
//            else
//            {
//                m_buttonList[i].gameObject.SetActive(false);
//            }
//        }
//    }

//    private List<Option> GetCorrectAndIncorrectOptions(List<Option> options)
//    {
//        Option correctOption = options.Find(o => o.correct);
//        if (correctOption == null)
//        {
//            Debug.LogError("No se encontró opción correcta en la lista");
//            return options.GetRange(0, Math.Min(2, options.Count));
//        }

//        List<Option> incorrectOptions = options.FindAll(o => !o.correct);
//        if (incorrectOptions.Count == 0)
//        {
//            Debug.LogError("No hay opciones incorrectas disponibles");
//            return new List<Option> { correctOption, null };
//        }

//        System.Random rng = new System.Random();
//        Option randomIncorrect = incorrectOptions[rng.Next(incorrectOptions.Count)];

//        return new List<Option> { correctOption, randomIncorrect };
//    }

//    private void ShuffleList<T>(List<T> list)
//    {
//        System.Random rng = new System.Random();
//        int n = list.Count;
//        while (n > 1)
//        {
//            n--;
//            int k = rng.Next(n + 1);
//            T value = list[k];
//            list[k] = list[n];
//            list[n] = value;
//        }
//    }
//}
