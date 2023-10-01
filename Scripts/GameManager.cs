using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Mole> moles;
    


    [Header("UI objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject GoBackButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject bombText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private AudioSource gameOver;
    [SerializeField] private AudioSource bgmusic;
    [SerializeField] private AudioSource hitSound;
    
    // mole positions sounds
    [SerializeField] private AudioSource OneOne;
    [SerializeField] private AudioSource OneTwo;
    [SerializeField] private AudioSource OneThree;
    [SerializeField] private AudioSource TwoOne;
    [SerializeField] private AudioSource TwoTwo;
    [SerializeField] private AudioSource TwoThree;
    [SerializeField] private AudioSource ThreeOne;
    [SerializeField] private AudioSource ThreeTwo;
    [SerializeField] private AudioSource ThreeThree;


    // Hardcoded variables you may want to tune.
    private float startingTime = 30f;

    private int moleIndex;

    // Global variables
    private float timeRemaining;
    private HashSet<Mole> currentMoles = new HashSet<Mole>();
    private int score;
    private bool playing = false;
    private const string HighScoreKey = "HighScore";
    // This is public so the play button can see it.

    public void SetMoleIndex(int index)
    {
        moleIndex = index;
    }

    public int GetMoleIndex()
    {
        return moleIndex;
    }
    private int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    private void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(HighScoreKey, score);
    }
    public void StartGame()
    {
        // Hide/show the UI elements we don't/do want to see.
        playButton.SetActive(false);
        GoBackButton.SetActive(false);
        outOfTimeText.SetActive(false);
        bombText.SetActive(false);
        gameUI.SetActive(true);

        bgmusic.Play();
        // Hide all the visible moles.
        for (int i = 0; i < moles.Count; i++)
        {
            moles[i].Hide();
            moles[i].SetIndex(i);
            
        }
        // Remove any old game state.
        currentMoles.Clear();
        // Start with 30 seconds.
        timeRemaining = startingTime;
        score = 0;
        //scoreText.text = "0";
        playing = true;
    }

    public void GameOver(int type)
    {
        // Show the message.
        if (type == 0)
        {
            outOfTimeText.SetActive(true);
        }
        else
        {
            bombText.SetActive(true);
        }
        // Hide all moles.
        foreach (Mole mole in moles)
        {
            mole.StopGame();
        }
        int currentHighScore = GetHighScore();
        if (score > currentHighScore)
        {
            SaveHighScore(score);
        }
        // Stop the game and show the start UI.
        playing = false;
        bgmusic.Pause();
        gameOver.Play();
        playButton.SetActive(true);
        GoBackButton.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            // Update time.
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver(0);
            }
            timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
            // Check if we need to start any more moles.
            if (currentMoles.Count <= (score / 10))
            {
                // Choose a random mole.
                int index = Random.Range(0, moles.Count);
                int currentMoleIndex = GetMoleIndex();
                // Debug.Log("Current Mole Index: " + index);
                if (index == 5)
                {
                    ThreeOne.Play();
                    Debug.Log("top left");
                }
                else if (index == 3)
                {
                    ThreeTwo.Play();
                    Debug.Log("top middle");
                }
                else if (index == 4)
                {
                    ThreeThree.Play();
                    Debug.Log("top right");
                }
                else if (index == 2)
                {
                    TwoOne.Play();
                    Debug.Log("middle left");
                }
                else if (index == 0)
                {
                    TwoTwo.Play();
                    Debug.Log("middle");
                }
                else if (index == 1)
                {
                    TwoThree.Play();
                    Debug.Log("middle right");
                }
                else if (index == 8)
                {
                    OneOne.Play();
                    Debug.Log("bottom left");
                }
                else if (index == 6)
                {
                    OneTwo.Play();
                    Debug.Log("bottom middle");
                }
                else if (index == 7)
                {
                    OneThree.Play();
                    Debug.Log("bottom right");
                }
                // Doesn't matter if it's already doing something, we'll just try again next frame.
                if (!currentMoles.Contains(moles[index]))
                {
                    currentMoles.Add(moles[index]);
                    moles[index].Activate(score / 10);
                }
            }
        }
    }

    public void AddScore(int moleIndex)
    {
        // Add and update score.
    score += 1;
    hitSound.Play();
    scoreText.text = $"{score}";
    
    // Increase time by a little bit.
    timeRemaining += 1;
    // Remove from active moles.
    currentMoles.Remove(moles[moleIndex]);
    }

    public void Missed(int moleIndex, bool isMole)
    {
        if (isMole)
        {
            // Decrease time by a little bit.
            timeRemaining -= 2;
        }
        // Remove from active moles.
        currentMoles.Remove(moles[moleIndex]);
    }

    public void GoBack()
    {
        
        SceneManager.LoadScene("Main Menu");
        GoBackButton.SetActive(false); // Replace "YourSceneName" with the desired scene name
    }

    public int GetSavedHighScore()
{
    return GetHighScore();
}
}
