using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject Canvas;
    public TMP_Text spaceMessage; 
    public string nextSceneName = "Game";

    void Start()
    {
        if (spaceMessage != null)
        {
            spaceMessage.text = "Press Enter key or X button on gamepad to play."; 
            spaceMessage.gameObject.SetActive(true); 
        }

        if (Canvas != null)
        {
            Canvas.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName); 
    }
}

