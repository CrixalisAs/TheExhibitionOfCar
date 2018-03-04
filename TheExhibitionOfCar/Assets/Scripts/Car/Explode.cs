using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Explode : MonoBehaviour
{

    public Transform[] childrens;
    public Transform cacheTransform;
    private Vector3[] originalPoses;
    private Vector3[] originalScales;
    private int childCount;
    private Vector3 targetPos;
    public float distance;
    public Vector3 offsetAxis;

    void Start()
    {
        cacheTransform = transform;
        childCount = cacheTransform.childCount;
        childrens=new Transform[childCount];
        originalPoses=new Vector3[childCount];
        originalScales=new Vector3[childCount];
        for (int i = 0; i < childCount; i++)
        {
            childrens[i] = cacheTransform.GetChild(i);
            originalPoses[i] = childrens[i].position;
            originalScales[i] = childrens[i].localScale;
        }
        EventCenter.UIEvent.ExplodeEvent += StartExlpode;
    }

    private void StartExlpode(bool @on)
    {
        if (on)
        {
            Disassamble();
        }
        else
        {
            Assamble();
        }
    }

    private void Disassamble()
    {
        float delay;
        float duration = 0.3f;
        Vector3 targetPos;
        Vector3 targetScale;
        Transform tempTf;
        for (int i = 0; i < childCount; i++)
        {
            targetPos = originalPoses[i].MultiplyEachElement(offsetAxis) * distance + originalPoses[i];
            delay = Random.Range(0, 0.5f);
            tempTf = childrens[i];
            tempTf.DOMove(targetPos, duration).SetDelay(delay);
            targetScale=Vector3.zero;
            tempTf.DOScale(targetScale, duration).SetDelay(delay + duration);
        }
    }

    private void Assamble()
    {
        float delay;
        float duration = 0.3f;
        Vector3 targetPos;
        Vector3 targetScale;
        Transform tempTf;
        for (int i = 0; i < childCount; i++)
        {
            targetPos = originalPoses[i];
            delay = Random.Range(0, 0.5f);
            tempTf = childrens[i];
            tempTf.DOMove(targetPos, duration).SetDelay(delay+duration);
            targetScale=originalScales[i];
            tempTf.DOScale(targetScale, duration).SetDelay(delay);
        }
    }
    void OnDestroy()
    {
        EventCenter.UIEvent.ExplodeEvent -= StartExlpode;
    }
}
