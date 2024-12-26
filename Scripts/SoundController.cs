using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    private bool isSoundOn = true; 
    public Image soundOffIcon;
    public Image soundOnIcon;

    void Start()
    {
        UpdateSoundIcons();
    }

    public void ToggleGameVolume()
    {
        isSoundOn = !isSoundOn;

        if (isSoundOn)
        {
            AudioListener.volume = 1f;
        }
        else
        {
            AudioListener.volume = 0f;
        }

        UpdateSoundIcons();
    }

    private void UpdateSoundIcons()
    {
        if (isSoundOn)
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;
        }
        else
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;
        }
    }
}


