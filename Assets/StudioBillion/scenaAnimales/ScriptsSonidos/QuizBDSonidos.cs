using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class QuizBDSonidos : MonoBehaviour
{
    [SerializeField] private List<QuestionSonidos> m_questionLists = null;
    private List<QuestionSonidos> m_backups = null;

    private void Awake()
    {
        m_backups = m_questionLists.ToList();
    }

    public QuestionSonidos GetRandomS(bool remove = true)
    {
        if (m_questionLists.Count == 0)
        {
            RestoreBackup();
            if (m_questionLists.Count == 0)
            {
                Debug.LogError("The question list is empty.");
                return null;
            }
        }

        System.Random rnd = new System.Random();
        int index = rnd.Next(0, m_questionLists.Count);

        if (index < 0 || index >= m_questionLists.Count)
        {
            Debug.LogError("Random index out of range: " + index);
            return null;
        }

        if (!remove)
            return m_questionLists[index];

        QuestionSonidos q = m_questionLists[index];
        m_questionLists.RemoveAt(index);
        return q;
    }

    private void RestoreBackup()
    {
        m_questionLists = m_backups.ToList();
    }
}
