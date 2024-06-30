using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonController : EventTrigger
{
    public TextMeshProUGUI buttonText;

    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void OnPointerDown(PointerEventData data)
    {
        buttonText.transform.position += new Vector3(0.5f, -0.5f, 0);
        PlayButtonSound();
    }

    public override void OnPointerUp(PointerEventData data)
    {
        buttonText.transform.position -= new Vector3(0.5f, -0.5f, 0);
    }

    void PlayButtonSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonSound();
        }
    }
}
