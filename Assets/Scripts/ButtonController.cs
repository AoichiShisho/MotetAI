using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonController : EventTrigger
{
    public override void OnPointerDown(PointerEventData data)
    {
        foreach (Transform child in GetComponentInChildren<Transform>())
        {
            if (child.gameObject != gameObject)
                child.transform.position += new Vector3(0.5f, -0.5f, 0);
        }
        PlayButtonSound();
    }

    public override void OnPointerUp(PointerEventData data)
    {
        foreach (Transform child in GetComponentInChildren<Transform>())
        {
            if (child.gameObject != gameObject)
                child.transform.position -= new Vector3(0.5f, -0.5f, 0);
        }
    }

    void PlayButtonSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayButtonSound();
        }
    }
}
