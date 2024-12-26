using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    public float blinkInterval = 1f;
    private TextMeshProUGUI text;
    private bool isVisible = true;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        InvokeRepeating("ToggleVisibility", 1f, blinkInterval);
    }

    void ToggleVisibility()
    {
        isVisible = !isVisible;
        text.enabled = isVisible;
    }
}
