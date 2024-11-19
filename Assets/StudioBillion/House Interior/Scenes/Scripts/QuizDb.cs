using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizDb : MonoBehaviour
{
    [SerializeField] private List<Question> m_questionList = null;
    private List<Question> m_backup = null;

    private void Awake()
    {
        m_backup = m_questionList.ToList();
    }

    public Question GetRandom(bool remove = true)
    {
        if (m_questionList.Count == 0)
        {
            RestoreBackup();
            // If after restoring backup the list is still empty, handle this case
            if (m_questionList.Count == 0)
            {
                Debug.LogError("The question list is empty.");
                return null; // or handle appropriately in your application
            }
        }

        System.Random rnd = new System.Random();
        int index = rnd.Next(0, m_questionList.Count);

        if (index < 0 || index >= m_questionList.Count)
        {
            Debug.LogError("Random index out of range: " + index);
            return null; // or handle appropriately in your application
        }

        if (!remove)
            return m_questionList[index];

        Question q = m_questionList[index];
        m_questionList.RemoveAt(index);
        return q;
    }


    private void RestoreBackup()
    {
        m_questionList = m_backup.ToList();
    }

}
