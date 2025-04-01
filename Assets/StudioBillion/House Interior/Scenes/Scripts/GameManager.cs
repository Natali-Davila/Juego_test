using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    //Se declaran las variables necesarias para el juego
    [SerializeField] private AudioClip correctSound = null;
    [SerializeField] private AudioClip incorrectSound = null;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color incorrectColor = Color.red;
    [SerializeField] private float waitTime = 1.0f;
    [SerializeField] private Text correctAnswerText = null;
    [SerializeField] private Button exitButton = null;
    //panel de instucciones con boton
    [SerializeField] private Button instructionsButton = null;
    [SerializeField] private GameObject instructionsPanel = null;
    [SerializeField] private Button closeInstructionsButton = null;

    public GameObject[] hearts;
    private int life;
    private int correctAnswerCount = 0;
    private const int maxCorrectAnswers = 10;

    private QuizDb quizDB = null;
    private QuizUI quizUI = null;
    private AudioSource audioSource = null;

    //Cuando pierde el usuario
    [SerializeField] private GameObject gameOverPanel = null;
    [SerializeField] private Button restartButton = null;
    [SerializeField] private Button goToMenuButton = null;

    //Cuando gana el usuario
    [SerializeField] private GameObject victoryPanel = null;
    [SerializeField] private Text victoryText = null;
    [SerializeField] private Button victoryExitButton = null;
    [SerializeField] private Button victoryRestartButton = null;

    //Cuando el usuario quiere salir
    [SerializeField] private GameObject exitConfirmationPanel = null;
    [SerializeField] private Button confirmExitButton = null;
    [SerializeField] private Button cancelExitButton = null;

    //Se inicializan los valores necesarios para el juego
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
            confirmExitButton.onClick.AddListener(OnConfirmExit);
        }
        else
        {
            Debug.LogError("Confirm Exit Button not assigned.");
        }

        if (cancelExitButton != null)
        {
            cancelExitButton.onClick.AddListener(OnCancelExit);
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
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }
        else
        {
            Debug.LogError("RestartButton not assigned.");
        }

        if (goToMenuButton != null)
        {
            goToMenuButton.onClick.AddListener(OnGoToMenuButtonClicked);
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
            victoryRestartButton.onClick.AddListener(OnRestartButtonClicked);
        }
        else
        {
            Debug.LogError("Victory Restart Button not assigned.");
        }

        if (instructionsButton != null)
        {
            instructionsButton.onClick.AddListener(ShowInstructions);
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
            closeInstructionsButton.onClick.AddListener(HideInstructions);
        }
        else
        {
            Debug.LogError("Close Instructions Button not assigned.");
        }

        NextQuestion();
    }

    //Se pasa a la siguiente pregunta
    private void NextQuestion()
    {
        quizUI.Construtc(quizDB.GetRandom(), GiveAnswer);
    }

    //Se da la respuesta a la pregunta
    private void GiveAnswer(OptionButton optionButton)
    {
        StartCoroutine(GiveAnswerRoutine(optionButton));
    }

    //Se da la respuesta a la pregunta y sonido de correcto o incorrecto
    private IEnumerator GiveAnswerRoutine(OptionButton optionButton)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = optionButton.Option.correct ? correctSound : incorrectSound;
        optionButton.SetColor(optionButton.Option.correct ? correctColor : incorrectColor);

        audioSource.Play();

        yield return new WaitForSeconds(waitTime);

        //Si la respuesta es correcta se incrementa el contador de respuestas correctas y se pasa a la siguiente pregunta
        if (optionButton.Option.correct)
        {
            IncrementCorrectAnswerCount();
            NextQuestion();
        }
        //Si la respuesta es incorrecta se decrementa la vida y se actualizan los corazones
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

    //Se incrementa el contador de respuestas correctas
    private void IncrementCorrectAnswerCount()
    {
        if (correctAnswerCount < maxCorrectAnswers)
        {
            correctAnswerCount++;
            UpdateCorrectAnswerText();
            if (correctAnswerCount == maxCorrectAnswers)
            {
                GameOver();
            }
        }
    }

    //Se actualiza el texto de respuestas correctas
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

    //Se actualizan los corazones(vidas)
    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < life);
        }
    }

    //Se muestra la pantalla de Game Over o Victory
    private void GameOver()
    {
        if (correctAnswerCount == maxCorrectAnswers && life > 0)
        {
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
                if (victoryText != null)
                {
                    victoryText.text = $"¡Ganaste! Tu puntuación es: {correctAnswerCount}/{maxCorrectAnswers}";
                }
                else
                {
                    Debug.LogError("Victory Text not assigned.");
                }
            }
            else
            {
                Debug.LogError("Victory Panel not assigned.");
            }
        }
        else
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
            else
            {
                Debug.LogError("Game Over Panel not assigned.");
            }
        }
    }

    //Se muestra la pantalla de confirmación de salida
    private void OnExitButtonClicked()
    {
        if (exitConfirmationPanel != null)
        {
            exitConfirmationPanel.SetActive(true);
        }
    }

    //Se sale del juego
    private void OnConfirmExit()
    {
        SceneManager.LoadScene(1);
    }

    //Se cancela la salida del juego
    private void OnCancelExit()
    {
        if (exitConfirmationPanel != null)
        {
            exitConfirmationPanel.SetActive(false);
        }
    }

    //Se reinicia el juego
    private void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Se va al menú principal
    private void OnGoToMenuButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    private void ExitToMenu()
    {
        SceneManager.LoadScene(1);
    }

    //se muestran las instrucciones
    private void ShowInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
        }
    }

    //Se ocultan las instrucciones
    private void HideInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
    }
}
