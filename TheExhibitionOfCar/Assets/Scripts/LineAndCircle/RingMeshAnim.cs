using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RingMeshAnim : MeshAnimBase
{

    private RingMesh[] ringMeshes;
    private float[] endAngles;

    private int count;

    void Start()
    {
        ringMeshes = gameObject.GetComponentsInChildren<RingMesh>();
        count = ringMeshes.Length;
        endAngles=new float[count];
        for (int i = 0; i < count; i++)
        {
            endAngles[i] = ringMeshes[i].AngleEnd;
        }
        SetBeginState();
        if (animAtStart)
        {
            PlayForwardAnimation();
        }
    }

    public void StartAnimToAndBack()
    {
        PlayBackwardAnimation();
        Invoke("PlayForwardAnimation",maxDuration+maxDelay);
    }
    public override void StartAnimation(bool forward)
    {
        if (forward)
        {
            PlayForwardAnimation();
        }
        else
        {
            PlayBackwardAnimation();
        }
    }

    private void SetBeginState()
    {
        for (int i = 0; i < count; i++)
        {
            ringMeshes[i].AngleBegin = 0;
            ringMeshes[i].AngleEnd = 0;
        }
    }

    private void PlayForwardAnimation()
    {
        float duration;
        float delay;
        for (int i = 0; i < count; i++)
        {
            duration = Random.Range(minDuration, maxDuration);
            delay = Random.Range(minDelay, maxDelay);
            ringMeshes[i].DoEndAngle(endAngles[i], duration, delay);
        }
    }

    private void PlayBackwardAnimation()
    {
        float duration;
        float delay;
        for (int i = 0; i < count; i++)
        {
            duration = Random.Range(minDelay, maxDuration);
            delay = Random.Range(minDelay, maxDelay);
            ringMeshes[i].DoEndAngle(0, duration, delay);
        }
    }

}
