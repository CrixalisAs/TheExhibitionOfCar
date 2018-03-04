using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Global instance;
    
    public Transform mainCameraTransform;
    public Camera mainCamera;
    public Camera uiCamera;

    void Awake()
    {
        instance = this;
    }
    
}
