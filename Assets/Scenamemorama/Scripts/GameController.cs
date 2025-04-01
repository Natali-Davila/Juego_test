using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Variables del juego
    [SerializeField] private Sprite bgImage;
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Button exitButton = null;
    public GameObject[] hearts;
    public Sprite[] puzzles;

    // Panel de instrucciones con botón
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

    // Cuando el usuario quiere salir
    [SerializeField] private GameObject exitConfirmationPanel = null;
    [SerializeField] private Button confirmExitButton = null;
    [SerializeField] private Button cancelExitButton = null;

    private List<Sprite> gamePuzzles = new List<Sprite>();
    private List<Button> btns = new List<Button>();

    private bool firstGuess, secondGuess;
    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessPuzzle, secondGuessPuzzle;

    private int score = 0;
    private int lives;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/ropani");
    }

    void Start()
    {
        lives = hearts.Length;
        GetButtons();
        AddListeners();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
        UpdateScoreText();
        UpdateHearts();

        if (exitButton != null)
            exitButton.onClick.AddListener(OnExitButtonClicked);

        if (exitConfirmationPanel != null)
            exitConfirmationPanel.SetActive(false);

        if (confirmExitButton != null)
            confirmExitButton.onClick.AddListener(OnConfirmExit);

        if (cancelExitButton != null)
            cancelExitButton.onClick.AddListener(OnCancelExit);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartButtonClicked);

        if (goToMenuButton != null)
            goToMenuButton.onClick.AddListener(OnGoToMenuButtonClicked);

        if (victoryExitButton != null)
            victoryExitButton.onClick.AddListener(OnExitButtonClicked);

        if (victoryRestartButton != null)
            victoryRestartButton.onClick.AddListener(OnRestartButtonClicked);

        if (instructionsButton != null)
            instructionsButton.onClick.AddListener(ShowInstructions);

        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        if (closeInstructionsButton != null)
            closeInstructionsButton.onClick.AddListener(HideInstructions);
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        foreach (GameObject obj in objects)
        {
            Button btn = obj.GetComponent<Button>();
            btns.Add(btn);
            btn.image.sprite = bgImage;
        }
    }

    void AddGamePuzzles()
    {
        int index = 0;
        for (int i = 0; i < btns.Count; i++)
        {
            if (index == btns.Count / 2)
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListeners()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle()
    {
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];
            countGuesses++;
            StartCoroutine(CheckIfThePuzzlesMatch());
        }
    }

    IEnumerator CheckIfThePuzzlesMatch()
    {
        yield return new WaitForSeconds(1f);
        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(0.5f);
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;
            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            countCorrectGuesses++;
            score += 1;
            UpdateScoreText();
            CheckIfTheGameIsFinished();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;

            lives--;
            UpdateHearts();
            if (lives <= 0)
            {
                GameOver();
            }
        }
        yield return new WaitForSeconds(0.5f);
        firstGuess = secondGuess = false;
    }

    void UpdateScoreText()
    {
        scoreText.text = "Puntuación: " + score;
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < lives);
        }
    }

    void CheckIfTheGameIsFinished()
    {
        if (countCorrectGuesses == gameGuesses)
        {
            Victory();
        }
    }

    void GameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    void Victory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            if (victoryText != null)
            {
                victoryText.text = $"¡Ganaste! Tu puntuación es: {score}";
            }
        }
    }

    private void OnExitButtonClicked()
    {
        if (exitConfirmationPanel != null)
        {
            exitConfirmationPanel.SetActive(true);
        }
    }

    private void OnConfirmExit()
    {
        SceneManager.LoadScene(1);
    }

    private void OnCancelExit()
    {
        if (exitConfirmationPanel != null)
        {
            exitConfirmationPanel.SetActive(false);
        }
    }

    private void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnGoToMenuButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    private void ShowInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
        }
    }

    private void HideInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
    }
    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
