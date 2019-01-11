using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const string SPAWN_MANAGER_ID = "SpawnManager";
    private const string ASTEROID_TAG = "Asteroid";
    private const int SCORE_VALUE = 10;
    private const float GAME_OVER_VIEW_TIME = 1.5f;

    [SerializeField]
    private SpawnManager spawnManager;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject globe;

    [SerializeField]
    private GameObject startMenu;
    
    [SerializeField]
    private GameObject gameOver;
    
    [SerializeField]
    private Text ScoreTextbox;

    [SerializeField]
    private AudioClip gameMusic;

    [SerializeField]
    private AudioClip menuMusic;

    private AudioSource audioSource;

    private int score;

    private float gameOverViewTimeStamp = 0;

    public enum GameState { Start, Game, GameOver };

    private GameState currentGameState;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
    }

    // Use this for initialization
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        
        //Play menu music
        audioSource.clip = menuMusic;
        audioSource.Play();

        currentGameState = GameState.Start;

        startMenu.SetActive(true);
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (currentGameState == GameState.Start)
            {
                ScoreTextbox.text = string.Format("SCORE: {0}", 0);

                currentGameState = GameState.Game;
                
                player.SetActive(true);
                globe.SetActive(true);
                
                startMenu.SetActive(false);
                spawnManager.StartSpawning();

                //Play game music
                audioSource.clip = gameMusic;
                audioSource.Play();
            }
            else if (currentGameState == GameState.GameOver) //Take player back to start screen
            {
                if (Time.time > (gameOverViewTimeStamp + GAME_OVER_VIEW_TIME))
                {
                    //Play menu music
                    audioSource.clip = menuMusic;
                    audioSource.Play();

                    gameOver.SetActive(false);
                    startMenu.SetActive(true);

                    currentGameState = GameState.Start;

                    ScoreTextbox.text = string.Empty;
                    score = 0;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Q) && currentGameState == GameState.Start)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                  Application.Quit();
#endif
        }
    }

    public void AddScore()
    {
        score += SCORE_VALUE;

        ScoreTextbox.text = string.Format("SCORE: {0}", score);
    }

    public void SetGameOver()
    {
        currentGameState = GameState.GameOver;
        gameOver.SetActive(true);
        CleanObjects();

        gameOverViewTimeStamp = Time.time;
    }

    private void CleanObjects() //To make sure that all the asteroid have been deleted after game over
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(ASTEROID_TAG);

        foreach (var gItem in gameObjects)
        {
            GameObject.Destroy(gItem);
        }
    }
}
