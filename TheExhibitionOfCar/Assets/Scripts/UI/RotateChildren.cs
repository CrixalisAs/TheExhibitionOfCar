using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateChildren : MonoBehaviour
{

    public Transform[] children;
    private int count;
    public float minSpeed = 0.3f;
    public float maxSpeed = 0.6f;
    private float[] speeds;

    void Start()
    {
        count = transform.childCount;
        children=new Transform[count];
        speeds=new float[count];
        for (int i = 0; i < count; i++)
        {
            children[i] = transform.GetChild(i);
        }
        for (int i = 0; i < count; i++)
        {
            speeds[i] = Random.Range(minSpeed, maxSpeed);
        }
    }

    void Update()
    {
        for (int i = 0; i < count; i++)
        {
            children[i].Rotate(0,speeds[i],0);
        }
    }
}
