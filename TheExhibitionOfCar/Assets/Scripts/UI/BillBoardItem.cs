using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BillBoardItem : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public Text uiText;
    public Transform rearrangeTransform;
    public Transform cacheTransform;
    private int count;
    private float localPositionY = 60;
    private float delayRange = 0.5f;
    private float duration = 0.5f;

    void Start()
    {
        if (cacheTransform == null)
        {
            cacheTransform = transform;
        }
        if (rearrangeTransform == null)
        {
            rearrangeTransform = cacheTransform.GetChild(0);
        }
        canvasGroup.alpha = 0;
        rearrangeTransform.localPosition=new Vector3(0,localPositionY,0);
        gameObject.SetActive(false);
    }

    public void TweenAnim(bool forward)
    {
        float delay = Random.Range(0, delayRange);
        if (forward)
        {
            gameObject.SetActive(true);
            canvasGroup.DOFade(1, duration).SetDelay(delay);
            rearrangeTransform.DOLocalMoveY(0, duration).SetDelay(delay);
        }
        else
        {
            canvasGroup.DOFade(0, duration).SetDelay(delay).OnComplete(() => gameObject.SetActive(false));
            rearrangeTransform.DOLocalMoveY(localPositionY, duration).SetDelay(delay);
        }
    }

    public void SetPosition(Vector3 localPosition)
    {
        cacheTransform.localPosition = localPosition;
    }
}
