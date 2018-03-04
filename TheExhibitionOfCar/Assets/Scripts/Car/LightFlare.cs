using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LightFlare : MonoBehaviour
{

    private LensFlare[] flares;
    public Transform[] childrens;
    private Vector3[] originalPoses;
    private int count;
    private float positionMultiply = 2.5f;
    private bool opened = false;
    private float duration = 0.7f;
    private float delay;
    private Vector3 targetPos;
    private float flareDuration = 0.2f;
    public float sizeMultiply = 20;

    void Start()
    {
        flares = GetComponentsInChildren<LensFlare>();
        count = flares.Length;
        originalPoses=new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            originalPoses[i] = childrens[i].transform.position;
        }
        gameObject.SetActive(false);
        SetBeginState();
        EventCenter.UIEvent.LightFlareEvent+=LightFlareEvent;
        EventCenter.UIEvent.ExplodeEvent += ExplodeEvent;
    }

    private void ExplodeEvent(bool on)
    {
        if (on)
        {
            if (opened)
            {
                TurnOff();
            }
        }
    }

    private void LightFlareEvent(bool on)
    {
        if (on)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    private void SetBeginState()
    {
        for (int i = 0; i < count; i++)
        {
            childrens[i].position = originalPoses[i] * positionMultiply;
        }
    }

    private void TurnOn()
    {
        opened = true;
        gameObject.SetActive(true);
        for (int i = 0; i < count; i++)
        {
            delay = 0.05f * i;
            targetPos = originalPoses[i];
            childrens[i].DOMove(targetPos, duration).SetDelay(delay).SetEase(Ease.InCubic);
            flares[i].DoBrightness(1f, flareDuration).SetDelay(delay + duration - 0.1f);
        }
    }

    private void TurnOff()
    {
        for (int i = 0; i < count; i++)
        {
            delay = 0.05f * i + 0.2f;
            targetPos = originalPoses[i] * positionMultiply;
            childrens[i].DOMoveX(targetPos.x, duration).SetDelay(delay).SetEase(Ease.OutSine);
            childrens[i].DOMoveZ(targetPos.z, duration).SetDelay(delay).SetEase(Ease.OutSine);
            if (i == count - 1)
            {
                childrens[i].DOMoveY(targetPos.y, duration).SetDelay(delay).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    opened = false;
                    gameObject.SetActive(false);
                });
            }
            else
            {
                childrens[i].DOMoveY(targetPos.y, duration).SetDelay(delay).SetEase(Ease.InCubic);
            }
            flares[i].DoBrightness(1, flareDuration).SetDelay(delay - 0.2f);
            flares[i].DoBrightness(0, duration).SetDelay(delay+duration);

        }
    }

    void Update()
    {
        for (int i = 0; i < count; i++)
        {
            float size = -sizeMultiply * 1f / Global.instance.mainCameraTransform.localPosition.z;
            flares[i].brightness = Mathf.Lerp(flares[i].brightness, size, Time.fixedDeltaTime * 10);
        }
    }

    void OnDestroy()
    {
        EventCenter.UIEvent.LightFlareEvent -= LightFlareEvent;
        EventCenter.UIEvent.ExplodeEvent -= ExplodeEvent;
    }
}
