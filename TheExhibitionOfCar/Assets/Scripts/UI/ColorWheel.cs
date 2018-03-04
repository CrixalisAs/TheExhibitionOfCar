using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorWheel : MonoBehaviour
{

    public Transform[] colorTfs;
    public Transform rootTransform;
    private bool isOpened = false;
    public ColorWheelAnim anim;
    public Color[] colors;

    void Start()
    {
        InitColliderAddListeners();
        SetBeginState();
        EventCenter.ConfigEvent.ClickBodyEvent += ClickBodyEvent;
    }
    private void ClickBodyEvent(bool click)
    {
        if (click)
        {
            if (!isOpened)
            {
                isOpened = true;
                SetPosition();
                ShowColorWheel(true);
            }
            else
            {
                ShowColorWheel(false);
                isOpened = false;
            }
        }
        MouseLock.Instance.Lock(2);
    }

    private void InitColliderAddListeners()
    {
        int count = colorTfs.Length;
        for (int i = 0; i < count; i++)
        {
            colorTfs[i].gameObject.AddComponent<MeshCollider>();
            EventTriggerListener.Get(colorTfs[i].gameObject).onClick += OnClickChild;
        }
    }

    private void SetBeginState()
    {
        anim.SetBeginState(colorTfs);
        rootTransform.gameObject.SetActive(false);
    }
    private void OnClickChild(GameObject go, PointerEventData eventdata)
    {
        int colorIndex = int.Parse(go.name);
        EventCenter.ConfigEvent.RaiseClickColorWheel(colors[colorIndex],eventdata);
        ShowColorWheel(false);
        isOpened = false;
    }

    private void ShowColorWheel(bool show)
    {
        if (show)
        {
            anim.PlayForward(colorTfs);
            rootTransform.gameObject.SetActive(true);
            SoundManager.instance.PlayColorWheelShow();
        }
        else
        {
            anim.PlayBackward(colorTfs,()=>rootTransform.gameObject.SetActive(false));
        }
    }

    private void SetPosition()
    {
        Canvas canvas = ScreenResize.instance.canvas;
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera, out pos);
        rootTransform.localPosition = new Vector3(pos.x,pos.y,0);
            
    }
}
