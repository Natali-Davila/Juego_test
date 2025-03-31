
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class GameController : MonoBehaviour
//{
//    [SerializeField] private Sprite bgImage;
//    [SerializeField] private Text scoreText = null;
//    [SerializeField] private Button exitButton = null;
//    public GameObject[] hearts;
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
//        scoreText.text = "puntuación: " + score;
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
//        SceneManager.LoadScene(0);
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

//    private void OnExitButtonClickedS()
//    {
//        SceneManager.LoadScene(1);
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Sprite bgImage;
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Button exitButton = null;
    public GameObject[] hearts;
    public Sprite[] puzzles;

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
        scoreText.text = "Score: " + score;
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
            SceneManager.LoadScene(0);
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene(1);
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

    private void OnExitButtonClickedS()
    {
        SceneManager.LoadScene(1);
    }
}
