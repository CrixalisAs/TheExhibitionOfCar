using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarBodyColor : MonoBehaviour
{

    private Transform cacheTransform;
    private int dragData = 5;
    private ColorItem[] items;
    private int count;
    private Color targetColor;
    private float startOffset = -5.5f;
    private float endOffset = 50;
    private float duration = 1.5f;

    void Start()
    {
        cacheTransform = transform;
        InitBodyChildren();
        EventCenter.ConfigEvent.ClickColorWheelEvent += ClickColorWheelEvent;
    }

    private void ClickColorWheelEvent(Color color, PointerEventData eventdata)
    {
        Ray ray = Global.instance.mainCamera.ScreenPointToRay(eventdata.position);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            for (int i = 0; i < count; i++)
            {
                items[i].SetStartPoint(hitInfo.point);
            }
        }
        for (int i = 0; i < count; i++)
        {
            items[i].SetTargetColor(targetColor);
        }
        targetColor = color;
        StartCoroutine("TweenOffset");
        SoundManager.instance.PlayCarColorChange();
    }
    private IEnumerator TweenOffset()
    {
        for (float i = 0; ; i+=Time.deltaTime)
        {
            for (int j = 0; j < count; j++)
            {
                float offset = EaseInCubic(i, duration, startOffset, endOffset);
                items[j].SetOffset(offset);
            }
            if (i >= duration)
            {
                TweenColorComplete();
                StopCoroutine("TweenOffset");
            }
            yield return 0;
        }
    }

    private void TweenColorComplete()
    {
        for (int i = 0; i < count; i++)
        {
            items[i].SetOffset(startOffset);
            items[i].SetMainColor(targetColor);
        }
    }
    private float EaseInCubic(float time, float duration,float start,float end)
    {
        return (end - start) * (time /= duration) * time * time + start;
    }
    private void InitBodyChildren()
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        int count = children.Length;
        List<ColorItem> colorItems=new List<ColorItem>();
        for (int i = 0; i < count; i++)
        {
            Transform child = children[i];
            if (child != cacheTransform)
            {
                if (child.name.StartsWith("Body"))
                {
                    child.gameObject.AddComponent<MeshCollider>();
                    EventTriggerListener.Get(child.gameObject).onClick += OnClickBody;
                    ColorItem item = child.gameObject.AddComponent<ColorItem>();
                    colorItems.Add(item);
                }
            }
        }
        items = colorItems.ToArray();
        this.count = items.Length;
    }

    private void OnClickBody(GameObject go, PointerEventData eventdata)
    {
        Vector2 delta = eventdata.position - eventdata.pressPosition;
        if (delta.x < dragData && delta.y < dragData)
        {
            EventCenter.ConfigEvent.RaiseClickBody(true);
        }
    }
}