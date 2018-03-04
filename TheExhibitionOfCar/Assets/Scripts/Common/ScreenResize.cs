using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResize : MonoBehaviour
{

    public static ScreenResize instance;
    private int m_ActiveHeight;
    private int m_ActiveWidth;
    public CanvasScaler canvasScaler;
    private float m_AspectRatio;
    public Canvas canvas;

    public int activeWidth
    {
        get { return m_ActiveWidth; }
    }

    public int activeHeight
    {
        get { return m_ActiveHeight; }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        m_ActiveWidth = (int)canvasScaler.referenceResolution.x;
        m_AspectRatio = (float)Screen.height / Screen.width;
        m_ActiveHeight = (int)(m_ActiveWidth * m_AspectRatio);
    }
    
}
