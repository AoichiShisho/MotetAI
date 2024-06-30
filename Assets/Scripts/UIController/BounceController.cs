using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BounceText : MonoBehaviour
{
    public Image uiText;

    void Start()
    {
        uiText.transform.localScale = Vector3.zero;
        
        uiText.transform.DOScale(Vector3.one, 1f)
            .SetEase(Ease.OutBounce);
    }
}