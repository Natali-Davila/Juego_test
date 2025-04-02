using System.Collections;
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

    // Panel de instrucciones
    [SerializeField] private Button instructionsButton = null;
    [SerializeField] private GameObject instructionsPanel = null;
    [SerializeField] private Button closeInstructionsButton = null;

    // Cuando pierde el usuario
    [SerializeField] private GameObject gameOverPanel = null;
    [SerializeField] private Button restartButton = null;
    [SerializeField] private Button goToMenuButton = null;

    // Cuando gana el usuario
    [SerializeField] private GameObject victoryPanel = null;
    [SerializeField] private Text victoryText = null;
    [SerializeField] private Button victoryExitButton = null;
    [SerializeField] private Button victoryRestartButton = null;

    // Confirmación de salida
    [SerializeField] private GameObject exitConfirmationPanel = null;
    [SerializeField] private Button confirmExitButton = null;
    [SerializeField] private Button cancelExitButton = null;

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

        if (exitConfirmationPanel != null)
        {
            exitConfirmationPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Exit Confirmation Panel not assigned.");
        }

        if (confirmExitButton != null)
        {
            confirmExitButton.onClick.AddListener(OnConfirmExitS);
        }
        else
        {
            Debug.LogError("Confirm Exit Button not assigned.");
        }

        if (cancelExitButton != null)
        {
            cancelExitButton.onClick.AddListener(OnCancelExitS);
        }
        else
        {
            Debug.LogError("Cancel Exit Button not assigned.");
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClickedS);
        }
        else
        {
            Debug.LogError("RestartButton not assigned.");
        }

        if (goToMenuButton != null)
        {
            goToMenuButton.onClick.AddListener(OnGoToMenuButtonClickedS);
        }
        else
        {
            Debug.LogError("GoToMenuButton not assigned.");
        }

        if (victoryExitButton != null)
        {
            victoryExitButton.onClick.AddListener(ExitToMenu);
        }
        else
        {
            Debug.LogError("Victory Exit Button not assigned.");
        }

        if (victoryRestartButton != null)
        {
            victoryRestartButton.onClick.AddListener(OnRestartButtonClickedS);
        }
        else
        {
            Debug.LogError("Victory Restart Button not assigned.");
        }

        if (instructionsButton != null)
        {
            instructionsButton.onClick.AddListener(ShowInstructionsS);
        }
        else
        {
            Debug.LogError("Instructions Button not assigned.");
        }

        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Instructions Panel not assigned.");
        }

        if (closeInstructionsButton != null)
        {
            closeInstructionsButton.onClick.AddListener(HideInstructionsS);
        }
        else
        {
            Debug.LogError("Close Instructions Button not assigned.");
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
            if (correctAnswerCounts == maxCorrectAnswerss)
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
        if (correctAnswerCounts == maxCorrectAnswerss && lifes > 0)
        {
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
                if (victoryText != null)
                {
                    victoryText.text = " ";
                }
            }
        }
        else
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }
    }

    private void OnExitButtonClickedS()
    {
        if (exitConfirmationPanel != null)
        {
            exitConfirmationPanel.SetActive(true);
        }
    }

    private void OnConfirmExitS()
    {
        SceneManager.LoadScene(1);
    }

    private void OnCancelExitS()
    {
        if (exitConfirmationPanel != null)
        {
            exitConfirmationPanel.SetActive(false);
        }
    }

    private void OnRestartButtonClickedS()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnGoToMenuButtonClickedS()
    {
        SceneManager.LoadScene(1);
    }
    private void ExitToMenu()
    {
        SceneManager.LoadScene(1);
    }
   
    private void ShowInstructionsS()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
        }
    }

    private void HideInstructionsS()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
    }

}

