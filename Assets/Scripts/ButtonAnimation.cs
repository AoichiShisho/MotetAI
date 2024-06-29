using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonAnimation : EventTrigger
{
    public TextMeshProUGUI buttonText;

    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void OnPointerDown(PointerEventData data)
    {
        buttonText.transform.position += new Vector3(0.5f, -0.5f, 0);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        buttonText.transform.position -= new Vector3(0.5f, -0.5f, 0);
    }
}