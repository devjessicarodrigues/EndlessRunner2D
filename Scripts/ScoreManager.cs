using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public Button playAgainButton;
    public Button quitButton;
    public AudioClip buttonClickSound;
    public PlayerController playerController;

    private float score;
    private float previousX;
    private bool isGameOver = false;
    private bool isScoringStarted = false;
    private AudioSource audioSource;

    void Start()
    {
        previousX = player.position.x;
        score = 0;
        gameOverPanel.SetActive(false);

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        playAgainButton.Select();
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (!isScoringStarted && player.position.x >= 10.34936f)
            {
                isScoringStarted = true;
                previousX = player.position.x;
            }

            if (isScoringStarted && player.position.x > previousX)
            {
                score += player.position.x - previousX;
                previousX = player.position.x;

                if (scoreText != null)
                {
                    scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (isGameOver)
            {
                RestartGame();
                QuitToMainMenu();
            }
        }

        navigateWithLeftStick();

        handleButtonClick();
    }

    public void GameOver()
    {
        isGameOver = true;
        playerController.PauseMovement();

        int finalScore = Mathf.FloorToInt(score);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (finalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            PlayerPrefs.Save();
        }

        gameOverPanel.SetActive(true);

        finalScoreText.text = finalScore.ToString();
        highScoreText.text = highScore.ToString();

        playAgainButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitToMainMenu);
    }

    public void RestartGame()
    {
        PlayButtonClickSound();
        SceneManager.LoadScene("Game");
    }

    public void QuitToMainMenu()
    {
        PlayButtonClickSound();
        SceneManager.LoadScene("MainMenu");
    }

    private void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    private void navigateWithLeftStick()
    {
        if (isGameOver)
        {
            float horizontal = Input.GetAxis("Horizontal");

            if (horizontal < 0)
            {
                playAgainButton.Select();
            }
            else if (horizontal < 0)
            {
                quitButton.Select();
            }
        }
    }

    private void handleButtonClick()
    {
        if (isGameOver && gameOverPanel.activeSelf)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Button selectedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

                if (selectedButton != null)
                {
                    selectedButton.onClick.Invoke();  
                }
            }
        }
    }
}
