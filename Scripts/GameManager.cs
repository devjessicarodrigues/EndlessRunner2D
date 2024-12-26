using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject img;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button soundButton;
    public SoundController soundController;
    public AudioClip buttonClickSound;
    public AudioClip gameplayMusic;
    private AudioSource audioSource;
    public PlayerController playerController; 

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (gameplayMusic != null)
        {
            audioSource.clip = gameplayMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        resumeButton.Select();
    }

    void Update()
    {
        callPauseResume();
        navigateWithLeftStick();
        handleButtonPress();
    }

    public void callPauseResume()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        PlayButtonClickSound();
        Time.timeScale = 0f;
        isPaused = true;
        img.SetActive(true);
        playerController.PauseMovement(); 
    }

    public void ResumeGame()
    {
        PlayButtonClickSound();
        Time.timeScale = 1f;
        isPaused = false;
        img.SetActive(false);
        playerController.ResumeMovement(); 
    }

    public void GoToMainMenu()
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
        if (isPaused)
        {
            float horizontal = Input.GetAxis("Horizontal");

            if (horizontal < 0)
            {
                if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == soundButton)
                {
                    mainMenuButton.Select();
                }
                else if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == mainMenuButton)
                {
                    resumeButton.Select();
                }
            }
            else if (horizontal > 0)
            {
                if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == resumeButton)
                {
                    mainMenuButton.Select();
                }
                else if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == mainMenuButton)
                {
                    soundButton.Select();
                }
            }
        }
    }

    private void handleButtonPress()
    {
        if (isPaused && img.activeSelf)
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
