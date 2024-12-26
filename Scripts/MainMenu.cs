using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class MainMenu : MonoBehaviour
{
    [Header("Scene")]
    public string levelName;

    [Header("Audio")]
    public AudioClip backgroundMusic;
    public AudioClip buttonClickSound;

    private AudioSource audioSource;

    public GameObject panelMainMenu;
    public Button startButton;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }

        startButton.Select();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            StartGame();
        }
        NavigateWithGamepad();
    }

    private void NavigateWithGamepad()
    {
        if (Input.GetAxis("Vertical") < 0)
        {
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
    }

    public void StartGame()
    {
        PlayButtonSound();
        SceneManager.LoadScene(levelName);
    }

    private void PlayButtonSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
