using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMagerSonidos : MonoBehaviour
{
    [SerializeField] private AudioClip correctSounds = null;
    [SerializeField] private AudioClip incorrectSounds = null;
    [SerializeField] private Color correctColors = Color.green;
    [SerializeField] private Color incorrectColors = Color.red;
    [SerializeField] private float waitTimes = 1.0f;
    [SerializeField] private Text correctAnswerTexts = null;
    [SerializeField] private Button exitButtons = null;
    public GameObject[] hearts;
    private int lifes;
    private int correctAnswerCounts = 0;
    private const int maxCorrectAnswerss = 10;

    private QuizBDSonidos quizDBs = null;
    private QuizUISonidos quizUIs = null;
    private AudioSource audioSources = null;

    private void Start()
    {
        audioSources = GetComponent<AudioSource>();
        quizDBs = FindObjectOfType<QuizBDSonidos>();
        quizUIs = FindObjectOfType<QuizUISonidos>();

        if (quizDBs == null)
        {
            Debug.LogError("QuizDb not found in the scene.");
            return;
        }

        if (quizUIs == null)
        {
            Debug.LogError("QuizUI not found in the scene.");
            return;
        }

        lifes = hearts.Length;
        UpdateHeartsS();

        if (exitButtons != null)
        {
            exitButtons.onClick.AddListener(OnExitButtonClickedS);
        }
        else
        {
            Debug.LogError("ExitButton not assigned.");
        }

        NextQuestionS();
    }

    private void NextQuestionS()
    {
        quizUIs.Construtc(quizDBs.GetRandomS(), GiveAnswerS);
    }

    private void GiveAnswerS(OptionButtonSonidos optionButtonSonidos)
    {
        StartCoroutine(GiveAnswerRoutineS(optionButtonSonidos));
    }

    private IEnumerator GiveAnswerRoutineS(OptionButtonSonidos optionButtonSonidos)
    {
        if (audioSources.isPlaying)
            audioSources.Stop();

        audioSources.clip = optionButtonSonidos.Option.correct ? correctSounds : incorrectSounds;
        optionButtonSonidos.SetColor(optionButtonSonidos.Option.correct ? correctColors : incorrectColors);

        audioSources.Play();

        yield return new WaitForSeconds(waitTimes);

        if (optionButtonSonidos.Option.correct)
        {
            IncrementCorrectAnswerCountS();
            NextQuestionS();
        }
        else
        {
            lifes--;
            UpdateHeartsS();

            if (lifes <= 0)
            {
                GameOverS();
            }
        }
    }

    private void IncrementCorrectAnswerCountS()
    {
        if (correctAnswerCounts < maxCorrectAnswerss)
        {
            correctAnswerCounts++;
            UpdateCorrectAnswerTextS();
            if (correctAnswerCounts == 10)
            {
                GameOverS();
            }
        }
    }

    private void UpdateCorrectAnswerTextS()
    {
        if (correctAnswerTexts != null)
        {
            correctAnswerTexts.text = $"{correctAnswerCounts}/{maxCorrectAnswerss}";
        }
        else
        {
            Debug.LogWarning("Correct Answer Text is not assigned.");
        }
    }

    private void UpdateHeartsS()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < lifes);
        }
    }

    private void GameOverS()
    {
        SceneManager.LoadScene(0);
    }

    private void OnExitButtonClickedS()
    {
        SceneManager.LoadScene(1);
    }
}
