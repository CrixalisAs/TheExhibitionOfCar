using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RingMesh : PrimitiveBase
{

    public int segment = 1;
    public float ringWidth;
    public float ringRadius;
    public float angleBegin;
    public float angleEnd;

    public float AngleBegin
    {
        get { return angleBegin; }
        set
        {
            angleBegin = value;
            UpdateShape();
        }
    }

    public float AngleEnd
    {
        get { return angleEnd; }
        set
        {
            angleEnd = value;
            UpdateShape();
        }
    }

    protected override void InitMesh()
    {
        if (cacheTransform == null || meshFilter == null)
        {
            Init();
        }
        segment = segment > 0 ? segment : 1;
        triangles=new int[segment*2*3];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 3;
        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 1;
        for (int i = 0; i < segment; i++)
        {
            int k = i * 6;
            int j = i * 2;
            triangles[k] = j;
            triangles[k + 1] = j + 2;
            triangles[k + 2] = j + 3;
            triangles[k + 3] = j;
            triangles[k + 4] = j + 3;
            triangles[k + 5] = j + 1;
        }
        int vertexCount = segment * 2 + 2;
        uvs=new Vector2[vertexCount];
        float singleUV = 1f / segment;
        float uvY = 0;
        for (int i = 0; i < vertexCount; i+=2)
        {
            uvs[i].x = 0f;
            uvs[i + 1].x = 1;
            uvs[i].y = uvY;
            uvs[i + 1].y = uvY;
            uvY += singleUV;
        }
        UpdateShape();
    }

    protected override void UpdateShape()
    {
        int vertexCount = segment * 2 + 2;
        vertices=new Vector3[vertexCount];
        float angle = angleBegin * Mathf.Deg2Rad;
        float wHalf = ringWidth * 0.5f;
        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        float minRingRadius = ringRadius - wHalf;
        float maxRingRadius = ringRadius + wHalf;
        Vector3 localPos = offset;
        float x = cos * minRingRadius+localPos.y;
        float y = localPos.y;
        float z = sin * minRingRadius + localPos.z;
        vertices[0].x = x;
        vertices[0].y = y;
        vertices[0].z = z;
        x = cos * maxRingRadius + localPos.x;
        z = sin * maxRingRadius + localPos.z;

        vertices[1].x = x;
        vertices[1].y = y;
        vertices[1].z = z;

        float singleAngle = (angleEnd - angleBegin) / segment * Mathf.Deg2Rad;
        for (int i = 0; i < segment; i++)
        {
            angle += singleAngle;
            sin = Mathf.Sin(angle);
            cos = Mathf.Cos(angle);
            x = cos * minRingRadius + localPos.x;
            y = localPos.y;
            z = sin * minRingRadius + localPos.z;
            vertices[i*2+2]=new Vector3(x,y,z);
            x = cos * maxRingRadius + localPos.x;
            y = localPos.y;
            z = sin * maxRingRadius + localPos.z;
            vertices[i*2+3]=new Vector3(x,y,z);
        }
        UpdateMesh();
    }

    public Tween DoEndAngle(float endValue,float duration,float delay)
    {
        return DOTween.To(() => AngleEnd, x => AngleEnd = x, endValue, duration).SetDelay(delay);
    }
}
