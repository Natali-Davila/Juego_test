using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioClip correctSound = null;
    [SerializeField] private AudioClip incorrectSound = null;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color incorrectColor = Color.red;
    [SerializeField] private float waitTime = 1.0f;
    [SerializeField] private Text correctAnswerText = null;
    [SerializeField] private Button exitButton = null; 
    public GameObject[] hearts;
    private int life;
    private int correctAnswerCount = 0;
    private const int maxCorrectAnswers = 10;

    private QuizDb quizDB = null;
    private QuizUI quizUI = null;
    private AudioSource audioSource = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        quizDB = FindObjectOfType<QuizDb>();
        quizUI = FindObjectOfType<QuizUI>();

        if (quizDB == null)
        {
            Debug.LogError("QuizDb not found in the scene.");
            return;
        }

        if (quizUI == null)
        {
            Debug.LogError("QuizUI not found in the scene.");
            return;
        }

        life = hearts.Length;
        UpdateHearts();

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        else
        {
            Debug.LogError("ExitButton not assigned.");
        }

        NextQuestion();
    }

    private void NextQuestion()
    {
        quizUI.Construtc(quizDB.GetRandom(), GiveAnswer);
    }

    private void GiveAnswer(OptionButton optionButton)
    {
        StartCoroutine(GiveAnswerRoutine(optionButton));
    }

    private IEnumerator GiveAnswerRoutine(OptionButton optionButton)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = optionButton.Option.correct ? correctSound : incorrectSound;
        optionButton.SetColor(optionButton.Option.correct ? correctColor : incorrectColor);

        audioSource.Play();

        yield return new WaitForSeconds(waitTime);

        if (optionButton.Option.correct)
        {
            IncrementCorrectAnswerCount();
            NextQuestion();
        }
        else
        {
            life--;
            UpdateHearts();

            if (life <= 0)
            {
                GameOver();
            }
        }
    }

    private void IncrementCorrectAnswerCount()
    {
        if (correctAnswerCount < maxCorrectAnswers)
        {
            correctAnswerCount++;
            UpdateCorrectAnswerText();
            if (correctAnswerCount == 10) 
            {
                GameOver();
            }
        }
    }

    private void UpdateCorrectAnswerText()
    {
        if (correctAnswerText != null)
        {
            correctAnswerText.text = $"{correctAnswerCount}/{maxCorrectAnswers}";
        }
        else
        {
            Debug.LogWarning("Correct Answer Text is not assigned.");
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < life);
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene(0); 
    }

    private void OnExitButtonClicked()
    {
        SceneManager.LoadScene(1); 
    }
}
