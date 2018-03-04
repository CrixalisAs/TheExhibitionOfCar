using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILightFlare : MonoBehaviour
{

    public ButtonAnim anim;
    private bool isOpened = false;

    public void ClickButton()
    {
        if (isOpened)
        {
            EventCenter.UIEvent.RaiseLightFlare(false);
            anim.Close();
            isOpened = false;
            SoundManager.instance.PlayLightClose();
        }
        else
        {
            EventCenter.UIEvent.RaiseLightFlare(true);
            anim.Open();
            isOpened = true;
            SoundManager.instance.PlayLightOpen();
        }
        MouseLock.Instance.Lock(1.5f);
    }
}
