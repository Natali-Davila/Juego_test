////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;
////using UnityEngine.SceneManagement;
////using UnityEngine.UI;

////public class GameManagerMemorama : MonoBehaviour
////{
////    // Start is called before the first frame update
////    [SerializeField] private AudioClip correctSounds = null;
////    [SerializeField] private AudioClip incorrectSounds = null;
////    [SerializeField] private Color correctColors = Color.green;
////    [SerializeField] private Color incorrectColors = Color.red;
////    [SerializeField] private float waitTimes = 1.0f;
////    [SerializeField] private Text correctAnswerTexts = null;
////    [SerializeField] private Button exitButtons = null;
////    public GameObject[] hearts;
////    private int lifes;
////    private int correctAnswerCounts = 0;
////    private const int maxCorrectAnswerss = 10;

////    private QuizBDSonidos quizDBs = null;
////    private QuizUISonidos quizUIs = null;
////    private AudioSource audioSources = null;

////    private void Start()
////    {
////        audioSources = GetComponent<AudioSource>();
////        quizDBs = FindObjectOfType<QuizBDSonidos>();
////        quizUIs = FindObjectOfType<QuizUISonidos>();

////        if (quizDBs == null)
////        {
////            Debug.LogError("QuizDb not found in the scene.");
////            return;
////        }

////        if (quizUIs == null)
////        {
////            Debug.LogError("QuizUI not found in the scene.");
////            return;
////        }

////        lifes = hearts.Length;
////        UpdateHeartsS();

////        if (exitButtons != null)
////        {
////            exitButtons.onClick.AddListener(OnExitButtonClickedS);
////        }
////        else
////        {
////            Debug.LogError("ExitButton not assigned.");
////        }

////        NextQuestionS();
////    }

////    private void NextQuestionS()
////    {
////        quizUIs.Construtc(quizDBs.GetRandomS(), GiveAnswerS);
////    }

////    private void GiveAnswerS(OptionButtonSonidos optionButtonSonidos)
////    {
////        StartCoroutine(GiveAnswerRoutineS(optionButtonSonidos));
////    }

////    private IEnumerator GiveAnswerRoutineS(OptionButtonSonidos optionButtonSonidos)
////    {
////        if (audioSources.isPlaying)
////            audioSources.Stop();

////        audioSources.clip = optionButtonSonidos.Option.correct ? correctSounds : incorrectSounds;
////        optionButtonSonidos.SetColor(optionButtonSonidos.Option.correct ? correctColors : incorrectColors);

////        audioSources.Play();

////        yield return new WaitForSeconds(waitTimes);

////        if (optionButtonSonidos.Option.correct)
////        {
////            IncrementCorrectAnswerCountS();
////            NextQuestionS();
////        }
////        else
////        {
////            lifes--;
////            UpdateHeartsS();

////            if (lifes <= 0)
////            {
////                GameOverS();
////            }
////        }
////    }

////    private void IncrementCorrectAnswerCountS()
////    {
////        if (correctAnswerCounts < maxCorrectAnswerss)
////        {
////            correctAnswerCounts++;
////            UpdateCorrectAnswerTextS();
////            if (correctAnswerCounts == 10)
////            {
////                GameOverS();
////            }
////        }
////    }

////    private void UpdateCorrectAnswerTextS()
////    {
////        if (correctAnswerTexts != null)
////        {
////            correctAnswerTexts.text = $"{correctAnswerCounts}/{maxCorrectAnswerss}";
////        }
////        else
////        {
////            Debug.LogWarning("Correct Answer Text is not assigned.");
////        }
////    }

////    private void UpdateHeartsS()
////    {
////        for (int i = 0; i < hearts.Length; i++)
////        {
////            hearts[i].SetActive(i < lifes);
////        }
////    }

////    private void GameOverS()
////    {
////        SceneManager.LoadScene(0);
////    }

////    private void OnExitButtonClickedS()
////    {
////        SceneManager.LoadScene(1);
////    }
////}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class GameManagerMemorama : MonoBehaviour
//{
//    [SerializeField] private Sprite bgImage;
//    [SerializeField] private Text scoreText;
//    [SerializeField] private GameObject[] hearts;
//    public Sprite[] puzzles;

//    private List<Sprite> gamePuzzles = new List<Sprite>();
//    private List<Button> btns = new List<Button>();

//    private bool firstGuess, secondGuess;
//    private int firstGuessIndex, secondGuessIndex;
//    private string firstGuessPuzzle, secondGuessPuzzle;

//    private int score = 0;
//    private int lives;
//    private int countGuesses;
//    private int countCorrectGuesses;
//    private int gameGuesses;

//    void Awake()
//    {
//        puzzles = Resources.LoadAll<Sprite>("Sprites/ropani");
//    }

//    void Start()
//    {
//        lives = hearts.Length;
//        GetButtons();
//        AddListeners();
//        AddGamePuzzles();
//        Shuffle(gamePuzzles);
//        gameGuesses = gamePuzzles.Count / 2;
//        UpdateScoreText();
//        UpdateHearts();
//    }

//    void GetButtons()
//    {
//        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
//        foreach (GameObject obj in objects)
//        {
//            Button btn = obj.GetComponent<Button>();
//            btns.Add(btn);
//            btn.image.sprite = bgImage;
//        }
//    }

//    void AddGamePuzzles()
//    {
//        int index = 0;
//        for (int i = 0; i < btns.Count; i++)
//        {
//            if (index == btns.Count / 2)
//            {
//                index = 0;
//            }
//            gamePuzzles.Add(puzzles[index]);
//            index++;
//        }
//    }

//    void AddListeners()
//    {
//        foreach (Button btn in btns)
//        {
//            btn.onClick.AddListener(() => PickAPuzzle());
//        }
//    }

//    public void PickAPuzzle()
//    {
//        if (!firstGuess)
//        {
//            firstGuess = true;
//            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
//            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
//            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
//        }
//        else if (!secondGuess)
//        {
//            secondGuess = true;
//            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
//            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
//            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];
//            countGuesses++;
//            StartCoroutine(CheckIfThePuzzlesMatch());
//        }
//    }

//    IEnumerator CheckIfThePuzzlesMatch()
//    {
//        yield return new WaitForSeconds(1f);
//        if (firstGuessPuzzle == secondGuessPuzzle)
//        {
//            yield return new WaitForSeconds(0.5f);
//            btns[firstGuessIndex].interactable = false;
//            btns[secondGuessIndex].interactable = false;
//            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
//            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

//            countCorrectGuesses++;
//            score += 10;
//            UpdateScoreText();
//            CheckIfTheGameIsFinished();
//        }
//        else
//        {
//            yield return new WaitForSeconds(0.5f);
//            btns[firstGuessIndex].image.sprite = bgImage;
//            btns[secondGuessIndex].image.sprite = bgImage;

//            lives--;
//            UpdateHearts();
//            if (lives <= 0)
//            {
//                GameOver();
//            }
//        }
//        yield return new WaitForSeconds(0.5f);
//        firstGuess = secondGuess = false;
//    }

//    void UpdateScoreText()
//    {
//        scoreText.text = "Score: " + score;
//    }

//    void UpdateHearts()
//    {
//        for (int i = 0; i < hearts.Length; i++)
//        {
//            hearts[i].SetActive(i < lives);
//        }
//    }

//    void CheckIfTheGameIsFinished()
//    {
//        if (countCorrectGuesses == gameGuesses)
//        {
//            Debug.Log("Game Finished");
//            Debug.Log("It took you " + countGuesses + " guesses to finish the game");
//        }
//    }

//    void GameOver()
//    {
//        Debug.Log("Game Over");
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }

//    void Shuffle(List<Sprite> list)
//    {
//        for (int i = 0; i < list.Count; i++)
//        {
//            Sprite temp = list[i];
//            int randomIndex = UnityEngine.Random.Range(i, list.Count);
//            list[i] = list[randomIndex];
//            list[randomIndex] = temp;
//        }
//    }
//}
