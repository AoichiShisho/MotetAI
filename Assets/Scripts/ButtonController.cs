using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonController : EventTrigger
{
    public Transform buttonTextTransform;

    void Start()
    {
        buttonTextTransform = GetComponentInChildren<Transform>();
    }

    public override void OnPointerDown(PointerEventData data)
    {
        buttonTextTransform.transform.position += new Vector3(0.5f, -0.5f, 0);
        PlayButtonSound();
    }

    public override void OnPointerUp(PointerEventData data)
    {
        buttonTextTransform.transform.position -= new Vector3(0.5f, -0.5f, 0);
    }

    void PlayButtonSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonSound();
        }
    }
}
