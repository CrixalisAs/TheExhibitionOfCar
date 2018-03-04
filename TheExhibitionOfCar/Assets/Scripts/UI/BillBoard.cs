using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{

    private Transform cacheTransform;
    private Transform[] targetTfs;
    private BillBoardItem[] billBoardItems;
    public GameObject itemObject;
    private int count;
    private bool showBillBoard = false;
    private Vector3 tempV3;

    void Awake()
    {
        cacheTransform = transform;
        EventCenter.BillBoardEvent.ShowBillBoardEvent += ShowBillBoard;
        EventCenter.BillBoardEvent.SetBillBoardTargetEvent += SetBillBoardTarget;
    }

    void Update()
    {
        if (showBillBoard)
        {
            SetPosition();
        }
    }
    private void SetBillBoardTarget(Transform[] targettfs)
    {
        targetTfs = targettfs;
        count = targettfs.Length;
        billBoardItems=new BillBoardItem[count];
        GameObject tempGo;
        for (int i = 0; i < count; i++)
        {
            tempGo = Instantiate(itemObject);
            billBoardItems[i] = tempGo.GetComponent<BillBoardItem>();
            billBoardItems[i].uiText.text = targettfs[i].gameObject.name.Replace("Billboard_", "");
            billBoardItems[i].transform.SetParent(cacheTransform);
            billBoardItems[i].transform.localScale=Vector3.one;
        }
    }

    private void ShowBillBoard(bool show)
    {
        showBillBoard = show;
        for (int i = 0; i < count; i++)
        {
            billBoardItems[i].TweenAnim(show);
        }
    }

    private void SetPosition()
    {
        Canvas canvas = ScreenResize.instance.canvas;
        Vector2 pos;
        for (int i = 0; i < count; i++)
        {
            tempV3 = Global.instance.mainCamera.WorldToScreenPoint(targetTfs[i].position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                new Vector2(tempV3.x,tempV3.y), canvas.worldCamera, out pos);
            tempV3 = new Vector3(pos.x, pos.y, 0);
            billBoardItems[i].SetPosition(tempV3);
        }
    }
    void OnDestroy()
    {
        EventCenter.BillBoardEvent.ShowBillBoardEvent -= ShowBillBoard;
        EventCenter.BillBoardEvent.SetBillBoardTargetEvent -= SetBillBoardTarget;
    }
}
