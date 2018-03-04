using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CameraParameter
{
    public bool limitXAngle;
    public float minXAngle;
    public float maxXAngle;

    public bool limitYAngle;
    public float minYAngle;
    public float maxYAngle;

    public float orbitSensitive;
    public float mouseMoveRate;

    public CameraParameter(bool limitXAngle = true, float minXAngle = 0, float maxXAngle = 80, bool limitYAngle = false,
        float minYAngle = 0, float maxYAngle = 0,float orbitSensitive=10,float mouseMoveRate=0.3f)
    {
        this.limitXAngle = limitXAngle;
        this.minXAngle = minXAngle;
        this.maxXAngle = maxXAngle;
        this.limitYAngle = limitYAngle;
        this.minYAngle = minYAngle;
        this.maxYAngle = maxYAngle;
        this.orbitSensitive = orbitSensitive;
        this.mouseMoveRate = mouseMoveRate;
    }
}
public class CameraOrbit : MonoBehaviour
{

    private Vector3 lastMousePos;
    private Vector3 targetEularAngle;
    private Vector3 eularAngle;

    public CameraParameter freeOrbitParameter;
    private CameraParameter currentCameraParameter;

    public Transform cameraRootTf;
    public Transform cameraTf;

    private float cameraDistance;
    private float targetCameraDistance;

    private float lastTouchDistance;

    public float minDistance = 30;
    public float maxDistance = 100;
    public float mouseScrollRatio = 2;
    public float touchRatio = 0.1f;
    public float zoomSensitive = 3;

    private Quaternion originalRotate;

    public float[] yMinAngles;
    public float[] yMaxAngles;
    public bool[] isAlreadyFire;

    void Start()
    {
        originalRotate = cameraRootTf.rotation;
        cameraDistance = cameraTf.localPosition.z;
        targetCameraDistance = cameraDistance;
        currentCameraParameter = freeOrbitParameter;
        isAlreadyFire=new bool[yMinAngles.Length];
    }

    void Update()
    {
        Orbit();
        Zoom();
    }
    private void Orbit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            targetEularAngle.x += -(Input.mousePosition.y - lastMousePos.y) * currentCameraParameter.mouseMoveRate;
            targetEularAngle.y += (Input.mousePosition.x - lastMousePos.x) * currentCameraParameter.mouseMoveRate;
            if (currentCameraParameter.limitXAngle)
            {
                targetEularAngle.x = Mathf.Clamp(targetEularAngle.x, currentCameraParameter.minXAngle,
                    currentCameraParameter.maxXAngle);
            }
            if (currentCameraParameter.limitYAngle)
            {
                targetEularAngle.y = Mathf.Clamp(targetEularAngle.y, currentCameraParameter.minYAngle,
                    currentCameraParameter.maxYAngle);
            }
            lastMousePos = Input.mousePosition;
        }
        if (Input.touchCount < 2)
        {
            eularAngle = Vector3.Lerp(eularAngle, targetEularAngle,
                Time.fixedDeltaTime * currentCameraParameter.orbitSensitive);
            cameraRootTf.rotation = originalRotate * Quaternion.Euler(eularAngle);
        }
        FireEvent(cameraRootTf.localEulerAngles.y);
    }

    private void Zoom()
    {
        if (Input.touchCount < 2)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                cameraDistance = -cameraTf.localPosition.z;
                targetCameraDistance =
                    cameraDistance - Input.GetAxis("Mouse ScrollWheel") * cameraDistance * mouseScrollRatio;
                targetCameraDistance = Mathf.Clamp(targetCameraDistance, minDistance, maxDistance);
            }
        }
        else
        {
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                lastTouchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }
            if (Input.GetTouch(1).phase == TouchPhase.Moved||Input.GetTouch(0).phase==TouchPhase.Moved)
            {
                cameraDistance = -cameraTf.localPosition.z;
                targetCameraDistance = cameraDistance -
                                       (Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) -
                                        lastTouchDistance) * touchRatio;
                lastMousePos = Input.mousePosition;
            }
        }
        if (Mathf.Abs(targetCameraDistance - cameraDistance) > 0.1f)
        {
            cameraDistance = Mathf.Lerp(cameraDistance, targetCameraDistance, Time.fixedDeltaTime * zoomSensitive);
            cameraTf.localPosition=new Vector3(0,0,-cameraDistance);
        }
    }

    private void FireEvent(float yAngle)
    {
        for (int i = 0; i < yMinAngles.Length; i++)
        {
            if (yAngle > yMinAngles[i] && yAngle < yMaxAngles[i])
            {
                if (!isAlreadyFire[i])
                {
                    EventCenter.CameraEvent.RaiseCameraReachAngle(i,true);
                    isAlreadyFire[i] = true;
                }
            }
            else
            {
                if (isAlreadyFire[i])
                {
                    EventCenter.CameraEvent.RaiseCameraReachAngle(i,false);
                    isAlreadyFire[i] = false;
                }
            }
        }
    }
}
